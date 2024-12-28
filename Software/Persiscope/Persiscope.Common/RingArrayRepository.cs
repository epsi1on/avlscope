using System;

namespace Persiscope.Lib
{
    public class RingArrayRepository<T> : ISampleRepository<T>
    {
        //will use a single array as circular

        public readonly int FixedLength;

        long totalWrites;

        public long TotalWrites
        {
            get
            {
                return totalWrites;
            }
            private set
            {
                totalWrites = value;
            }
        }

        //public double Alpha { get; set; }

        //public double Beta { get; set; }



        private object lc = new object();

        //public object GetLocker
        


        private T[] arr;


        public RingArrayRepository(int l)
        {
            arr = new T[l];
            FixedLength = l;
        }


        public int Index = 0;


        public void Add(T item)
        {
            lock (lc)
            {
                //https://stackoverflow.com/q/33781853


                if (Index >= FixedLength)
                    Index = 0;

                arr[Index] = item;

                Index++; // increment index

                totalWrites++;
            }
        }

        public void Add(T[] items)
        {
            lock (lc)
            {
                //https://stackoverflow.com/q/33781853

                var l = items.Length;

                for (int i = 0; i < l; i++)
                {
                    if (Index >= FixedLength)
                        Index = 0;

                    arr[Index] = items[i];

                    Index++; // increment index

                    totalWrites++;
                }

            }
        }

        public void CopyTo(T[] other,bool resort,out int idxx)
        {
            if (other.Length != FixedLength)
                throw new Exception();

            Array.Clear(other, 0, other.Length);

            var tmp = new T[1000];

            int sz;

            unsafe
            {
                sz = sizeof(T);
            }

            lock (lc)
            {
                var L = FixedLength;
                var idx = Index;
                idxx = idx;

                var t = Index % L;

                var thisArr = arr;

                if(resort)
                {
                    Buffer.BlockCopy(thisArr, sz * t, other, 0, sz * (L - t));

                    Buffer.BlockCopy(thisArr, 0, other, sz * (L - t), sz * t);
                }
                else
                {
                    Buffer.BlockCopy(thisArr, 0, other, 0, sz * (L ));
                }

            }
        }

        public void CopyTo(T[] other, out int idxx)
        {
            if (other.Length != FixedLength)
                throw new Exception();

            Array.Clear(other, 0, other.Length);

            var tmp = new T[1000];

            int sz;

            unsafe
            {
                sz = sizeof(T);
            }

            lock (lc)
            {
                var L = FixedLength;
                var idx = Index;
                idxx = idx;

                var t = Index % L;

                var thisArr = arr;

                {
                    Buffer.BlockCopy(thisArr, sz * t, other, 0, sz * (L - t));

                    Buffer.BlockCopy(thisArr, 0, other, sz * (L - t), sz * t);
                }
            }
        }

        public void CopyUnordered(T[] other, out long writes)
        {
            if (other.Length != FixedLength)
                throw new Exception();

            int sz;

            unsafe
            {
                sz = sizeof(T);
            }

            lock (lc)
            {
                var L = FixedLength;
                var idx = Index;

                var thisArr = arr;
                Buffer.BlockCopy(thisArr, 0, other, 0, sz * thisArr.Length);
                writes = totalWrites;
            }

        }

    }
}
