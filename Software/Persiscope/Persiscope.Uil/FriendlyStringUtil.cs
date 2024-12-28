using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Uil
{
    public static class FriendlyStringUtil
    {
        //https://stackoverflow.com/a/12181661
        public static string ToSI(double d, string format = null)
        {

            if (double.IsNaN(d))
                return "-";

            char[] incPrefixes = new[] { 'k', 'M', 'G', 'T', 'P', 'E', 'Z', 'Y' };
            char[] decPrefixes = new[] { 'm', '\u03bc', 'n', 'p', 'f', 'a', 'z', 'y' };

            int degree = (int)Math.Floor(Math.Log10(Math.Abs(d)) / 3);
            double scaled = d * Math.Pow(1000, -degree);

            char? prefix = null;


            if (d == 0)
                return "0";

            switch (Math.Sign(degree))
            {
                case 1: prefix = incPrefixes[degree - 1]; break;
                case -1: prefix = decPrefixes[-degree - 1]; break;
            }

            var bs = scaled.ToString(format);

            bs = bs.TrimEnd('0');
            bs = bs.TrimEnd('.');

            return bs + " " + prefix;
        }

    }
}
