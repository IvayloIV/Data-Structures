using System;

namespace _04.Linked_Stack
{
    class Program
    {
        static void Main(string[] args)
        {
            var stack = new LinkedStack<int>();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            Console.WriteLine(String.Join(", ", stack.ToArray()));
        }
    }
}