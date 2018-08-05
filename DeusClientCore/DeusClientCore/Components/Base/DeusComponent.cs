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

        public bool Stopped { get; protected set; }
        
        private static uint m_nextId = 1;

        public DeusComponent()
        {
            m_uniqueIdentifier = m_nextId;
            m_nextId++;
        }

        public DeusComponent(uint identifier)
        {
            m_uniqueIdentifier = identifier;
            m_nextId = identifier + 1;
        }

        public void Update(decimal deltatimeMs)
        {
            if (!Stopped)
                OnUpdate(deltatimeMs);
        }

        public void Stop()
        {
            OnStop();
            Stopped = true;
        }

        public void Start()
        {
            Stopped = false;
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
