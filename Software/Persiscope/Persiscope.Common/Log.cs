using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Lib
{
    public static class Log
    {
        public static void DebugInfo(string format, params object[] args)
        {
            var msg = string.Format(format, args);

            Debug.Write("Debug: ");
            Debug.WriteLine(msg);
        }


        public static void Info(string format, params object[] args)
        {
            var msg = string.Format(format, args);

            Debug.Write("info: ");
            Debug.WriteLine(msg);
        }

        public static void Warning(string format, params object[] args)
        {
            var msg = string.Format(format, args);

            Debug.Write("Warn: ");
            Debug.WriteLine(msg);
        }

        public static void Error(string format, params object[] args)
        {
            var msg = string.Format(format, args);

            Debug.Write("Err: ");
            Debug.WriteLine(msg);
        }
    }
}
