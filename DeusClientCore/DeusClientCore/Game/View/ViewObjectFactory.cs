using DeusClientCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    //public struct ViewObjectCreateArgs
    //{
    //    public EObjectType Type { get; private set; }
    //    public GameObject LinkedGameObject { get; private set; }

    //    public ViewObjectCreateArgs(EObjectType type, GameObject linkedGameObject)
    //    {
    //        Type = type;
    //        LinkedGameObject = linkedGameObject;
    //    }
    //}

    //public class ViewObjectFactory
    //{       

    //    public ViewObject CreateViewObject(ViewObjectCreateArgs args)
    //    {
    //        ViewObject viewObject = new ViewObject(args.LinkedGameObject.UniqueIdentifier, args.Type);

    //        // fill our viewobject with components by copying the ones we want
    //        List<IViewableComponent> gameComponents = args.LinkedGameObject.GetViewableGameComponents().ToList();
    //        if(gameComponents != null && gameComponents.Count > 0)
    //        {
    //            // foreach viewable components, we get the ViewComponent associated and add it to our list of ViewComponents
    //            foreach (var component in gameComponents)
    //            {
    //                viewObject.AddObject(component.GetViewComponent());
    //            }
    //        }

    //        return viewObject;
    //    }

    //}

}
