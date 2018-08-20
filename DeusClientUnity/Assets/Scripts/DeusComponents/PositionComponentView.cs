using DeusClientCore;
using DeusClientCore.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionComponentView : DeusComponentLinker
{
    [SerializeField]
    private DeusVector2 m_objectPos;

    [SerializeField]
    private DeusVector2 m_lastObjectPos;

    public override void UpdateViewValue(System.Object value)
    {
        if ((value is DeusVector2))
        {
            m_lastObjectPos = m_objectPos;
            m_objectPos = (DeusVector2)value;
        }
    }

    private void Update()
    {
        // We want to get the position as fast as possible ! So instead of waiting for update, we interpolate directly the value
        if (m_deusLinkedComponent is PositionTimeLineComponent)
        {
            UpdateViewValue((m_deusLinkedComponent as PositionTimeLineComponent).GetViewValue());

            if (Mathf.Pow(m_objectPos.Y - m_lastObjectPos.Y, 2) > 1)
                Debug.Log($"Something strange happened");
            //Debug.Log($"{m_objectPos.X}, {m_objectPos.Y}");
            // update transform here
            gameObject.transform.position = new Vector3(m_objectPos.X, 0, m_objectPos.Y);
        }
    }
}
