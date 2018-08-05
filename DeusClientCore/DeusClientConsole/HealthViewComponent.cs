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

        public HealthViewComponent(IViewableComponent linkedComponent) : base(linkedComponent)
        {
        }

        /************************************************************************************/
        /* Use this if you want to update the value only when receive update from the logic */
        /************************************************************************************/
        public override void UpdateViewValue(object value)
        {
            if (value is int)
                m_currentHealth = (int)value;
            
            // specific behavior for console
            Console.WriteLine($"My Health is : {m_currentHealth}");
        }

        /**********************************************************/
        /* Use this if you want to update the value at each loop  */
        /**********************************************************/
        
        /// <summary>
        /// On update of <see cref="DeusViewComponent"/> we just get the current state of health from our health Timeline
        /// </summary>
        /// <param name="deltatimeMs">The time elapsed since the last loop</param>
        protected override void OnUpdate(decimal deltatimeMs)
        {
            if(m_linkedComponent is HealthTimeLineComponent)
                UpdateViewValue(m_linkedComponent.GetViewValue());
        }
        

    }
}
