using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Common
{

    public class Linear1DTransformation : I1DTransformation
    {
        public double alpha, betta;

        public double Transform(double value)
        {
            return alpha * value + betta;
        }


        public double TransformBack(double value)
        {
            return (value - betta) / alpha;
        }

        public Linear1DTransformation Invert()
        {
            var buf = new Linear1DTransformation();
            buf.alpha = 1 / this.alpha;
            buf.betta = -this.betta / this.alpha;

            return buf;
        }

        public Linear1DTransformation()
        {

        }

        public Linear1DTransformation(double alpha, double betta)
        {
            this.alpha = alpha;
            this.betta = betta;
        }

        public static Linear1DTransformation FromInOut(
            double in1, double in2,
            double out1, double ou2)
        {
            //octave:
            //syms in1 in2 out1 ou2
            //inv([in1,1;in2,1])*[out1;ou2]


            var buf = new Linear1DTransformation();

            buf.alpha = -ou2 / (-in2 + in1) + out1 / (-in2 + in1);
            buf.betta = -in2 * out1 / (-in2 + in1) + ou2 * in1 / (-in2 + in1);

            return buf;
        }
    }
}
