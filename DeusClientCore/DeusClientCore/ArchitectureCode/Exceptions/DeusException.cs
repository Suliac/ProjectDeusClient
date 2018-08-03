using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Exceptions
{
    public class DeusException : Exception
    {
        public DeusException() : base()
        {
        }

        public DeusException(string message) : base(message)
        {
        }
    }
}
