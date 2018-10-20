using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05.Balanced_Ordered_Set
{
    class Program
    {
        static void Main(string[] args)
        {
            var set = new OrderedSet<int>();
            set.Add(17);
            set.Add(19);
            set.Add(9);
            set.Add(12);
            set.Add(6);

            set.Remove(17);

            Console.WriteLine(set.Count);

            foreach (var item in set)
            {
                Console.WriteLine(item);
            }
        }
    }
}
