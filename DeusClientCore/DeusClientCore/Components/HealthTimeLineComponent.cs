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
        /// Extrapolate the health the object has, at time given
        /// </summary>
        /// <param name="timeStamp">The timestamp en MS at chich we want datas</param>
        /// <returns>The interpolated value</returns>
        public override int ExtrapolateValue(double timeStamp)
        {
            int interpolateValue = 0;

            if (!m_dataWithTime.Any())
                throw new TimeLineException("Impossible to interpolate health without at least 1 entry");

            // Interpolate the health of an object is very simple : 
            // just take the last amount we got for any datas saved before the timestamp
            if (!m_dataWithTime.Any(d => d.TimeStampMs <= timeStamp))
                interpolateValue = m_dataWithTime.LastOrDefault(d => d.TimeStampMs <= timeStamp).Data;
            else
                throw new TimeLineException("Impossible to interpolate health without at least 1 entry before the timestamp");


            return interpolateValue;
        }

        protected override void OnStart()
        {
            // On start, init datas with 0 life at time 0
            InsertData(0, 0);
        }
    }
}
