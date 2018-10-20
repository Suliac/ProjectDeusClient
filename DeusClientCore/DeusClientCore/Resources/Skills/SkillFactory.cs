using DeusClientCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DeusClientCore.Resources.Skills
{
    public class SkillFactory
    {
        public List<SkillModel> LoadSkill(string filePath)
        {
            List<SkillModel> skills = new List<SkillModel>();

            XDocument doc = XDocument.Load(filePath);

            XElement root = doc.Element("skills");
            for (XElement element = root.Element("skill"); element != null; element = element.ElementsAfterSelf().FirstOrDefault(elem => elem.Name == "skill"))
            {
                try
                {
                    skills.Add(CreateSkillFromXML(element));
                }
                catch (DeusException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return skills;
        }

        private SkillModel CreateSkillFromXML(XElement rootElement)
        {
            SkillModel skill = new SkillModel();

            // Get ID
            uint id = 0;
            if (!uint.TryParse(rootElement.Element("id").Value, out id))
                throw new DeusException("Cannot read the id of the skill");
            skill.Id = id;

            // Get Name
            skill.Name = rootElement.Element("name").Value;

            // Get Shape
            skill.IsCircle = rootElement.Element("shape").Value == "circle";

            // Get Cast time
            uint castTime = 0;
            if (!uint.TryParse(rootElement.Element("casttime").Value, out castTime))
                throw new DeusException("Cannot read the cast time of the skill");
            skill.CastTime = castTime;

            // Get Scope
            ushort scope = 0;
            if (!ushort.TryParse(rootElement.Element("scope").Value, out scope))
                throw new DeusException("Cannot read the scope of the skill");
            skill.MaxScope = scope;

            // Get Scope
            ushort radius = 0;
            if (!ushort.TryParse(rootElement.Element("radius").Value, out radius))
                throw new DeusException("Cannot read the radius of the skill");
            skill.Radius = radius;

            // Get Level
            ushort level = 0;
            if (!ushort.TryParse(rootElement.Element("level").Value, out level))
                throw new DeusException("Cannot read the level of the skill");
            skill.Level = level;

            // Get Mana cost
            ushort manaCost = 0;
            if (!ushort.TryParse(rootElement.Element("manacost").Value, out level))
                throw new DeusException("Cannot read the manacost of the skill");
            skill.ManaCost = manaCost;

            // Get Effects
            skill.Effects = new List<SkillEffect>();
            XElement rootEffect = rootElement.Element("effects");
            for (XElement effectElement = rootEffect.Element("effect"); effectElement != null; effectElement = effectElement.ElementsAfterSelf().FirstOrDefault(elem => elem.Name == "effect"))
                skill.Effects.Add(CreateSkillEffectFromXML(effectElement));

            ////////////////////////////////////////////////////////
            Console.WriteLine($"Skill loaded : {skill.ToString()}");
            return skill;
        }

        private SkillEffect CreateSkillEffectFromXML(XElement rootElement)
        {
            SkillEffect effect = new SkillEffect();

            // Get duration
            float duration = 0.0f;
            if (!float.TryParse(rootElement.Element("duration").Value, out duration))
                throw new DeusException("Cannot read the duration of the skill");
            effect.Duration = duration;

            // Get damages
            float damages = 0.0f;
            if (!float.TryParse(rootElement.Element("damages").Value, out damages))
                throw new DeusException("Cannot read the damage of the skill");
            effect.Damages = damages;

            return effect;
        }
    }
}
