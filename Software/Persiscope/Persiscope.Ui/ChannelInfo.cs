using Avalonia.Media;
using AvlScope.Lib;

namespace AvlScope.Ui
{
    public class ChannelInfo
    {
        public string Id;

        public ChannelSource Source;

        public Color DisplayColor;

        public bool Enabled;
    }


    public enum ChannelSourceType
    {
        Hardware,
        Software,
        Hybrid
    }

    public abstract class ChannelSource
    {
        //public ChannelSourceType Type;


        /// <summary>
        /// gets the shot, fills the data
        /// </summary>
        /// <param name="values"></param>
        public abstract void FillChannelReadouts(DataRepositorySnapshot shot, AnalogSamplePackage data);

        public abstract bool CanFillChannelReadouts(DataRepositorySnapshot shot);

        public abstract void GetInfo(DataRepositorySnapshot shot, out long depth,out double sampleRate,out long startIndex);
    }


}
