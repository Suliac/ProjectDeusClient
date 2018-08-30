using DeusClientCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public interface IDeusObject : IIdentifiable, IExecutable
    {
        EObjectType ObjectType { get; }

        bool IsLocalPlayer { get; }

        uint PlayerLinkedId { get; }
    }
}
