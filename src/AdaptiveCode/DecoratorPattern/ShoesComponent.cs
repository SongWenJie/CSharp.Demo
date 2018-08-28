using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoratorPattern
{
    public class ShoesComponent : IComponent
    {
        private readonly IComponent component;
        public ShoesComponent(IComponent component)
        {
            this.component = component;
        }

        public void Wear()
        {
            component.Wear();
            Console.WriteLine("穿好鞋子啦！");
        }
    }
}
