using AvlScope.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvlScope.Hardware.Pico.Picov1
{
    public class Rp2DaqMchInterfaceUi : BaseDeviceInterface<Rp2daqMchCalibrationData, Rp2daqMchUserSettings>
    {
        public override IDaqInterface GenerateDaqInterface(BaseDeviceCalibrationData calibrationData, BaseDeviceUserSettingsData userSettings)
        {
            throw new NotImplementedException();
            //var buf = new Rp2DaqMchInterface(userSettings, calibrationData);
            //return buf;
        }

        /**/
        public override BaseDaqConfigGUIControl GenerateUiInterface(BaseDeviceUserSettingsData config)
        {

            throw new NotImplementedException ();

            //var buf = new Rp2DaqMchInterfaceControl();

            //buf.Init();
            //buf.SetDefaultUserSettings(config);

            //return buf;
        }
        /**/

        protected override BaseDeviceCalibrationData GetDefaulCalibration()
        {
            var buf = new Rp2daqMchCalibrationData();

            buf.AlphaA1 = buf.AlphaA2 = 1.0 / 4096;
            buf.AlphaB1 = buf.AlphaB2 = 1.0 / 4096;
            buf.AlphaC1 = buf.AlphaC2 = 1.0 / 4096;

            buf.BetaA1 = buf.BetaB1 = buf.BetaC1 = 0;
            buf.BetaA2 = buf.BetaB2 = buf.BetaC2 = 0;

            return buf;
        }

        protected override BaseDeviceUserSettingsData GetDefaultUserSettings()
        {
            var set = new Rp2daqMchUserSettings();

            set.SampleRate = 500_000;
            set.ChannelIds = new[] { Rp2040AdcChannels.Gpio27 };
            set.BitWidth = 12;

            return set;
        }

        public override string GetDescription()
        {
            return "rp2daq on RPi Pico";
        }

        public override string GetName()
        {
            return "rp2daq MCH";
        }

        public override bool TryCalibrate(out BaseDeviceCalibrationData config)
        {
            throw new NotImplementedException();
        }

        public override string GetUid()
        {
            return "rp2daq_4681";//4681 just a random
        }

        protected override Type GetUserSettingsType()
        {
            throw new NotImplementedException();
        }
    }
}
