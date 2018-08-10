using DeusClientCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Components
{
    public class PositionTimeLineComponent : TimeLineComponent<DeusObjectMovement>
    {
        public PositionTimeLineComponent() : base(true)
        {
        }

        /// <summary>
        /// Interpolate the <see cref="DeusVector2"/> of the object
        /// The next <see cref="DeusVector2"/> of the object is :
        /// newPos = start_movement_pos + dir * elapsed_time_since_start_move
        /// Where :
        /// newPos                        is <see cref="DeusVector2"/>, 
        /// start_movement_pos            is the positition in <see cref="DeusObjectMovement"/>, 
        /// dir                           is the dir vector in <see cref="DeusObjectMovement"/>, 
        /// elapsed_time_since_start_move is <see cref="long"/>
        /// </summary>
        /// <returns>The <see cref="DeusVector2"/> interpolate</returns>
        public override System.Object GetViewValue(long timeStampMs = -1)
        {
            DeusVector2 interpolateValue = new DeusVector2();
            DataTimed<DeusObjectMovement> matchingMovementTimed = null;
            long currentTimeStamp = timeStampMs < 0 ? TimeHelper.GetUnixMsTimeStamp() : timeStampMs;

            /////////////////////////////////////////////////////////
            // 1 - Get the last DeusObjectMovement timed from the timeline
            if (!m_dataWithTime.Any())
                throw new TimeLineException("Impossible to interpolate position without at least 1 entry");

            if (m_dataWithTime.Any(d => d.TimeStampMs <= currentTimeStamp))
                matchingMovementTimed = m_dataWithTime.LastOrDefault(d => d.TimeStampMs <= currentTimeStamp);
            else // if we can't get 'old' datas, juste take the first one we can
                matchingMovementTimed = m_dataWithTime.FirstOrDefault(d => d.TimeStampMs >= currentTimeStamp);

            /////////////////////////////////////////////////////////
            // 2 - Interpolate the next position
            long dtElapsed = currentTimeStamp - matchingMovementTimed.TimeStampMs;

            // newPos = start_movement_pos + dir * elapsed_time_since_start_move
            interpolateValue.X = matchingMovementTimed.Data.Position.X + matchingMovementTimed.Data.Dir.X * (dtElapsed / 1000.0f);
            interpolateValue.Y = matchingMovementTimed.Data.Position.Y + matchingMovementTimed.Data.Dir.Y * (dtElapsed / 1000.0f);

            return interpolateValue;
        }

        protected override void OnStart()
        {
            InsertData(new DeusObjectMovement(DeusVector2.Zero, DeusVector2.Zero));
        }
    }
}
