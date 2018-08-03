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
        public double TimeStampMs;

        public DataTimed(T data, double timeStampMs)
        {
            Data = data;
            TimeStampMs = timeStampMs;
        }
    }
}
