using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03.Longest_Subsequence
{
    class Program
    {
        static void Main(string[] args)
        {
            var nums = Console.ReadLine().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            List<int> longestSequence = FindLongestSequence(nums);
            Console.WriteLine(string.Join(" ", longestSequence));
        }

        private static List<int> FindLongestSequence(List<int> nums)
        {
            var maxElement = nums[0];
            var currentElement = nums[0];
            var maxSequence = 1;
            var currentSequence = 1;
            for (int i = 1; i < nums.Count; i++)
            {
                if (currentElement == nums[i])
                {
                    currentSequence++;
                    if (maxSequence < currentSequence)
                    {
                        maxSequence = currentSequence;
                        maxElement = currentElement;
                    }
                } 
                else 
                {
                    currentSequence = 1;
                    currentElement = nums[i];
                }
            }

            return CreateList(maxElement, maxSequence);
        }

        private static List<int> CreateList(int maxElement, int maxSequence)
        {
            var list = new List<int>(maxSequence);
            for (int index = 0; index < maxSequence; index++)
            {
                list.Add(maxElement);
            }

            return list;
        }
    }
}
