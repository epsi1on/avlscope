using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persiscope.Hardware
{
    public class SnifferSerial : SerialPort
    {

        public bool LogToConsole = false;

        public SnifferSerial(string portName, int baudRate) : base(portName, baudRate)
        {
        }

        public byte[] ReadAvailable()
        {
            return ReadExplicitLength(this.BytesToRead);
        }

        public byte[] ReadExplicitLength(int length)
        {
            var buf = new byte[length];


            var counter = 0;

            var l = length;

            while (counter < l)
            {
                var remain = l - counter;

                var rdr = this.Read(buf, counter, remain);
                counter += rdr;
            }


            if (LogToConsole)
            {
                var sb = new StringBuilder();

                for (var i = 0; i < length; i++)
                    sb.AppendFormat(" {0:x2}", buf[i]);

                Console.WriteLine("Reading {0} bytes: {1}", length, sb.ToString());
            }

            Array.Resize(ref buf, length);

            return buf;
        }

        public void ReadExplicitLength(int length, byte[] data)
        {
            var buf = data;

            var counter = 0;

            var l = length;

            while (counter < l)
            {
                var remain = l - counter;

                var rdr = this.Read(buf, counter, remain);
                counter += rdr;
            }

            /*
            if (LogToConsole)
            {
                var sb = new StringBuilder();

                for (var i = 0; i < length; i++)
                    sb.AppendFormat(" {0:x2}", buf[i]);

                Console.WriteLine("Reading {0} bytes: {1}", length, sb.ToString());
            }*/

            // Array.Resize(ref buf, length);

            //            return buf;
        }


        public void Write(params byte[][] data)
        {
            var sb = new StringBuilder();

            var l = data.Sum(i => i.Length);
            var buf = new byte[l];

            var cnt = 0;

            foreach (var item in data)
            {
                item.CopyTo(buf, cnt);
                cnt += item.Length;
            }

            this.Write(buf, 0, buf.Length);

            if (LogToConsole)
            {
                foreach (var b in buf)
                    sb.AppendFormat(" {0:x2}", b);

                Console.WriteLine("Writing {0} bytes: {1}", l, sb.ToString());
            }
        }

        internal void ReadArray(byte[] array)
        {
            var buf = array;

            var counter = 0;

            var l = array.Length;

            while (counter < l)
            {
                var remain = l - counter;

                var rdr = this.Read(buf, counter, remain);
                counter += rdr;
            }
        }
    }
}
