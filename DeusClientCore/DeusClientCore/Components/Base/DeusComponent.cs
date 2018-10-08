using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Components
{
    public enum EComponentType : byte
    {
        Error = 0,
        HealthComponent = 1,
        PositionComponent = 2,
        SkillComponent = 3,
    }

    public abstract class DeusComponent : IExecutable, IIdentifiable
    {
        protected uint m_uniqueIdentifier;
        public uint UniqueIdentifier { get => m_uniqueIdentifier; protected set => m_uniqueIdentifier = value; }

        protected uint m_objectIdentifier;
        public uint ObjectIdentifier { get => m_objectIdentifier; protected set => m_objectIdentifier = value; }

        protected EComponentType m_componentType;
        public EComponentType ComponentType { get => m_componentType; protected set => m_componentType = value; }

        public bool Stopped { get; protected set; }
        
        public DeusComponent(uint identifier, uint objectIdentifier, EComponentType type)
        {
            m_uniqueIdentifier = identifier;
            m_objectIdentifier = objectIdentifier;
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
