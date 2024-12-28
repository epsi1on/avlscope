namespace Persiscope.Common
{
    public class CalibrationInfo
    {

        public double Alpha, Beta;

        public CalibrationInfo()
        {
        }

        public CalibrationInfo(double alpha, double beta)
        {
            Alpha = alpha;
            Beta = beta;
        }

        public double Transform(double x)
        {
            return Alpha * x + Beta;
        }

    }
}
