using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06.Reversed_List
{
    class Program
    {
        static void Main(string[] args)
        {
            var reversedList = new ReversedList<int>();

            reversedList.Add(1);
            reversedList.Add(2);
            reversedList.Add(3);
            reversedList.Add(4);

            Console.WriteLine(reversedList.RemoveAt(3));
            Console.WriteLine(reversedList.RemoveAt(0));

            Console.WriteLine(reversedList.Count);
            Console.WriteLine(reversedList.Capacity);
        }
    }
}