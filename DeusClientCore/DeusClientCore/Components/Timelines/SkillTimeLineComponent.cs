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
    public class SkillTimeLineComponent : TimeLineComponent<Skill>
    {
        SkillState m_lastValue;

        public SkillTimeLineComponent(uint identifier, uint objectIdentifier, DataTimed<Skill> origin, DataTimed<Skill> destination = null) : base(true, identifier, objectIdentifier, EComponentType.SkillComponent, origin, destination)
        {
        }

        protected override Skill Extrapolate(DataTimed<Skill> dataBeforeTimestamp, uint currentMs)
        {
            return dataBeforeTimestamp.Data;
        }

        protected override Skill Interpolate(DataTimed<Skill> dataBeforeTimestamp, DataTimed<Skill> dataAfterTimestamp, uint currentMs)
        {
            return dataBeforeTimestamp.Data;
        }

        protected override void OnUpdate(decimal deltatimeMs)
        {
            base.OnUpdate(deltatimeMs);
            uint currentTime = TimeHelper.GetUnixMsTimeStamp();

            // estimate current state of skill
            Skill currentValue = (Skill)GetViewValue(currentTime);
            if (currentValue == null)
                return;

            SkillState currentState = SkillState.NotLaunched;

            if(currentValue.LaunchTime > currentTime) // spell is at least casting
            {
                currentState = SkillState.Casting;
                if(currentValue.LaunchTime + currentValue.CastTime > currentTime) // spell is at least launched
                {
                    currentState = SkillState.Launched;

                    if (currentValue.LaunchTime + currentValue.CastTime + currentValue.Effects.Sum(effect => effect.Duration) > currentTime)
                        currentState = SkillState.Finished;
                }
            }


            if (m_lastValue != currentState)
            {
                Console.WriteLine($"Skill state just changed from {m_lastValue} to {currentValue}");
                SendViewPacket(currentValue);
                m_lastValue = currentState;
            }

            if(currentState == SkillState.Finished)
            {
                m_dataWithTime.RemoveAt(0); // remove current skill
            }
        }
    }
}
