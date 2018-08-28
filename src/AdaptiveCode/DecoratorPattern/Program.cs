using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoratorPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            IComponent component = new ShoesComponent(new PantsComponent(
                    new CoatComponent(new NudeComponent())));
            component.Wear();


            Console.ReadKey();
        }
    }
}
