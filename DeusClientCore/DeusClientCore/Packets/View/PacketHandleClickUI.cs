using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketHandleClickUI : Packet
    {
        public enum UIButton
        {
            JoinGameButton,
            CreateGameButton,
            LeaveGameButton,
            GetGameButton,
            SendTextButton,
        }

        public UIButton UIClicked { get; set; }

        public uint GameIdToJoin { get; set; }

        public string TextMessage { get; set; }

        public PacketHandleClickUI() : base(EPacketType.HandleClickUI)
        {
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            throw new Exception("Don't mean to be serialized or deserialized !");
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            throw new Exception("Don't mean to be serialized or deserialized !");

        }

        public override byte[] OnSerialize()
        {
            throw new Exception("Don't mean to be serialized or deserialized !");

        }
    }
}
