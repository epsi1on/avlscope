using Persiscope.Lib;
using Persiscope.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Common
{

    public class DataRepository
    {



        //public static readonly int RepoLength =
        //5_000_000 * 3;//1.5 M sample capacity, 3 sec for 500ksps
        //    5_00_000;//1.5 M sample capacity, 3 sec for 500ksps
        //public static readonly int ChannelCount = 1;
        //public int AdcSampleRate = 500_000;//Sps
        //public List<ChannelData> Channels = new List<ChannelData>();// [ChannelCount];
        //public int AdcMaxValue = 4096;//rpi pico: 4096, arduino nano 1024
        //public double AdcMaxVoltage = 3.3;//rpi pico: 3.3, arduino nano 1024
        //public ChannelData Channel1 = new ChannelData(RepoLength);
        //public ChannelData Channel2 = new ChannelData(RepoLength);
        //public ISampleRepository<float> SamplesF;//= new FixedLengthList<short>(RepoLength);
        //public ISampleRepository<short> Samples;//= new FixedLengthList<short>(RepoLength);

        //updated by daq interface regularely, per hardware channel
        public CalibrationInfo[] CalibrationDatas;

        public bool[] ChannelEnable;

        public readonly object Lock = new object();

        //public ISampleRepository<int> Samples { get { return Channels[0]; } }

        public ISampleRepository<int>[] Channels;

        public long MemoryDepth;

        public double SampleRate;

        //public double LastAlphaCh1, LastBetaCh1;
        //public double LastAlphaCh2, LastBetaCh2;

        /**/
        public void Init(int sampleRate, long recordDepth, int channelCount)
        {
            var lng = recordDepth;// (int)(sampleRate * ;

            //RuntimeVariables.AdcConfig.SampleRate = sampleRate;

            //RuntimeVariables.Instance.LastSampleRate = sampleRate;

            Channels = new ISampleRepository<int>[channelCount];

            for (var i = 0; i < Channels.Length; i++)
            {
                Channels[i] = new RingArrayRepository<int>((int)lng);
            }

            MemoryDepth = recordDepth;// (long)(sampleRate * RuntimeVariables.RepoLengthSecsMemoryDepth);
        }
        /**/


    }
}
