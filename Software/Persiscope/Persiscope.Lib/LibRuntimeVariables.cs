using Persiscope.Common;
using Persiscope.Hardware;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Lib
{
    public class LibRuntimeVariables
    {

        

        //public const int MaxChannelCount = 4;

        //public static bool DisableSignalPropertyCalculation = false;

        //public static readonly PixelFormat BitmapPixelFormat = PixelFormats.Bgra32;

        public DataRepository CurrentRepo;
        public IDaqInterface CurrentDaq;

        //public int RenderBitmapWidth = 500;
        //public int RenderBitmapHeight = 500;

        //public readonly AdcChannelInfo[] Channels;

        public static readonly double RepoLengthSecsMemoryDepth = 1;

        public static LibRuntimeVariables Instance = new LibRuntimeVariables();

        public uint ImageBackgroundColor = uint.MinValue;


        //public double LastAlphaCh1, LastBetaCh1;
        //public double LastAlphaCh2, LastBetaCh2;

        //public double LastSampleRate;

        private LibRuntimeVariables()
        {
            CurrentRepo = new DataRepository();
            //CurrentRepo.Channels.Add(new ChannelData(DataRepository.RepoLength));

            //Channels = InitChannels();
        }
    }
}
