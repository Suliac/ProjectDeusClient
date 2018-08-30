using DeusClientCore;
using DeusClientCore.Components;
using DeusClientCore.Exceptions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ViewObjectFactory : MonoBehaviour
{
    public GameObject playerPrefab;
    private static ViewObjectFactory instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }

    public static GameObject CreateViewObject(ViewObjectCreateArgs args)
    {
        GameObject viewObj = null;
        /////////////////////////////////////////////////////////////
        // Instantiate the good prefab if needed
        switch (args.LinkedGameObject.ObjectType)
        {
            case EObjectType.Player:
                viewObj = Instantiate(instance.playerPrefab);
                viewObj.transform.parent = GameManager.GameObjectContainer.transform;

                if (args.LinkedGameObject.PlayerLinkedId > 0)
                {
                    string playerName = GameManager.PlayerInfos?.FirstOrDefault(pi => pi.Key == args.LinkedGameObject.PlayerLinkedId).Value ?? "";
                    TextMeshPro text = viewObj.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshPro>();
                    if (text)
                        text.text = playerName; 
                }
                break;
            default:
                break;
        }

        /////////////////////////////////////////////////////////////
        // always create deusobjectlinker
        DeusObjectLinker goLinker = viewObj.AddComponent<DeusObjectLinker>();
        goLinker.Init(args.LinkedGameObject.UniqueIdentifier, args.LinkedGameObject.IsLocalPlayer);
        /////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////
        // Manage components
        List<IViewableComponent> components = args.LinkedGameObject.GetViewableGameComponents().ToList();

        uint idComponentPosition = 0;
        foreach (var component in components)
        {
            DeusComponentLinker linker = null;
            switch (component.ComponentType)
            {
                case EComponentType.HealthComponent:
                    linker = viewObj.AddComponent<HealthComponentView>();
                    linker.Init(component);
                    break;
                case EComponentType.PositionComponent:
                    linker = viewObj.AddComponent<PositionComponentView>();
                    linker.Init(component);
                    idComponentPosition = component.UniqueIdentifier;
                    break;
                default:
                    throw new DeusException("Unknown component type. Did you forget to update your ViewObjectFactory ?");
            }
        }
        /////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////
        // Specific behavior if this is the local player
        if (goLinker.IsLocalPlayer())
        {
            PlayerInputHandler handlerInput = viewObj.AddComponent<PlayerInputHandler>();
            handlerInput.ObjectId = goLinker.GetDeusObjectId();
            handlerInput.PositionComponentId = idComponentPosition;

            Renderer rend = viewObj.GetComponentInChildren<Renderer>();
            if (rend)
                rend.material.color = Color.red;

            Camera playerCam = viewObj.transform.GetChild(1).GetComponentInChildren<Camera>(true);
            if(playerCam)
            {
                playerCam.gameObject.SetActive(true);
                GameManager.Instance.SetCamera(false);
            }

        }
        /////////////////////////////////////////////////////////////

        return viewObj;
    }

}
