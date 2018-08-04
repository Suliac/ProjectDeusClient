using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public interface IIdentifiable
    {
        uint UniqueIdentifier { get; }
    }
}
