using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03.Phonebook
{
    class Program
    {
        static void Main(string[] args)
        {
            var dict = new Dictionary<string, string>();

            string line;
            while ((line = Console.ReadLine()) != "search")
            {
                var tokens = line.Split(new[] { "-" }, StringSplitOptions.None);
                var name = tokens[0];
                var phone = tokens[1];
                dict[name] = phone;
            }

            while ((line = Console.ReadLine()) != "end")
            {
                if (!dict.ContainsKey(line))
                {
                    Console.WriteLine($"Contact {line} does not exist.");
                }
                else
                {
                    Console.WriteLine($"{line} -> {dict[line]}");
                }
            }
        }
    }
}
