using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Exceptions
{
    public class TimeLineException : DeusException
    {
        public TimeLineException() : base()
        {
        }

        public TimeLineException(string message) : base(message)
        {
        }
    }
}
