using DeusClientCore.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DeusComponentLinker : MonoBehaviour {

    protected uint m_linkedComponentId;
    protected IViewableComponent m_deusLinkedComponent;
    protected bool m_needRealtimeUpdate;

    public uint GetComponentId()
    {
        return m_linkedComponentId;
    }

    public void Init(IViewableComponent linkedComponent)
    {
        m_deusLinkedComponent = linkedComponent;
        m_linkedComponentId = m_deusLinkedComponent.UniqueIdentifier;
        m_needRealtimeUpdate = m_deusLinkedComponent.RealtimeViewUpdate;

        OnInit();
    }

    public virtual void OnInit()
    {

    }

    public abstract void UpdateViewValue(System.Object value);
}
