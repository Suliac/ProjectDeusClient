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
            ViewObject viewObject = null;

            // fill our viewobject with components by copying the ones we want
            List<IViewableComponent> components = args.LinkedGameObject.GetViewableGameComponents().ToList();
            switch (args.LinkedGameObject.ObjectType)
            {
                case EObjectType.Player:
                    viewObject = PlayerViewObject.Create(args);
                    break;
                default:
                    break;
            }

            return viewObject;
        }


    }
}
