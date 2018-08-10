using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeusObjectLinker : MonoBehaviour {

    private uint m_linkedDeusObjectId;
    private bool m_isLocalPlayer;

    public void Init(uint linkedObjectId, bool isLocalPlayer)
    {
        m_linkedDeusObjectId = linkedObjectId;
        m_isLocalPlayer = isLocalPlayer;
    }

    public bool IsLocalPlayer()
    {
        return m_isLocalPlayer;
    }

    public uint GetDeusObjectId()
    {
        return m_linkedDeusObjectId;
    }
}
