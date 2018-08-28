using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoratorPattern
{
    public class PantsComponent : IComponent
    {
        private readonly IComponent component;
        public PantsComponent(IComponent component)
        {
            this.component = component;
        }

        public void Wear()
        {
            component.Wear();
            Console.WriteLine("穿好裤子啦！");
        }
    }
}
