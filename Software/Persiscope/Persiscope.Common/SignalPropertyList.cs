using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Lib
{
    public class SignalPropertyList : IDisposable
    {
        public double alpha;
        public double beta;

        public double Max { get; set; }
        public double Min { get; set; }
        public double Avg { get; set; }
        public int Domain { get; set; }

        public int MinPercentile1 { get; set; }
        public int MaxPercentile1 { get; set; }

        public int Percentile1Domain { get; set; }
        public int MinPercentile5 { get; set; }
        public int MaxPercentile5 { get; set; }
        public int Percentile5Domain { get; set; }

        public double Frequency { get; set; }
        public double PhaseRadian { get; set; }

        public double PwmDutyCycle { get; set; }

        public bool Error { get; set; }


        //public FftContext FftContext;

        public void Dispose()
        {
            //if (FftContext != null)
            //    FftContext.Dispose();
        }
    }
}
