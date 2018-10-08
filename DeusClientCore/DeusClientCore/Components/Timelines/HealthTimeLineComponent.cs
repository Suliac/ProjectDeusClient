using DeusClientCore.Events;
using DeusClientCore.Exceptions;
using DeusClientCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Components
{
    public class HealthTimeLineComponent : TimeLineComponent<int>
    {
        private int m_lastValue = 0;

        /// <summary>
        /// Create <see cref="HealthTimeLineComponent"/>
        /// We specify with the 'base(false)' that we don't want our ViewComponent to bypass the event queue, 
        /// and the view component will only be updated by handling <see cref="Packets.PacketUpdateViewObject"/> packets
        /// </summary>
        public HealthTimeLineComponent(uint identifier, uint objectIdentifier, DataTimed<int> origin, DataTimed<int> destination) : base(false, identifier, objectIdentifier, EComponentType.HealthComponent, origin, destination) // This component doesn't have its view updated in at each update loop
        {
        }
        
        protected override int Extrapolate(DataTimed<int> dataBeforeTimestamp, uint currentMs)
        {
            return dataBeforeTimestamp.Data;
        }

        protected override int Interpolate(DataTimed<int> dataBeforeTimestamp, DataTimed<int> dataAfterTimestamp, uint currentMs)
        {
            return dataBeforeTimestamp.Data;
        }

        protected override void OnUpdate(decimal deltatimeMs)
        {
            base.OnUpdate(deltatimeMs);

            int currentValue = (int)GetViewValue();
            if(m_lastValue != currentValue)
            {
                Console.WriteLine($"Health just changed from {m_lastValue} to {currentValue}");
                // Notify the view, that component value has just changed : use this only if your component isn't getting in realtime informations
                PacketUpdateViewObject feedBackPacket = new PacketUpdateViewObject();
                feedBackPacket.ObjectId = ObjectIdentifier;
                feedBackPacket.ComponentId = UniqueIdentifier;
                feedBackPacket.NewValue = currentValue;
                EventManager.Get().EnqueuePacket(0, feedBackPacket);
                
                m_lastValue = currentValue;
            }
        }

    }
}
