using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegate
{
    class Program
    {
        static void Main(string[] args)
        {
            Calculate(1, 2, Add);
            Calculate(1, 1, Divisi);

            Calculate(1, 2, delegate (double a,double b) { return a + b; });
            Calculate(1, 1, delegate (double a, double b) { return a / b; });

            Calculate(1, 2, (double a,double b) => { return a + b; });
            Calculate(1, 2, (a,b) => { return a + b; });


            int[] nums = { 1, 2, 3 };

            //CallbackFunction.Double(nums);
            //CallbackFunction.AddOne(nums);

            CallbackFunction.DoubleAndAddOne(nums, n => n + 1);
        }

        public enum Operate
        {
            Add,
            Subtrac,
            Multip,
            Divisi
        }

        public delegate double CalculateDelegate(double a, double b);

        public static double Calculate(double a, double b, CalculateDelegate operate)
        {
            return operate(a, b);
        }


        public static double Calculate(double a,double b, Operate operate)
        {
            switch(operate)
            {
                case Operate.Add:
                    return Add(a, b);
                case Operate.Subtrac:
                    return Subtrac(a, b);
                case Operate.Multip:
                    return Multip(a, b);
                case Operate.Divisi:
                    return Divisi(a, b);
                default:
                    return 0;
            }
        }

        public static double Add(double a , double b)
        {
            return a + b;
        }
        public static double Subtrac(double a, double b)
        {
            return a - b;
        }
        public static double Multip(double a, double b)
        {
            return a * b;
        }
        public static double Divisi(double a, double b)
        {
            if (b == 0) throw new Exception("分母不能为0");
            return a / b;
        }



    }
}
