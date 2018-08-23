using DeusClientCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientConsole
{

    public class HealthViewComponent : DeusViewComponent
    {
        private int m_currentHealth;

        public HealthViewComponent(IViewableComponent linkedComponent, uint identifier) : base(linkedComponent, identifier, EComponentType.HealthComponent)
        {
        }

        public override void UpdateViewValue(object value)
        {
            if (value is int)
                m_currentHealth = (int)value;
            
            // specific behavior for console
            Console.WriteLine($"Id : {m_uniqueIdentifier} | Health : {m_currentHealth}");
        }

        /// <summary>
        /// On update of <see cref="DeusViewComponent"/> we just get the current state of health from our health Timeline.
        /// This function is called if this <see cref="DeusViewComponent"/> is set to realtime update,
        /// as the component get itself the values it wants, it doesn't flood our event queue
        /// </summary>
        /// <param name="deltatimeMs">The time elapsed since the last loop</param>
        protected override void OnRealtimeViewUpdate(decimal deltatimeMs)
        {
            if (m_linkedComponent is HealthTimeLineComponent)
                UpdateViewValue(m_linkedComponent.GetViewValue());
        }
    }
}
