using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pluginprank.CustomClasses
{
    public class CustomClass
    {
        public List<Ability> abilities;
        public string Description;
        public bool onCooldown;

        public CustomClass(List<Ability> abilities, string description)
        {
            this.abilities = abilities;
            Description = description;
        }
    }
}
