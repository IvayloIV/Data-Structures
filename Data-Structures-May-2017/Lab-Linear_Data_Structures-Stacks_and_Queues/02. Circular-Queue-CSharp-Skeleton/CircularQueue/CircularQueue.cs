using System;

public class CircularQueue<T>
{
    private const int DefaultCapacity = 4;

    public int Count { get; private set; }

    private int startIndex;
    private int endIndex;
    private T[] data;

    public CircularQueue(int capacity = DefaultCapacity)
    {
        this.data = new T[capacity];
    }

    public void Enqueue(T element)
    {
        if (this.Count >= this.data.Length)
        {
            this.Resize();
        }

        this.data[this.endIndex] = element;
        this.endIndex = (this.endIndex + 1) % this.data.Length;
        this.Count++;
    }

    private void Resize()
    {
        var newArr = new T[this.data.Length * 2];
        this.data = this.CopyAllElements(newArr);
        this.startIndex = 0;
        this.endIndex = this.Count;
    }

    private T[] CopyAllElements(T[] newArray)
    {
        var currentIndex = 0;
        while (currentIndex < this.Count)
        {
            newArray[currentIndex++] = this.data[this.startIndex];
            this.startIndex = (this.startIndex + 1) % this.data.Length;
        }

        return newArray;
    }

    // Should throw InvalidOperationException if the queue is empty
    public T Dequeue()
    {
        if (this.Count == 0)
        {
            throw new InvalidOperationException();
        }

        var value = this.data[this.startIndex];
        this.startIndex = (this.startIndex + 1) % this.data.Length;
        this.Count--;
        return value;
    }

    public T[] ToArray()
    {
        var newArr = new T[this.Count];
        newArr = CopyAllElements(newArr);
        return newArr;
    }
}


public class Example
{
    public static void Main()
    {

        CircularQueue<int> queue = new CircularQueue<int>();

        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);
        queue.Enqueue(4);
        queue.Enqueue(5);
        queue.Enqueue(6);

        Console.WriteLine("Count = {0}", queue.Count);
        Console.WriteLine(string.Join(", ", queue.ToArray()));
        Console.WriteLine("---------------------------");

        int first = queue.Dequeue();
        Console.WriteLine("First = {0}", first);
        Console.WriteLine("Count = {0}", queue.Count);
        Console.WriteLine(string.Join(", ", queue.ToArray()));
        Console.WriteLine("---------------------------");

        queue.Enqueue(-7);
        queue.Enqueue(-8);
        queue.Enqueue(-9);
        Console.WriteLine("Count = {0}", queue.Count);
        Console.WriteLine(string.Join(", ", queue.ToArray()));
        Console.WriteLine("---------------------------");

        first = queue.Dequeue();
        Console.WriteLine("First = {0}", first);
        Console.WriteLine("Count = {0}", queue.Count);
        Console.WriteLine(string.Join(", ", queue.ToArray()));
        Console.WriteLine("---------------------------");

        queue.Enqueue(-10);
        Console.WriteLine("Count = {0}", queue.Count);
        Console.WriteLine(string.Join(", ", queue.ToArray()));
        Console.WriteLine("---------------------------");

        first = queue.Dequeue();
        Console.WriteLine("First = {0}", first);
        Console.WriteLine("Count = {0}", queue.Count);
        Console.WriteLine(string.Join(", ", queue.ToArray()));
        Console.WriteLine("---------------------------");
    }
}
