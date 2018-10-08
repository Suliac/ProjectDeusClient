using DeusClientCore;
using DeusClientCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientConsole
{
    
    public class ViewObjectFactory
    {

        public ViewObject CreateViewObject(ViewObjectCreateArgs args)
        {
            ViewObject viewObject = new ViewObject(args.LinkedGameObject.UniqueIdentifier, args.LinkedGameObject.ObjectType, args.LinkedGameObject.IsLocalPlayer, args.LinkedGameObject.PlayerLinkedId);

            List<IViewableComponent> components = args.LinkedGameObject.GetViewableGameComponents().ToList();

            foreach (var component in components)
            {
                DeusViewComponent tmpComponent = null;
                switch (component.ComponentType)
                {
                    case EComponentType.HealthComponent:
                        tmpComponent = new HealthViewComponent(component, component.UniqueIdentifier, args.LinkedGameObject.UniqueIdentifier);
                        break;
                    case EComponentType.PositionComponent:
                        break;
                    default:
                        break;
                }
                if (tmpComponent != null)
                    viewObject.AddComponent(tmpComponent);
            }
            
            return viewObject;
        }


    }
}
