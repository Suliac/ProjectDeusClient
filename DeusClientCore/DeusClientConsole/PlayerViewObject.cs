using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeusClientCore;
using DeusClientCore.Components;

namespace DeusClientConsole
{
    public class PlayerViewObject : ViewObject
    {


        public PlayerViewObject(uint linkedGameObjectId) : base(linkedGameObjectId, EObjectType.Player, null)
        {
        }


    }
}
