using System;
using System.Collections;
using System.Collections.Generic;

public class LinkedList<T> : IEnumerable<T>
{
    public class Node
    {
        public Node(T value)
        {
            this.Value = value;
        }

        public T Value { get; set; }

        public Node Next { get; set; }
    }

    public int Count { get; private set; }

    public Node Head { get; private set; }

    public Node Tail { get; private set; }

    public void AddFirst(T item)
    {
        var newNode = new Node(item);

        if (this.Count == 0)
        {
            this.Head = this.Tail = newNode;
        }
        else
        {
            newNode.Next = this.Head;
            this.Head = newNode;
        }
        this.Count++;
    }

    public void AddLast(T item)
    {
        var newNode = new Node(item);
        if (this.Count == 0)
        {
            this.Tail = this.Head = newNode;
        }
        else
        {
            this.Tail.Next = newNode;
            this.Tail = newNode;
        }

        this.Count++;
    }

    public T RemoveFirst()
    {
        if (this.Count == 0)
        {
            throw new InvalidOperationException();
        }

        var oldValue = this.Head.Value;
        var newNode = this.Head.Next;
        this.Head = newNode;
        if (this.Head == null)
        {
            this.Tail = null;
        }

        this.Count--;

        return oldValue;
    }

    public T RemoveLast()
    {
        if (this.Count == 0)
        {
            throw new InvalidOperationException();
        }

        var oldValue = this.Head.Value;
        if (this.Count == 1)
        {
            this.Head = this.Tail = null;
        }
        else
        {
            var copyNode = this.Head;
            while (copyNode.Next != this.Tail)
            {
                copyNode = copyNode.Next;
            }

            oldValue = copyNode.Next.Value;
            copyNode.Next = null;
            this.Tail = copyNode;
        }

        this.Count--;
        return oldValue;
    }

    public IEnumerator<T> GetEnumerator()
    {
        var current = this.Head;
        while (current != null)
        {
            yield return current.Value;
            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
