using DeusClientCore;
using DeusClientCore.Components;
using DeusClientCore.Exceptions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        switch (args.LinkedGameObject.ObjectType)
        {
            case EObjectType.Player:
                viewObj = CreatePlayer(args);
                break;
            default:
                break;
        }

        return viewObj;
    }

    private static GameObject CreatePlayer(ViewObjectCreateArgs args)
    {
        GameObject viewObj = Instantiate(instance.playerPrefab);

        /////////////////////////////////////////////////////////////
        // always create deusobjectlinker
        DeusObjectLinker goLinker = viewObj.AddComponent<DeusObjectLinker>();
        goLinker.Init(args.LinkedGameObject.UniqueIdentifier, args.LinkedGameObject.IsLocalPlayer);
        /////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////
        // Manage components
        List<IViewableComponent> components = args.LinkedGameObject.GetViewableGameComponents().ToList();
        if (components.Count != 2)
            throw new DeusException("Try to create PlayerViewObject without the right number of components");

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
        }
        /////////////////////////////////////////////////////////////
        
        return viewObj;
    }
}
