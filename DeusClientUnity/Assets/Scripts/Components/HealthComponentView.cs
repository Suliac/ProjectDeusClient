using DeusClientCore.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponentView : DeusComponentLinker
{
    [SerializeField]
    private int m_currentHealth;
      
    public override void UpdateViewValue(System.Object value)
    {
        if (value is int)
            m_currentHealth = (int)value;

        // TODO : change visuals
    }
}
