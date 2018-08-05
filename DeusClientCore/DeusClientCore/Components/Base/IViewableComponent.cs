using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Components
{
    /// <summary>
    /// This interface has to be extended by a <see cref="DeusComponent"/> and force the <see cref="DeusComponent"/> to implement a method to give the current informations of component to the game view
    /// This permit to have readonly informations for the specific renderer
    /// </summary>
    public interface IViewableComponent
    {
        object GetViewValue();
    }
}
