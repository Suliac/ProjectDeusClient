using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Components
{
    public abstract class DeusComponent : IExecutable
    {
        private List<DeusComponent> m_childrenComponents = new List<DeusComponent>();

        public void AddComponent(DeusComponent component)
        {
            m_childrenComponents.Add(component);
        }

        public void Update(decimal deltatimeMs)
        {
            foreach (var component in m_childrenComponents)
                component.OnUpdate(deltatimeMs);
        }

        public void Stop()
        {
            foreach(var component in m_childrenComponents)
                component.OnStop();
        }

        public void Start()
        {
            foreach (var component in m_childrenComponents)
                component.OnStart();
        }

        protected virtual void OnUpdate(decimal deltatimeMs)
        {

        }

        protected virtual void OnStart()
        {

        }

        protected virtual void OnStop()
        {

        }
    }
}
