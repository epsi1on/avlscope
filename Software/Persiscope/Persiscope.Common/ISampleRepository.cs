using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Lib
{
    public interface ISampleRepository<T>
    {
        void Add(T item);

        void Add(T[] items);

        [Obsolete]
        void CopyTo(T[] other, bool resort, out int index);

        void CopyTo(T[] other, out int index);

        long TotalWrites { get; }

        //double Alpha { get; set; }//converting ADC value to volt

        //double Beta { get; set; }//converting ADC value to volt

    }
}
