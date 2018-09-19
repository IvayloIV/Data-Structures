using System;
using System.Collections.Generic;
using System.Linq;

namespace _04.Remove_Odd_Occurrences
{
    class Program
    {
        static void Main(string[] args)
        {
            var nums = Console.ReadLine().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            var oddCountNums = new List<int>();
            for (int index = 0; index < nums.Count; index++)
            {
                var currentNum = nums[index];
                var count = 1;
                for (int innerIndex = 0; innerIndex < nums.Count; innerIndex++)
                {
                    if (currentNum == nums[innerIndex])
                    {
                        count++;
                    }
                }
                if (count % 2 == 1)
                {
                    oddCountNums.Add(currentNum);
                }
            }

            Console.WriteLine(string.Join(" ", oddCountNums));
        }
    }
}