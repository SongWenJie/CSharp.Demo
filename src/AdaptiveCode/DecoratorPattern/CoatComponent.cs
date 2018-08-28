using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoratorPattern
{
    public class CoatComponent : IComponent
    {
        private readonly IComponent component;
        public CoatComponent(IComponent component)
        {
            this.component = component;
        }

        public void Wear()
        {
            component.Wear();
            Console.WriteLine("穿好上衣啦！");
        }
    }
}
