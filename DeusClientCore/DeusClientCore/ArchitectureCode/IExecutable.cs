using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public interface IExecutable : IUpdatable
    {
        bool Stopped { get; }

        void Stop();
        void Start();

    }
}
