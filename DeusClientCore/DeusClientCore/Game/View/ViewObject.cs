using DeusClientCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public struct ViewObjectCreateArgs
    {
        public DeusGameObject LinkedGameObject { get; private set; }

        public ViewObjectCreateArgs(DeusGameObject linkedGameObject)
        {
            LinkedGameObject = linkedGameObject;
        }
    }

    /// <summary>
    /// A <see cref="ViewObject"/> is <see cref="IDeusObject"/> : that means that it implement Start(), Update(decimal) and Stop() and have an identifier,
    /// it is also an <see cref="ExecutableObjectsHolder{DeusViewComponent}"/> : that means that it has a <see cref="List{DeusViewComponent}"/> and during the Start(), Stop() or Update(decimal), it call also thoses function for its components
    /// </summary>
    public class ViewObject : ExecutableObjectsHolder<DeusViewComponent>, IDeusObject
    {        
        protected uint m_uniqueIdentifier;
        public uint UniqueIdentifier { get => m_uniqueIdentifier; protected set => m_uniqueIdentifier = value; }

        protected EObjectType m_objectType;
        public EObjectType ObjectType { get => m_objectType; protected set => m_objectType = value; }
        
        protected bool m_isLocalPlayer;
        public bool IsLocalPlayer { get => m_isLocalPlayer; protected set => m_isLocalPlayer = value; }

        public ViewObject(uint linkedGameObjectId, EObjectType objectType, bool isLocalPlayer, ICollection<DeusViewComponent> viewComponents = null) : base (viewComponents)
        {
            UniqueIdentifier = linkedGameObjectId; 
            ObjectType = objectType;
            IsLocalPlayer = isLocalPlayer;
        }

        /// <summary>
        /// Expose AddObject for <see cref="ViewObject"/>
        /// </summary>
        /// <param name="newObject">The <see cref="DeusViewComponent"/> to add</param>
        /// <returns><see cref="true"/> if <see cref="DeusViewComponent"/> was successfully added</returns>
        public bool AddComponent(DeusViewComponent newObject)
        {
            return AddObject(newObject);
        }

        /// <summary>
        /// Expose RemoveObject for <see cref="ViewObject"/>
        /// </summary>
        /// <param name="toDelete">The <see cref="DeusViewComponent"/> to delete</param>
        /// <returns><see cref="true"/> if <see cref="DeusViewComponent"/> was successfully deleted</returns>
        public bool RemoveComponent(DeusViewComponent toDelete)
        {
            return RemoveComponent(toDelete);
        }

        public DeusViewComponent Get(uint id)
        {
            return m_holdedObjects.FirstOrDefault(comp => comp.UniqueIdentifier == id);
        }

    }
}
