using DeusClientCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Components
{
    public class HealthTimeLineComponent : TimeLineComponent<int>
    {
        /// <summary>
        /// Create <see cref="HealthTimeLineComponent"/>
        /// We specify with the 'base(false)' that we don't want our ViewComponent to bypass the event queue, 
        /// and the view component will only be updated by handling <see cref="Packets.PacketUpdateViewObject"/> packets
        /// </summary>
        public HealthTimeLineComponent(uint identifier, DataTimed<int> origin, DataTimed<int> destination) : base(false, identifier, EComponentType.HealthComponent, origin, destination) // This component doesn't have its view updated in at each update loop
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

        
    }
}
