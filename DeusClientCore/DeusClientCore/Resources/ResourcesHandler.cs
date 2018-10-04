using DeusClientCore.Resources.Skills;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Resources
{
    public class ResourcesHandler
    {
        private static ResourcesHandler m_instance;

        public static ResourcesHandler Get()
        {
            if (m_instance == null)
                m_instance = new ResourcesHandler();

            return m_instance;
        }

        private ResourcesHandler()
        {
            // Load skill
            SkillFactory factory = new SkillFactory();
            Console.WriteLine("Start loading skills");
            try
            {
                Skills = factory.LoadSkill("skills.xml");
                Console.WriteLine("Finish loading skills");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public List<Skill> Skills { get; set; }
    }
}
