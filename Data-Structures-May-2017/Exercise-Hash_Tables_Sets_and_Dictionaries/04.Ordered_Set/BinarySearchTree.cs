using System;
using System.Collections.Generic;

public class BinarySearchTree<T> where T : IComparable<T>
{
    private class Node 
    {
        public Node Left { get; set; }

        public Node Right { get; set; }

        public T Value { get; private set; }

        public Node(T value)
        {
            this.Value = value;
        }
    }

    private Node root;

    public int Count;

    public void Add(T item)
    {
        this.root = this.Add(this.root, item);
    }

    private Node Add(Node node, T item)
    {
        if (node == null)
        {
            this.Count++;
            return new Node(item);
        }

        var cmp = node.Value.CompareTo(item);
        if (cmp > 0)
        {
            node.Left = this.Add(node.Left, item);
        }
        else if (cmp < 0)
        {
            node.Right = this.Add(node.Right, item);
        }

        return node;
    }

    public IEnumerable<T> InOrder()
    {
        var result = new List<T>();
        this.InOrder(result, this.root);
        return result;
    }

    private void InOrder(List<T> result, Node node)
    {
        if (node == null)
        {
            return;
        }

        this.InOrder(result, node.Left);
        result.Add(node.Value);
        this.InOrder(result, node.Right);
    }

    public bool Contains(T item)
    {
        return this.Contains(this.root, item);
    }

    private bool Contains(Node node, T item)
    {
        if (node == null)
        {
            return false;
        }

        var cmp = node.Value.CompareTo(item);
        if (cmp > 0)
        {
            return this.Contains(node.Left, item);
        }
        else if (cmp < 0)
        {
            return this.Contains(node.Right, item);
        }

        return true;
    }

    public void Remove(T item)
    {
        this.root = this.Remove(this.root, item);
    }

    private Node Remove(Node node, T item)
    {
        if (node == null)
        {
            return node;
        }

        var cmp = node.Value.CompareTo(item);
        if (cmp > 0)
        {
            node.Left = this.Remove(node.Left, item);
        }
        else if (cmp < 0)
        {
            node.Right = this.Remove(node.Right, item);
        }
        else 
        {
            this.Count--;
            if (node.Left == null)
            {
                return node.Right;
            }
            else if (node.Right == null)
            {
                return node.Left;
            }
            else
            {
                var maxLeftElement = this.FindMaxLeftElement(node.Right);
                node.Right = this.DeleteMin(node.Right);
                maxLeftElement.Left = node.Left;
                maxLeftElement.Right = node.Right;
                return maxLeftElement;
            }
        }

        return node;
    }

    private Node DeleteMin(Node node)
    {
        if (node.Left == null)
        {
            return node.Right;
        }

        return this.DeleteMin(node.Left);
    }

    private Node FindMaxLeftElement(Node node)
    {
        if (node.Left == null)
        {
            return node;
        }

        return this.FindMaxLeftElement(node.Left);
    }
}