using Persiscope.Common;
using Persiscope.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Persiscope.Lib
{



    public class DataRepositorySnapshot : IDisposable
    {
        public long RecordDepth;

        public int[][] ChannelsSnapshot;
        public long[] ChannelsIndexes;//the index of first sample, used for ekg
        public bool[] ChannelsEnabled;//
        public CalibrationInfo[] CalibrationDatas;
        public double SampleRate;


        [Obsolete]
        public FftContext[] FftContexts;

        [Obsolete]
        public SignalPropertyList[] SignalProperties;
        

        public static DataRepositorySnapshot FromPool(long recordDepth, int channels)
        {
            var buf = new DataRepositorySnapshot();

            var max = channels;// LibRuntimeVariables.MaxChannelCount;

            buf.RecordDepth = recordDepth;

            buf.ChannelsSnapshot = new int[max][];
            buf.FftContexts = new FftContext[max];
            buf.SignalProperties = new SignalPropertyList[max];
            buf.ChannelsIndexes = new long[max];
            buf.ChannelsEnabled = new bool[max];

            for (int i = 0; i < channels; i++)
            {
                buf.ChannelsSnapshot[i] = ArrayPool.Int32((int)recordDepth);
                //this.FftContexts[chn] = new FftContext();
            }

            return buf;
        }

        public void Dispose()
        {
            for (var i = 0; i < ChannelsSnapshot.Length; i++)
            {
                if (ChannelsSnapshot[i] != null)
                {
                    ArrayPool.Return(ChannelsSnapshot[i]);
                    ChannelsSnapshot[i] = null;
                }
            }

            for (var i = 0; i < FftContexts.Length; i++)
            {
                if (FftContexts[i] != null)
                    FftContexts[i].Dispose();
            }
        }

        ~DataRepositorySnapshot()
        {
            Dispose();
        }

        public void Clear()
        {
            for (int i = 0; i < ChannelsSnapshot.Length; i++)
            {
                ChannelsSnapshot[i].HpClear();
            }

            for (int i = 0; i < FftContexts.Length; i++)
            {
                FftContexts[i].Context.HpClear();
                FftContexts[i].Magnitudes.HpClear();
                FftContexts[i].Phases.HpClear();
            }
        }


        public static DataRepositorySnapshot GenerateSnapshot(DataRepository repo/*, int[] channels*/)
        {
            //resortRingArray, only used for ekg

            var memoryDepth = repo.MemoryDepth;

            var shot = DataRepositorySnapshot.FromPool(memoryDepth, repo.Channels.Length);

            shot.SampleRate = repo.SampleRate;

            if (repo.CalibrationDatas != null)
                shot.CalibrationDatas = (CalibrationInfo[])repo.CalibrationDatas.Clone();

            //lock (repo.Lock)
            {
                for (var i = 0; i < shot.ChannelsSnapshot.Length; i++)
                //foreach (var ch in channels)
                {
                    int idx;

                    var ch = i;

                    repo.Channels[ch].CopyTo(shot.ChannelsSnapshot[ch], out idx);

                    shot.ChannelsIndexes[ch] = idx;
                    shot.ChannelsEnabled[ch] = repo.ChannelEnable[ch];
                }
            }

            //do not merge loops for performance

            //calculateProperties = false;

            /*
            if (calculateProperties)
            {
                foreach (var ch in channels)
                {
                    var samples = shot.ChannelsSnapshot[ch];
                    var fft = shot.FftContexts[ch] = FftContext.FromPool((int)memoryDepth);

                    FftwUtil.CalcFftSharp(samples, fft.Context);

                    fft.UpdateArrays();
                    shot.SignalProperties[ch] = SignalPropertyCalculator.Calculate(samples, fft, shot.SampleRate);
                }

            }
            */

            return shot;
        }
    }
}
