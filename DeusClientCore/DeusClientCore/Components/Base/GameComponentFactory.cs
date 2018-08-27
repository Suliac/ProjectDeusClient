using DeusClientCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Components
{
    public class GameComponentFactory
    {

        public static ISerializableComponent DeserializeComponent(byte[] buffer, ref int index)
        {
            ISerializableComponent component = null;

            byte tmpType = buffer[index + sizeof(uint)]; // we know that the type is after the id

            switch ((EComponentType)tmpType)
            {
                case EComponentType.HealthComponent:
                    component = new DeusSerializableTimelineComponent<int>();
                    break;
                case EComponentType.PositionComponent:
                    component = new DeusSerializableTimelineComponent<DeusVector2>();
                    break;
                default:
                    throw new Exception("Impossible to instantiate the serializable component");
            }

            component.Deserialize(buffer, ref index);

            return component;
        }

        //private static DeusSerializableComponent CreatePositionSerializableComponent()
        //{
        //    return null;
        //}

        public static DeusComponent CreateComponent(ISerializableComponent args)
        {
            switch (args.ComponentType)
            {
                case EComponentType.HealthComponent:
                    return new HealthTimeLineComponent(args.ComponentId, (args as DeusSerializableTimelineComponent<int>).Origin, (args as DeusSerializableTimelineComponent<int>).Destination);
                case EComponentType.PositionComponent:
                    return new PositionTimeLineComponent(args.ComponentId, (args as DeusSerializableTimelineComponent<DeusVector2>).Origin, (args as DeusSerializableTimelineComponent<DeusVector2>).Destination);
                default:
                    return null;
            }
        }
    }
}
