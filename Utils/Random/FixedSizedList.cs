using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Random
{
    public class FixedSizedList<T> : List<T>
    {
        private readonly object syncObject = new object();

        public int Size { get; private set; }


        public FixedSizedList()
        {
            //14 should be in config file
            Size = 14;
        }

        public new void Add(T obj)
        {
            base.Add(obj);
            lock (syncObject)
            {
                while (base.Count > Size)
                {
                    this.Remove(this[Count]);
                }
            }
        }

    }
}
