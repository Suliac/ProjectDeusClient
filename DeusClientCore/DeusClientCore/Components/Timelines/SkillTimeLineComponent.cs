using DeusClientCore.Events;
using DeusClientCore.Packets;
using DeusClientCore.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Components
{
    public class SkillTimeLineComponent : TimeLineComponent<SkillInfos>
    {
        SkillState m_lastValue;

        public SkillTimeLineComponent(uint identifier, uint objectIdentifier, DataTimed<SkillInfos> origin, DataTimed<SkillInfos> destination = null) : base(true, identifier, objectIdentifier, EComponentType.SkillComponent, origin, destination)
        {
        }

        protected override SkillInfos Extrapolate(DataTimed<SkillInfos> dataBeforeTimestamp, uint currentMs)
        {
            return dataBeforeTimestamp.Data;
        }

        protected override SkillInfos Interpolate(DataTimed<SkillInfos> dataBeforeTimestamp, DataTimed<SkillInfos> dataAfterTimestamp, uint currentMs)
        {
            return dataBeforeTimestamp.Data;
        }

        protected override void OnUpdate(decimal deltatimeMs)
        {
            base.OnUpdate(deltatimeMs);
            uint currentTime = TimeHelper.GetUnixMsTimeStamp();

            // estimate current state of SkillInfos
            SkillInfos currentValue = (SkillInfos)GetViewValue(currentTime);
            if (currentValue == null)
                return;

            currentValue.State = SkillState.NotLaunched;

            if(currentValue.LaunchTime > currentTime) // spell is at least casting
            {
                currentValue.State = SkillState.Casting;
                if(currentValue.LaunchTime + currentValue.CastTime > currentTime) // spell is at least launched
                {
                    currentValue.State = SkillState.Launched;

                    if (currentValue.LaunchTime + currentValue.CastTime + currentValue.Effects.Sum(effect => effect.Duration) > currentTime)
                        currentValue.State = SkillState.Finished;
                }
            }


            if (m_lastValue != currentValue.State)
            {
                Console.WriteLine($"SkillInfos state just changed from {m_lastValue} to {currentValue}");
                SendViewPacket(currentValue);
                m_lastValue = currentValue.State;
            }

            if(currentValue.State == SkillState.Finished)
            {
                m_dataWithTime.RemoveAt(0); // remove current SkillInfos
            }
        }
    }
}
