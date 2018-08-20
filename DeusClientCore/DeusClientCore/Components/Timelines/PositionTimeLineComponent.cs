using DeusClientCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Components
{
    public class PositionTimeLineComponent : TimeLineComponent<DeusVector2>
    {
        public PositionTimeLineComponent() : base(true)
        {
        }

        /// <summary>
        /// When we extrapolate a data, we just give the last one : the player isn't moving
        /// </summary>
        /// <param name="dataBeforeTimestamp">The last data known</param>
        /// <param name="currentMs">The current time</param>
        /// <returns>The <see cref="DeusVector2"/> we extrapolate</returns>
        protected override DeusVector2 Extrapolate(DataTimed<DeusVector2> dataBeforeTimestamp, uint currentMs)
        {
            return dataBeforeTimestamp.Data;
        }

        /// <summary>
        /// Interpolate the <see cref="DeusVector2"/> of the object
        /// The next <see cref="DeusVector2"/> of the object is :
        /// newPos = start + (end-start) x (time_elsapsed_since_start / duration_bewteen_start_and_end)
        /// </summary>
        /// <param name="dataBeforeTimestamp">The origin</param>
        /// <param name="dataAfterTimestamp">The destination</param>
        /// <param name="currentMs">The current time</param>
        /// <returns>The <see cref="DeusVector2"/> we interpolate</returns>
        protected override DeusVector2 Interpolate(DataTimed<DeusVector2> dataBeforeTimestamp, DataTimed<DeusVector2> dataAfterTimestamp, uint currentMs)
        {
            return dataBeforeTimestamp.Data + (dataAfterTimestamp.Data - dataBeforeTimestamp.Data) * (float)((float)(currentMs - dataBeforeTimestamp.TimeStampMs) / (float)(dataAfterTimestamp.TimeStampMs - dataBeforeTimestamp.TimeStampMs));
        }

        protected override void OnStart()
        {
            InsertData(DeusVector2.Zero, 0);
        }
    }
}
