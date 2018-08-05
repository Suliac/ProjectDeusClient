﻿using DeusClientCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    /// <summary>
    /// A <see cref="GameObject"/> is <see cref="IDeusObject"/> : that means that it implement Start(), Update(decimal) and Stop() and have an identifier,
    /// it is also an <see cref="ExecutableObjectsHolder{DeusComponent}"/> : that means that it has a <see cref="List{DeusComponent}"/> and during the Start(), Stop() or Update(decimal), it call also thoses function for its components
    /// So almost every behavior is handled in <see cref="ExecutableObjectsHolder{DeusComponent}"/>
    /// </summary>
    public class GameObject : ExecutableObjectsHolder<DeusComponent>, IDeusObject
    {
        protected uint m_uniqueIdentifier;
        public uint UniqueIdentifier { get => m_uniqueIdentifier; protected set => m_uniqueIdentifier = value; }

        protected EObjectType m_objectType;
        public EObjectType ObjectType { get => m_objectType; protected set => m_objectType = value; }

        public GameObject(UInt32 identifier, EObjectType objectType, ICollection<DeusComponent> components = null) : base(components)
        {
            UniqueIdentifier = identifier;
            ObjectType = objectType;
        }

        public IEnumerable<IViewableComponent> GetViewableGameComponents()
        {
            return m_holdedObjects.Where(obj => obj is IViewableComponent).Select(obj => obj as IViewableComponent);
        }

        public DeusComponent GetComponent(uint id)
        {
            return m_holdedObjects.FirstOrDefault(compo => compo.UniqueIdentifier == id);
        }
    }
}
