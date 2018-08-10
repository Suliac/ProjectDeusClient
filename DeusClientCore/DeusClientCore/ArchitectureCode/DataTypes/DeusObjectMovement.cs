using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public class DeusObjectMovement
    {
        public DeusVector2 Dir { get; set; }
        public DeusVector2 Position { get; set; }

        public DeusObjectMovement()
        {

        }

        public DeusObjectMovement(DeusVector2 positionOrigin, DeusVector2 dir)
        {
            Dir = dir;
            Position = positionOrigin;
        }
    }
}
