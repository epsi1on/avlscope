using Persiscope.Uil.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.UiHardware
{
    public static class ConnectManager
    {
        
        internal static readonly List<BaseConnectManager> Managers = new List<BaseConnectManager>();


        public static void Register(BaseConnectManager mgr)
        {
            Managers.Add(mgr);
        }


        

    }
}
