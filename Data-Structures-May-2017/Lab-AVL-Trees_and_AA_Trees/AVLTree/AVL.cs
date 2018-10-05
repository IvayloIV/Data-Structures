using System;

public class AVL<T> where T : IComparable<T>
{
    private Node<T> root;

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

    public void Insert(T item)
    {
        this.root = this.Insert(this.root, item);
    }

    public void EachInOrder(Action<T> action)
    {
        this.EachInOrder(this.root, action);
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

    private Node<T> Insert(Node<T> node, T item)
    {
        if (node == null)
        {
            return new Node<T>(item);
        }

        int cmp = item.CompareTo(node.Value);
        if (cmp < 0)
        {
            node.Left = this.Insert(node.Left, item);
        }
        else if (cmp > 0)
        {
            node.Right = this.Insert(node.Right, item);
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

    private void EachInOrder(Node<T> node, Action<T> action)
    {
        if (node == null)
        {
            return;
        }

        this.EachInOrder(node.Left, action);
        action(node.Value);
        this.EachInOrder(node.Right, action);
    }
}
