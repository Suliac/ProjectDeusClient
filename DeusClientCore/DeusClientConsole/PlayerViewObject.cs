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

    }
}
