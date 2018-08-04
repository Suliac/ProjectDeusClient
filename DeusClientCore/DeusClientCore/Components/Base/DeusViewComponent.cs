using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Components
{
    public abstract class DeusViewComponent : DeusComponent
    {
        protected DeusComponent m_linkedComponent;

        public DeusViewComponent(DeusComponent linkedComponent)
        {
            m_linkedComponent = linkedComponent;
        }

        /// <summary>
        /// Called by <see cref="GameView"/> when it receive <see cref="Packets.PacketUpdateViewObject"/>
        /// </summary>
        /// <param name="value">The new value of the current value</param>
        public abstract void UpdateViewValue(Object value);
    }
}
