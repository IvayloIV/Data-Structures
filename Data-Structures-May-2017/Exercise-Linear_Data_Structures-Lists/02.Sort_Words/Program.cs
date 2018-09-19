using System;
using System.Linq;

namespace _02.Sort_Words
{
    class Program
    {
        static void Main(string[] args)
        {
            var words = Console.ReadLine().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            Console.WriteLine(string.Join(" ", words.OrderBy(a => a)));
        }
    }
}