using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Components
{
    /// <summary>
    /// This interface has to be extended by a <see cref="DeusComponent"/> and force the <see cref="DeusComponent"/> to implement a method to give a matching <see cref="DeusViewComponent"/> to its current <see cref="DeusComponent"/>.
    /// The <see cref="DeusViewComponent"/> are used by the <see cref="GameView"/> to communicate quickly with the <see cref="GameLogic"/> without letting renderer to change <see cref="GameObject"/>.
    /// This permit to have readonly informations for the specific renderer
    /// </summary>
    public interface IViewableComponent
    {
        /// <summary>
        /// We need to have <see cref="DeusComponent"/> only for the view (without all infos) linked to another <see cref="DeusComponent"/> but without expose everything
        /// </summary>
        /// <returns>The <see cref="DeusViewComponent"/> that match with the current <see cref="DeusComponent"/></returns>
        DeusViewComponent GetViewComponent();
    }
}
