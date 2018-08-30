using DeusClientCore.Components;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthComponentView : DeusComponentLinker
{
    public TextMeshPro componentToUpdate;

    [SerializeField]
    private int m_currentHealth;

    public override void OnInit()
    {
        componentToUpdate = transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>();
        UpdateViewValue((m_deusLinkedComponent as HealthTimeLineComponent).GetViewValue());
    }

    public override void UpdateViewValue(System.Object value)
    {
        if (value is int)
            m_currentHealth = (int)value;

        if (componentToUpdate)
            componentToUpdate.text = $"Life : {m_currentHealth}";
    }
}
