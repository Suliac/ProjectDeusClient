using DeusClientCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Components
{
    //public class HealthViewComponent : DeusViewComponent
    //{
    //    private int m_currentHealth;

    //    public HealthViewComponent(DeusComponent linkedComponent) : base(linkedComponent)
    //    {
    //    }
        
    //    /************************************************************************************/
    //    /* Use this if you want to update the value only when receive update from the logic */
    //    /************************************************************************************/
    //    public override void UpdateViewValue(object value)
    //    {
    //        if(value is int)
    //            m_currentHealth = (int)value;
    //    }

    //    /**********************************************************/
    //    /* Use this if you want to update the value at each loop  */
    //    /**********************************************************/
    //    /*
    //    /// <summary>
    //    /// On update of <see cref="DeusViewComponent"/> we just get the current state of health from our health Timeline
    //    /// </summary>
    //    /// <param name="deltatimeMs">The time elapsed since the last loop</param>
    //    protected override void OnUpdate(decimal deltatimeMs)
    //    {
    //        if(m_linkedComponent is HealthTimeLineComponent)
    //        {
    //            TimeSpan currentTimeSpan = new TimeSpan(DateTime.UtcNow.Ticks);
    //            m_currentHealth = (m_linkedComponent as HealthTimeLineComponent).ExtrapolateValue(currentTimeSpan.TotalMilliseconds);
    //        }
    //    }
    //    */
    //}
}
