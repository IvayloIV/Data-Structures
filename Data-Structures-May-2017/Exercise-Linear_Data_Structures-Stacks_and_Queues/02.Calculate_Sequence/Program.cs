using System;
using System.Collections.Generic;

namespace _02.Calculate_Sequence
{
    class Program
    {
        static void Main(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(n);
            var nums = new List<int>();
            while (nums.Count < 50)
            {
                var currentNum = queue.Dequeue();
                queue.Enqueue(currentNum + 1);
                queue.Enqueue(2 * currentNum + 1);
                queue.Enqueue(currentNum + 2);
                nums.Add(currentNum);
            }
            Console.WriteLine(string.Join(", ", nums));
        }
    }
}