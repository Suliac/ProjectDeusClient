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
        public HealthTimeLineComponent() : base(false) // This component doesn't have its view updated in at each update loop
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

        /// <summary>
        /// On <see cref="HealthTimeLineComponent"/> start, we just insert by default 0 life
        /// </summary>
        protected override void OnStart()
        {
            // On start, init datas with 0 life at time 0
            InsertData(0, 0);
        }
    }
}
