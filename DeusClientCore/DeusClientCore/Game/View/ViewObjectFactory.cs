using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public struct ViewObjectCreateArgs
    {
        public EObjectType Type { get; private set; }
        public GameObject LinkedGameObject { get; private set; }

        public ViewObjectCreateArgs(EObjectType type, GameObject linkedGameObject)
        {
            Type = type;
            LinkedGameObject = linkedGameObject;
        }
    }

    public abstract class ViewObjectFactory
    {
        public abstract ViewObject CreateViewObject(ViewObjectCreateArgs args);
    }

}
