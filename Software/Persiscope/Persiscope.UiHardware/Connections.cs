using Persiscope.UiHardware.Rp2040Raw;
using Persiscope.Uil.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.UiHardware
{


    public static class Connections
    {

        static bool Inited = false;

        //init if didn't!
        public static void Initfdint()
        {
            if(Inited) return;

            {//do all registers here
                ConnectManager.Register(new Rp2040Raw.Manager());
            }

            Inited = true;
        }



        public static BaseConnectManager[] GetManagers()
        {
            return ConnectManager.Managers.ToArray();
        }
    }
}
