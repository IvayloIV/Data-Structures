using System;

public class Node<T>
    where T : IComparable<T>
{
    public Node(T value)
    {
        this.Value = value;
    }

    public Node<T> Next { get; set; }
    public Node<T> Prev { get; set; }
    public T Value { get; }
}