using System;

class Program
{
    static void Main(string[] args)
    {
        AVL<int> tree = new AVL<int>();
        tree.Insert(30);
        tree.Insert(25);
        tree.Insert(50);
        tree.Insert(20);
        tree.Insert(28);
        tree.Insert(38);
        tree.Insert(55);
        tree.Insert(29);
        tree.Insert(20);
        tree.Insert(35);
        tree.Insert(40);
        tree.Insert(52);
        tree.Insert(60);
        tree.Insert(32);
        tree.Insert(51);
        tree.Insert(53);
        tree.Insert(58);
        tree.Insert(65);
        tree.Delete(30);

        Console.WriteLine();
    }
}
