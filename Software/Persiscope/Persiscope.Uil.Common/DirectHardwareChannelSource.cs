using Persiscope.Lib;
using System;
using System.Linq;

namespace Persiscope.Ui
{
    public class DirectHardwareChannelSource : ChannelSource
    {
        public int TargetChannelIndex;//index of channel on the hardware

        public override void FillChannelReadouts(DataRepositorySnapshot shot, AnalogSamplePackage data)
        {
            var ints = shot.ChannelsSnapshot[TargetChannelIndex];

            if (ints.Length != data.Values.Length)
                throw new Exception();

            var calib = shot.CalibrationDatas[TargetChannelIndex];

            HpVectorOperation.AxB(ints, calib.Alpha, calib.Beta, data.Values);

            data.StartIndex = shot.ChannelsIndexes[TargetChannelIndex];
        }

        public override bool CanFillChannelReadouts(DataRepositorySnapshot shot)
        {
            var ints = shot.ChannelsSnapshot[TargetChannelIndex];

            if (shot.CalibrationDatas == null) return false;

            if (shot.CalibrationDatas.Length <= TargetChannelIndex) return false;

            return shot.CalibrationDatas[TargetChannelIndex] != null;
        }

        public override void GetInfo(DataRepositorySnapshot shot, out long depth,out double sampleRate, out long startIndex,out bool en)
        {
            depth = shot.ChannelsSnapshot[TargetChannelIndex].Length;
            sampleRate = shot.SampleRate;
            startIndex= shot.ChannelsIndexes[TargetChannelIndex];
            en= shot.ChannelsEnabled[TargetChannelIndex];
        }
    }




}
