using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoratorPattern
{
    public class NudeComponent : IComponent
    {
        public void Wear()
        {
            Console.WriteLine("什么也没穿，空空如也！");
        }
    }
}
