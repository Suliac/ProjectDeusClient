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
        public PlayerViewObject(uint linkedGameObjectId) : base(linkedGameObjectId, EObjectType.Player, null)
        {
        }

        public static ViewObject Create(ViewObjectCreateArgs args)
        {
            PlayerViewObject viewObj = new PlayerViewObject(args.LinkedGameObject.UniqueIdentifier);
            List<IViewableComponent> components = args.LinkedGameObject.GetViewableGameComponents().ToList();

            if (components.Count != 1)
                throw new DeusException("Try to create PlayerViewObject without the right number of components");

            foreach (var component in components)
            {
                if (component is HealthTimeLineComponent)
                {
                    viewObj.AddComponent(new HealthViewComponent(component, component.UniqueIdentifier));
                }
            }
            
            if (components.Count != 1)
                throw new DeusException("Try to create PlayerViewObject without the right number of components");

            return viewObj;
        }
    }
}
