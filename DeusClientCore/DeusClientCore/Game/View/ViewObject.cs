using DeusClientCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    ///// <summary>
    ///// A <see cref="ViewObject"/> is <see cref="IDeusObject"/> : that means that it implement Start(), Update(decimal) and Stop() and have an identifier,
    ///// it is also an <see cref="ExecutableObjectsHolder{DeusViewComponent}"/> : that means that it has a <see cref="List{DeusViewComponent}"/> and during the Start(), Stop() or Update(decimal), it call also thoses function for its components
    ///// </summary>
    //public class ViewObject : ExecutableObjectsHolder<DeusViewComponent>, IDeusObject
    //{        
    //    protected uint m_uniqueIdentifier;
    //    public uint UniqueIdentifier { get => m_uniqueIdentifier; protected set => m_uniqueIdentifier = value; }

    //    protected EObjectType m_objectType;
    //    public EObjectType ObjectType { get => m_objectType; protected set => m_objectType = value; }

    //    public ViewObject(uint linkedGameObjectId, EObjectType objectType, ICollection<DeusViewComponent> viewComponents = null) : base (viewComponents)
    //    {
    //        UniqueIdentifier = linkedGameObjectId; 
    //        ObjectType = objectType;
    //    }
        

    //}
}
