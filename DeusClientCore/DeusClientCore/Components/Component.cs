using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Components
{
    public abstract class Component : IExecutable
    {
        private List<Component> m_childrenComponents = new List<Component>();

        public void AddComponent(Component component)
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
