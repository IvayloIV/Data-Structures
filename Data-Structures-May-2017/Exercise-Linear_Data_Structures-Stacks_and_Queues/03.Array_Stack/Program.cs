using System;

namespace _03.Array_Stack
{
    class Program
    {
        static void Main(string[] args)
        {
            var stack = new ArrayStack<int>(2);
            stack.Push(2);
            stack.Push(3);
            stack.Push(4);
            stack.Pop();
            Console.WriteLine(String.Join(", ", stack.ToArray()));
        }
    }
}