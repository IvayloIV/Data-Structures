using System;
using System.Linq;

namespace _01.Sum_and_Average
{
    class Program
    {
        static void Main(string[] args)
        {
            var nums = Console.ReadLine().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            var sum = 0;
            for (int index = 0; index < nums.Count; index++)
            {
                sum += nums[index];
            }

            double average = 0;
            if (nums.Count != 0) 
            {
                average = (double)sum / nums.Count;
            }
            Console.WriteLine($"Sum={sum}; Average={average:f2}");
        }
    }
}