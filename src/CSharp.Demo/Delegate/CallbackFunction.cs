using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegate
{
    public class CallbackFunction
    {

        public static void Double(int[] nums)
        {
            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = nums[i] * 2;
            }
        }

        public static void AddOne(int[] nums)
        {
            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = nums[i] + 1;
            }
        }

        public static void DoubleAndAddOne(int[] nums,Func<int,int> func)
        {
            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = func(nums[i] * 2);
            }
        }
    }
}
