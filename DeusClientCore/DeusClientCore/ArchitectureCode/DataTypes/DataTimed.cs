using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public class DataTimed<T>
    {
        public T Data;
        public uint TimeStampMs;

        public DataTimed(T data, uint timeStampMs)
        {
            Data = data;
            TimeStampMs = timeStampMs;
        }

        public override string ToString()
        {
            return $"[{TimeStampMs}]{Data}";
        }
    }
}
