using System;
using System.Collections.Generic;
using System.Linq;

namespace _06.Sequence_N_M
{
    class Program
    {
        static void Main(string[] args)
        {
            var nums = Console.ReadLine()
            .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

            var startNum = nums[0];
            var endNum = nums[1];
            var queue = new Queue<Item<int>>();
            queue.Enqueue(new Item<int>(startNum));

            while (queue.Count > 0)
            {
                var currentItem = queue.Dequeue();
                var value = currentItem.Value;

                if (value < endNum)
                {
                    queue.Enqueue(new Item<int>(value + 1, currentItem));
                    queue.Enqueue(new Item<int>(value + 2, currentItem));
                    queue.Enqueue(new Item<int>(value * 2, currentItem));
                }
                else if (value == endNum)
                {
                    var result = new List<int>();
                    while (currentItem != null)
                    {
                        result.Add(currentItem.Value);
                        currentItem = currentItem.PrevItem;
                    }
                    for (int i = result.Count - 1; i >= 1; i--)
                    {
                        Console.Write(result[i] + " -> ");
                    }
                    Console.WriteLine(result[0]);
                    break;
                }
            }
        }
    }
}
