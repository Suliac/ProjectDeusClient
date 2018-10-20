using DeusClientCore.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillComponentView : DeusComponentLinker
{

    private SkillInfos m_currentSkillInfos;

    private GameObject m_casting = null;
    private GameObject m_currentSkillEffect = null;

    public override void OnInit()
    {
    }

    /// <summary>
    /// This function is called when the view received a <see cref="DeusClientCore.Packets.PacketUpdateViewObject"/>
    /// </summary>
    /// <param name="value">The new value for the component</param>
    public override void UpdateViewValue(object value)
    {
        if (value is SkillInfos)
        {
            m_currentSkillInfos = value as SkillInfos;
            switch (m_currentSkillInfos.State)
            {
                case SkillState.NotLaunched:
                    break;
                case SkillState.Casting:
                    // TODO : Display cast bar
                    if (m_casting)
                        m_casting.SetActive(true);
                    break;
                case SkillState.Launched:
                    // TODO : Start to display skill VFX
                    if (m_casting)
                        m_casting.SetActive(false);

                    m_currentSkillEffect = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

                    Renderer rend = m_currentSkillEffect.GetComponent<Renderer>();
                    if (rend)
                        rend.material.color = Color.green;

                    m_currentSkillEffect.transform.position = new Vector3(m_currentSkillInfos.Position.X, 0.0f, m_currentSkillInfos.Position.Y);
                    m_currentSkillEffect.transform.localScale = new Vector3(m_currentSkillInfos.Radius, 0.1f, m_currentSkillInfos.Radius);
                    break;
                case SkillState.Finished:
                    // TODO : Clean/Display ending VFX
                    break;
                default:
                    break;
            }
        }
    }
}
