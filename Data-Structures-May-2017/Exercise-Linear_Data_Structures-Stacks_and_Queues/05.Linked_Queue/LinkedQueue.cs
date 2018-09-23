using System;

public class LinkedQueue<T>
{
    public int Count { get; private set; }

    private QueueNode<T> Head;
    private QueueNode<T> Tail;

    public void Enqueue(T element)
    {
        var newNode = new QueueNode<T>(element);
        if (this.Count == 0)
        {
            this.Head = this.Tail = newNode;
        }
        else
        {
            this.Head.PrevNode = newNode;
            newNode.NextNode = this.Head;
            this.Head = newNode;
        }
        this.Count++;
    }

    public T Dequeue()
    {
        if (this.Count == 0)
        {
            throw new InvalidOperationException();
        }

        var element = this.Tail.Value;
        this.Tail = this.Tail.PrevNode;
        if (this.Tail == null)
        {
            this.Head = null;
        }
        else
        {
            this.Tail.NextNode = null;
        }
        this.Count--;
        return element;
    }

    public T[] ToArray()
    {
        var arr = new T[this.Count];
        var currentTail = this.Tail;
        for (int i = 0; i < this.Count; i++)
        {
            arr[i] = currentTail.Value;
            currentTail = currentTail.PrevNode;
        }
        return arr;
    }

    private class QueueNode<T>
    {
        public T Value { get; private set; }
        public QueueNode<T> NextNode { get; set; }
        public QueueNode<T> PrevNode { get; set; }

        public QueueNode(T value)
        {
            this.Value = value;
        }
    }
}