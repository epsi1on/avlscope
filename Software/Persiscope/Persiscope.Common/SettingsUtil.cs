using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Common
{
    public static class SettingsUtil
    {

        static string GetAndEnsureSaveDir()
        {
            var file = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            var buf = Path.GetDirectoryName(file);


            try
            {
                if (!Directory.Exists(buf))
                    Directory.CreateDirectory(buf);
            }
            catch (Exception ex)
            {
                var msg = "failed to create settings folder, try create this folder manually and try again:\r\n" + buf;

                var ex2 = new Exception(msg, ex);
                throw ex2;
            }

            return buf;

        }

        public static void Save(string key, byte[] data)
        {
            var dir = GetAndEnsureSaveDir();

            var fileNmae = Path.Combine(dir, key);

            File.WriteAllBytes(fileNmae, data);
        }

        public static byte[] Load(string key)
        {
            var dir = GetAndEnsureSaveDir();

            var fileNmae = Path.Combine(dir, key);

            if (!File.Exists(fileNmae))
                return null;//return null if key not exists

            return File.ReadAllBytes(fileNmae);
        }

    }
}
