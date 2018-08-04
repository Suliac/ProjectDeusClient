using DeusClientCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public abstract class DeusObject : IUpdatable
    {
        protected UInt32 m_uniqueIdentifier;
        protected List<DeusComponent> m_components;

        public uint UniqueIdentifier { get => m_uniqueIdentifier; private set => m_uniqueIdentifier = value; }

        public DeusObject(UInt32 identifier)
        {
            m_uniqueIdentifier = identifier;
            m_components = new List<DeusComponent>();
        }

        public void AddComponent(DeusComponent component)
        {
            m_components.Add(component);
        }

        public void Update(decimal deltatimeMs)
        {
            foreach (var component in m_components)
                component.Update(deltatimeMs);

            OnUpdate(deltatimeMs);
        }


        protected virtual void OnUpdate(decimal deltatimeMs)
        {
            // We let the children classes decide if they want to implement OnUpdate or not
        }
    }
}
