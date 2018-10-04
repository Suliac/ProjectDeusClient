using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Resources
{
    public class SkillEffect
    {
        public float Duration { get; set; }
        public float Damages { get; set; }

        // TODO : Buff/effects

        public override string ToString()
        {
            return $"[EFFECT => Duration : {Duration} | Damages : {Damages}]";
        }
    }
}
