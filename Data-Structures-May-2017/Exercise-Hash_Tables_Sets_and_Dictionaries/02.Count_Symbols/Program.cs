using System;
using System.Linq;

namespace _02.Count_Symbols
{
    class Program
    {
        static void Main(string[] args)
        {
            var line = Console.ReadLine().ToCharArray();
            var dict = new Dictionary<char, int>();

            for (int i = 0; i < line.Length; i++)
            {
                if (!dict.ContainsKey(line[i]))
                {
                    dict[line[i]] = 0;
                }
                dict[line[i]]++;
            }

            foreach (var item in dict.OrderBy(a => a.Key))
            {
                Console.WriteLine($"{item.Key}: {item.Value} time/s");
            }
        }
    }
}
