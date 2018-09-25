using System;
using System.Collections.Generic;

public class BinarySearchTree<T> where T : IComparable<T>
{
    private class Node
    {
        public T Value { get; private set; }

        public Node Left { get; set; }

        public Node Right { get; set; }

        public Node(T value)
        {
            this.Value = value;
        }
    }

    private Node root;

    public BinarySearchTree()
    {

    }

    private BinarySearchTree(Node node)
    {
        this.Copy(node);
    }

    private void Copy(Node node)
    {
        if (node == null)
        {
            return;
        }

        this.Insert(node.Value);
        this.Copy(node.Left);
        this.Copy(node.Right);
    }

    public void Insert(T value)
    {
        var newNode = new Node(value);
        if (this.root == null)
        {
            this.root = newNode;
        }

        var current = this.root;
        Node parent = null;

        while (current != null)
        {
            parent = current;

            var diffLeft = value.CompareTo(current.Value);
            var diffRight = value.CompareTo(current.Value);

            if (diffLeft < 0)
            {
                current = current.Left;
            }
            else if (diffRight > 0)
            {
                current = current.Right;
            }
            else
            {
                break;
            }
        }

        var parentLeftDiff = value.CompareTo(parent.Value);
        var parentRightDiff = value.CompareTo(parent.Value);

        if (parentLeftDiff < 0)
        {
            parent.Left = newNode;
        }
        else if (parentRightDiff > 0)
        {
            parent.Right = newNode;
        }
    }

    public bool Contains(T value)
    {
        var current = GetCurrentNode(value, this.root);
        return current != null;
    }

    private static Node GetCurrentNode(T value, Node current)
    {
        while (current != null)
        {
            var diffLeft = value.CompareTo(current.Value);
            var diffRight = value.CompareTo(current.Value);

            if (diffLeft < 0)
            {
                current = current.Left;
            }
            else if (diffRight > 0)
            {
                current = current.Right;
            }
            else
            {
                break;
            }
        }

        return current;
    }

    public void DeleteMin()
    {
        if (this.root == null)
        {
            return;
        }
        var current = this.root;
        Node parent = null;

        while (current.Left != null)
        {
            parent = current;
            current = current.Left;
        }

        if (parent == null)
        {
            this.root = this.root.Right;
        }
        else
        {
            parent.Left = current.Right;
        }
    }

    public BinarySearchTree<T> Search(T item)
    {
        var current = GetCurrentNode(item, this.root);
        return new BinarySearchTree<T>(current);
    }

    public IEnumerable<T> Range(T startRange, T endRange)
    {
        var queue = new Queue<T>();
        this.AddInRange(queue, startRange, endRange, this.root);
        return queue;
    }

    private void AddInRange(Queue<T> queue, T startRange, T endRange, Node node)
    {
        if (node == null)
        {
            return;
        }

        var diffLeft = startRange.CompareTo(node.Value);
        var diffRight = endRange.CompareTo(node.Value);

        if (diffLeft < 0)
        {
            this.AddInRange(queue, startRange, endRange, node.Left);
        }
        if (diffLeft <= 0 && diffRight >= 0)
        {
            queue.Enqueue(node.Value);
        }
        if (diffRight > 0)
        {
            this.AddInRange(queue, startRange, endRange, node.Right);
        }
    }

    public void EachInOrder(Action<T> action)
    {
        this.InOrder(action, this.root);
    }

    private void InOrder(Action<T> action, Node node)
    {
        if (node == null)
        {
            return;
        }

        this.InOrder(action, node.Left);
        action(node.Value);
        this.InOrder(action, node.Right);
    }
}

public class Launcher
{
    public static void Main(string[] args)
    {
        var binary = new BinarySearchTree<int>();
        binary.Insert(5);
        binary.Insert(2);
        binary.Insert(7);
        binary.Insert(1);
        binary.Insert(3);

        Console.WriteLine(binary.Contains(8));
    }
}