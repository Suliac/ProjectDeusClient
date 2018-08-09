using DeusClientCore.Events;
using DeusClientCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientConsole
{
    class Test
    {
        public Test()
        {
            EventManager.Get().AddListener(EPacketType.GetGameAnswer, ManagePacket);
            EventManager.Get().AddListener(EPacketType.JoinGameAnswer, ManagePacket);
        }

        public void ManagePacket(object sender, SocketPacketEventArgs e)
        {
            Console.WriteLine(e.Packet.Type);
        }
    }
}
