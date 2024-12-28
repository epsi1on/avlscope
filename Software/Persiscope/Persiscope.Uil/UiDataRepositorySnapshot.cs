using Persiscope.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Ui
{
    public class UiDataRepositorySnapshot:IDisposable
    {
        public static bool CanGenerate(DataRepositorySnapshot snapshot, ChannelInfo[] channels)
        {
            for (var i = 0; i < channels.Length; i++)
            {
                var ch = channels[i];

                if (!ch.Source.CanFillChannelReadouts(snapshot))
                    return false;
            }

            return true;
        }


        public static UiDataRepositorySnapshot Generate(DataRepositorySnapshot snapshot, ChannelInfo[] channels, bool calculateProperties)
        {
            var buf = new UiDataRepositorySnapshot();

            buf.Packages = new AnalogSamplePackage[channels.Length];

            for (var i = 0; i < channels.Length; i++)
            {
                var ch = channels[i];

                if (!ch.Source.CanFillChannelReadouts(snapshot))
                    continue;


                long l,si;
                double sr;
                bool en;

                ch.Source.GetInfo(snapshot, out l, out sr, out si, out en);

                var pkg = AnalogSamplePackage.FromPool(l, sr);

                pkg.StartIndex = si;

                pkg.ChannelInfo = ch;

                if (!en)
                    pkg.ChannelInfo.Enabled = en;

                if (en)
                {
                    pkg.ChannelInfo.Source.FillChannelReadouts(snapshot, pkg);

                    if (calculateProperties)
                    {
                        if (pkg.Fft == null)
                            pkg.Fft = FftContext.FromPool(pkg.Values.Length);

                        FftwUtil.CalcFftSharp(pkg.Values, pkg.Fft.Context);
                        pkg.Fft.UpdateArrays();

                        pkg.SignalPropertyList = SignalPropertyCalculator.Calculate(pkg.Values, pkg.Fft, pkg.SampleRate);
                    }
                }


                buf.Packages[i] = pkg;
            }

            return buf;
        }

        public void Dispose()
        {
            foreach (var item in Packages)
            {
                if (item != null) item.Dispose();
            }

            Packages = null;
        }

        public AnalogSamplePackage[] Packages;

        //public long RecordDepth;

        //public double SampleRate;

    }
}
