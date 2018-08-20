using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeusClientCore;
using DeusClientCore.Components;
using DeusClientCore.Exceptions;

namespace DeusClientConsole
{
    public class PlayerViewObject : ViewObject
    {
        public PlayerViewObject(uint linkedGameObjectId, bool isLocalPlayer) : base(linkedGameObjectId, EObjectType.Player, isLocalPlayer, null)
        {
        }

        public static ViewObject Create(ViewObjectCreateArgs args)
        {
            PlayerViewObject viewObj = new PlayerViewObject(args.LinkedGameObject.UniqueIdentifier, args.LinkedGameObject.IsLocalPlayer);
            List<IViewableComponent> components = args.LinkedGameObject.GetViewableGameComponents().ToList();

            if (components.Count != 2)
                throw new DeusException("Try to create PlayerViewObject without the right number of components");

            foreach (var component in components)
            {
                if (component is HealthTimeLineComponent)
                {
                    viewObj.AddComponent(new HealthViewComponent(component, component.UniqueIdentifier));
                }
            }

            if (components.Count != 2)
                throw new DeusException("Try to create PlayerViewObject without the right number of components");

            return viewObj;
        }
    }
}
