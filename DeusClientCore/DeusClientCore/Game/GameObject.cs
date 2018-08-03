using DeusClientCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public class GameObject : IUpdatable, IDisposable
    {
        private UInt32 m_uniqueIdentifier;
        private List<Component> m_components;

        public GameObject(UInt32 identifier)
        {
            m_uniqueIdentifier = identifier;
            m_components = new List<Component>();
        }

        public void AddComponent(Component component)
        {
            m_components.Add(component);
        }

        public void Dispose()
        {
            m_components.Clear();
        }

        public void Update(decimal deltatimeMs)
        {
            foreach (var component in m_components)
                component.Update(deltatimeMs);
        }
    }
}
