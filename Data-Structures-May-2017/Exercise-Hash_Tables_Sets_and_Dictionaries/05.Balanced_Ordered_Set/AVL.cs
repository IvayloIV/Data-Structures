using System;
using System.Collections.Generic;

public class AVL<T> where T : IComparable<T>
{
    private Node<T> root;

    public int Count;

    public Node<T> Root
    {
        get
        {
            return this.root;
        }
    }

    public bool Contains(T item)
    {
        var node = this.Search(this.root, item);
        return node != null;
    }

    public void Add(T item)
    {
        this.root = this.Add(this.root, item);
    }

    private Node<T> RotateLeft(Node<T> node)
    {
        var newSubTree = node.Left;
        node.Left = newSubTree.Right;
        newSubTree.Right = node;

        UpdateHeight(node);

        return newSubTree;
    }

    private Node<T> RotateRight(Node<T> node)
    {
        var newSubTree = node.Right;
        node.Right = newSubTree.Left;
        newSubTree.Left = node;

        this.UpdateHeight(node);

        return newSubTree;
    }

    private void UpdateHeight(Node<T> newSubTree)
    {
        newSubTree.Height = Math.Max(this.GetHeight(newSubTree.Left), this.GetHeight(newSubTree.Right)) + 1;
    }

    private int GetHeight(Node<T> node)
    {
        if (node == null)
        {
            return 0;
        }
        return node.Height;
    }

    private Node<T> Add(Node<T> node, T item)
    {
        if (node == null)
        {
            this.Count++;
            return new Node<T>(item);
        }

        int cmp = item.CompareTo(node.Value);
        if (cmp < 0)
        {
            node.Left = this.Add(node.Left, item);
        }
        else if (cmp > 0)
        {
            node.Right = this.Add(node.Right, item);
        }

        node = this.TryToRotate(node);
        this.UpdateHeight(node);
        return node;
    }

    private Node<T> TryToRotate(Node<T> node)
    {
        var diff = this.GetHeight(node.Left) - this.GetHeight(node.Right);
        if (diff > 1)
        {
            if (this.GetHeight(node.Left.Left) - this.GetHeight(node.Left.Right) < 0)
            {
                node.Left = this.RotateRight(node.Left);
            }
            node = this.RotateLeft(node);
        }
        else if (diff < -1)
        {
            if (this.GetHeight(node.Right.Left) - this.GetHeight(node.Right.Right) > 0)
            {
                node.Right = this.RotateLeft(node.Right);
            }
            node = this.RotateRight(node);
        }

        return node;
    }

    private Node<T> Search(Node<T> node, T item)
    {
        if (node == null)
        {
            return null;
        }

        int cmp = item.CompareTo(node.Value);
        if (cmp < 0)
        {
            return Search(node.Left, item);
        }
        else if (cmp > 0)
        {
            return Search(node.Right, item);
        }

        return node;
    }

    public IEnumerable<T> EachInOrder()
    {
        var result = new List<T>();
        this.EachInOrder(this.root, result);
        return result;
    }

    private void EachInOrder(Node<T> node, List<T> result)
    {
        if (node == null)
        {
            return;
        }

        this.EachInOrder(node.Left, result);
        result.Add(node.Value);
        this.EachInOrder(node.Right, result);
    }

    public void Remove(T item)
    {
        this.root = this.Remove(this.root, item);
        this.root = this.TryToRotate(this.root);
        this.UpdateHeight(this.root);
    }

    private Node<T> Remove(Node<T> node, T item)
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

        node = this.TryToRotate(node);
        this.UpdateHeight(node);
        return node;
    }

    private Node<T> DeleteMin(Node<T> node)
    {
        if (node.Left == null)
        {
            return node.Right;
        }

        node.Left = this.DeleteMin(node.Left);
        return node;
    }

    private Node<T> FindMaxLeftElement(Node<T> node)
    {
        if (node.Left == null)
        {
            return node;
        }

        return this.FindMaxLeftElement(node.Left);
    }
}
