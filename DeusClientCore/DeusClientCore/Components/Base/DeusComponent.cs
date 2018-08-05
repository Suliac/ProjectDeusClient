using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Components
{
    public abstract class DeusComponent : IExecutable, IIdentifiable
    {
        protected uint m_uniqueIdentifier;
        public uint UniqueIdentifier { get => m_uniqueIdentifier; protected set => m_uniqueIdentifier = value; }

        private static uint m_nextId = 1;

        public DeusComponent()
        {
            m_uniqueIdentifier = m_nextId;
            m_nextId++;
        }

        public void Update(decimal deltatimeMs)
        {
            OnUpdate(deltatimeMs);
        }

        public void Stop()
        {
            OnStop();
        }

        public void Start()
        {
            OnStart();
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
