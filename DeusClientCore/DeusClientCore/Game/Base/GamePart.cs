using DeusClientCore.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public abstract class GamePart<T> : ExecutableObjectsHolder<T>, IGamePart where T : IDeusObject
    {
        protected abstract void ManagePacket(object sender, SocketPacketEventArgs e);
    }
}
