using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public class ViewObject : DeusObject
    {
        GameObject m_linkedGameObject;

        public ViewObject(GameObject linkedGameObject, uint identifier) : base(identifier)
        {
            m_linkedGameObject = linkedGameObject;
        }


    }
}
