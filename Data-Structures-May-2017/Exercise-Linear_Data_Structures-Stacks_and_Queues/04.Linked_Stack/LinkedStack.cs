using System;

public class LinkedStack<T>
{
    private Node firstNode;

    public int Count { get; private set; }

    public void Push(T element) 
    {
        this.firstNode = new Node(element, this.firstNode);
        this.Count++;
    }

    public T Pop() 
    {
        if (this.Count == 0)
        {
            throw new InvalidOperationException();
        }

        var value = this.firstNode.value;
        this.firstNode = this.firstNode.NextNode;
        this.Count--;
        return value;
    }

    public T[] ToArray() 
    {
        var arr = new T[this.Count];
        var currentNode = this.firstNode;
        for (int i = 0; i < this.Count; i++)
        {
            arr[i] = currentNode.value;
            currentNode = currentNode.NextNode;
        }
        return arr;
    }

    private class Node
    {
        public T value;
        public Node NextNode { get; set; }
        public Node(T value, Node nextNode = null) 
        {
            this.value = value;
            this.NextNode = nextNode;
        }
    }
}