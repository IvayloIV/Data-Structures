using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05.Count_of_Occurrences
{
    class Program
    {
        static void Main(string[] args)
        {
            var nums = Console.ReadLine()
                .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .OrderBy(a => a).ToList();

            var result = new List<string>(); 
            for (int index = 0; index < nums.Count; index++)
            {
                var currentNumber = nums[index];

                var counter = 1;
                for (int innerIndex = index + 1; innerIndex < nums.Count; innerIndex++)
                {
                    if (currentNumber == nums[innerIndex])
                    {
                        counter++;
                    } 
                    else 
                    {
                        break;
                    }
                }
                index += counter - 1;

                result.Add($"{currentNumber} -> {counter} times");
            }

            Console.WriteLine(string.Join(Environment.NewLine, result));
        }
    }
}
