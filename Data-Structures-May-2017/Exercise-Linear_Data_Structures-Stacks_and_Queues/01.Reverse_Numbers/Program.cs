using System;
using System.Collections.Generic;
using System.Linq;

namespace _01.Reverse_Numbers
{
    class Program
    {
        static void Main(string[] args)
        {
            var nums = Console.ReadLine()
            .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

            var stack = new Stack<int>();
            FillStack(stack, nums);
            PrintStack(stack);
        }

        private static void PrintStack(Stack<int> stack)
        {
            while (stack.Count > 0)
            {
                Console.Write(stack.Pop() + " ");
            }
            Console.WriteLine();
        }

        private static void FillStack(Stack<int> stack, int[] nums)
        {
            for (int i = 0; i < nums.Length; i++)
            {
                stack.Push(nums[i]);
            }
        }
    }
}