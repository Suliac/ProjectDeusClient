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
        public HealthTimeLineComponent() : base(true) // This component doesn't have its view updated in at each update loop
        {
        }

        /// <summary>
        /// Extrapolate the health the object has, at current time
        /// </summary>
        /// <returns>An <see cref="int"/> -> the interpolated health value</returns>
        public override object GetViewValue()
        {
            int interpolateValue = 0;
            long timeStamp = TimeHelper.GetUnixMsTimeStamp();


            if (!m_dataWithTime.Any())
                throw new TimeLineException("Impossible to interpolate health without at least 1 entry");

            // Interpolate the health of an object is very simple : 
            // just take the last amount we got for any datas saved before the timestamp
            if (m_dataWithTime.Any(d => d.TimeStampMs <= timeStamp))
                interpolateValue = m_dataWithTime.LastOrDefault(d => d.TimeStampMs <= timeStamp).Data;
            else
                throw new TimeLineException("Impossible to interpolate health without at least 1 entry before the timestamp");


            return interpolateValue;
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
