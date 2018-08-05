using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Components
{
    public abstract class DeusViewComponent : DeusComponent
    {
        protected IViewableComponent m_linkedComponent;

        protected bool m_realtimeUpdateView = false;

        public DeusViewComponent(IViewableComponent linkedComponent, uint identifier) : base(identifier)
        {
            m_linkedComponent = linkedComponent;
            m_realtimeUpdateView = m_linkedComponent.RealtimeViewUpdate;
        }

        protected sealed override void OnUpdate(decimal deltatimeMs)
        {
            if (m_linkedComponent.Stopped && !Stopped)
            {
                Stopped = true;
                return;
            }

            // If this view component need to be updated at each frame without flood the Event Queue, implement behavior here
            if (m_realtimeUpdateView)
                OnRealtimeViewUpdate(deltatimeMs);

            OnViewComponentUpdate(deltatimeMs);
        }

        protected virtual void OnViewComponentUpdate(decimal deltatimeMs)
        {

        }

        /// <summary>
        /// On update of <see cref="DeusViewComponent"/> we just get the current state of health from our health Timeline.
        /// This function is called if this <see cref="DeusViewComponent"/> is set to realtime update,
        /// as the component get itself the values it wants, it doesn't flood our event queue
        /// </summary>
        /// <param name="deltatimeMs">The time elapsed since the last loop</param>
        protected virtual void OnRealtimeViewUpdate(decimal deltatimeMs)
        {

        }

        /// <summary>
        /// Called by <see cref="GameView"/> when it receive <see cref="Packets.PacketUpdateViewObject"/>
        /// </summary>
        /// <param name="value">The new value of the current value</param>
        public abstract void UpdateViewValue(Object value);
    }
}
