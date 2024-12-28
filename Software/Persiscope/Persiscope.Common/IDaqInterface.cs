namespace Persiscope.Common
{
    public interface IDaqInterface
    {
        //calibration info per channel
        //CalibrationInfo[] GetCalibrationInfos();

        //void Init();

        void StartSync();

        DataRepository TargetRepository { get; set; }

        int AdcResolutionBits { get; }//resulution in bits (8 or 10 or 12)

        double AdcMaxVoltage { get; }// for example for a 8 bit Resolution, (ResolutionBits = 8), max value is 256. 256 equal to AdcMaxVoltage.

        long AdcSampleRate { get; }


        void StopAdc();

        void DisConnect();

        int GetMaxChannelCount();


        //happens when something went wrong in the connection protocol,like packet loss etc.
        //other exceptions like usb unplug no need to handle
        //event EventHandler Crashed;

        //once this flag is set, should wait untill read thread is finished
        bool StopFlag { set; get; }
    }
}
