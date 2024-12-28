using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Hardware
{
    public interface BaseDaqConfigGUIControl
    {
        BaseDeviceUserSettingsData GetUserSettings();//gets the config (set by user on UI) as byte array to pass into DAQ interface

        void SetDefaultUserSettings(BaseDeviceUserSettingsData config);//last used config by user

        bool IsValidConfig();//config by user incomplete, invalid port, etc

        void Init();
    }

}
