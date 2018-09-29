using System;

public class HeapExample
{
    static void Main()
    {
        var arr = new int[] { 5, 7, 1, 3, -2, 8 };
        Heap<int>.Sort(arr);
        Console.WriteLine(string.Join(" - ", arr));
    }
}
