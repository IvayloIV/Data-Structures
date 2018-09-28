using System;
using System.Collections.Generic;

public class BinarySearchTree<T> : IBinarySearchTree<T> where T:IComparable
{
    private Node root;

    private Node FindElement(T element)
    {
        Node current = this.root;

        while (current != null)
        {
            if (current.Value.CompareTo(element) > 0)
            {
                current = current.Left;
            }
            else if (current.Value.CompareTo(element) < 0)
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

    private void PreOrderCopy(Node node)
    {
        if (node == null)
        {
            return;
        }

        this.Insert(node.Value);
        this.PreOrderCopy(node.Left);
        this.PreOrderCopy(node.Right);
    }

    private Node Insert(T element, Node node)
    {
        if (node == null)
        {
            node = new Node(element);
        }
        else if (element.CompareTo(node.Value) < 0)
        {
            node.Left = this.Insert(element, node.Left);
        }
        else if (element.CompareTo(node.Value) > 0)
        {
            node.Right = this.Insert(element, node.Right);
        }

        return node;
    }

    private void Range(Node node, Queue<T> queue, T startRange, T endRange)
    {
        if (node == null)
        {
            return;
        }

        int nodeInLowerRange = startRange.CompareTo(node.Value);
        int nodeInHigherRange = endRange.CompareTo(node.Value);

        if (nodeInLowerRange < 0)
        {
            this.Range(node.Left, queue, startRange, endRange);
        }
        if (nodeInLowerRange <= 0 && nodeInHigherRange >= 0)
        {
            queue.Enqueue(node.Value);
        }
        if (nodeInHigherRange > 0)
        {
            this.Range(node.Right, queue, startRange, endRange);
        }
    }
    
    private void EachInOrder(Node node, Action<T> action)
    {
        if (node == null)
        {
            return;
        }

        this.EachInOrder(node.Left, action);
        action(node.Value);
        this.EachInOrder(node.Right, action);
    }

    private BinarySearchTree(Node node)
    {
        this.PreOrderCopy(node);
    }

    public BinarySearchTree()
    {
    }
    
    public void Insert(T element)
    {
        this.root = this.Insert(element, this.root);
    }
    
    public bool Contains(T element)
    {
        Node current = this.FindElement(element);

        return current != null;
    }

    public void EachInOrder(Action<T> action)
    {
        this.EachInOrder(this.root, action);
    }

    public BinarySearchTree<T> Search(T element)
    {
        Node current = this.FindElement(element);

        return new BinarySearchTree<T>(current);
    }

    public void DeleteMin()
    {
        if (this.root == null)
        {
            throw new InvalidOperationException();
        }

        Node current = this.root;
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

    public IEnumerable<T> Range(T startRange, T endRange)
    {
        Queue<T> queue = new Queue<T>();

        this.Range(this.root, queue, startRange, endRange);

        return queue;
    }

    public void Delete(T element)
    {
        if (!this.Contains(element))
        {
            throw new InvalidOperationException();
        }

        if (this.Count() == 1)
        {
            this.root = null;
            return;
        }

        Node parent = null;
        Node current = this.root;

        while (current.Value.CompareTo(element) != 0)
        {
            parent = current;

            var diff = current.Value.CompareTo(element);

            if (diff > 0)
            {
                current = current.Left;
            }
            else
            {
                current = current.Right;
            }
        }
       
        if (current.Right == null)
        {
            if (parent.Right.Value.CompareTo(current.Value) == 0)
            {
                parent.Right = current.Left;
            }
            else
            {
                parent.Left = current.Left;
            }
        }
        else if (current.Right.Left == null)
        {
            if (parent.Right.Value.CompareTo(current.Value) == 0)
            {
                parent.Right = current.Right;
                current.Right.Left = current.Left;
            }
            else
            {
                parent.Left = current.Right;
                current.Right.Left = current.Left;
            }
        }
        else
        {
            Node innerParent = null;
            Node innerCurrent = current.Right;
            while (innerCurrent.Left != null)
            {
                innerParent = innerCurrent;
                innerCurrent = innerCurrent.Left;
            }

            if (parent.Right.Value.CompareTo(current.Value) == 0)
            {
                parent.Right = innerCurrent;
                innerParent.Left = innerCurrent.Right;
                parent.Right.Right = current.Right;
                parent.Right.Left = current.Left;
            }
            else
            {
                parent.Left = innerCurrent;
                innerParent.Left = innerCurrent.Right;
                parent.Left.Right = current.Right;
                parent.Left.Left = current.Left;
            }
        }
    }

    public void DeleteMax()
    {
        if (this.root == null)
        {
            throw new InvalidOperationException();
        }

        Node parent = null;
        Node currendNode = this.root;

        while (currendNode.Right != null)
        {
            parent = currendNode;
            currendNode = currendNode.Right;
        }

        if (parent == null)
        {
            this.root = this.root.Right;
        }
        else
        {
            parent.Right = currendNode.Left;
        }
    }

    public int Count()
    {
        return this.Count(this.root);
    }

    private int Count(Node node, int sum = 1)
    {
        if (node == null)
        {
            return 0;
        }

        sum += this.Count(node.Left) + this.Count(node.Right);
        return sum;
    }

    public int Rank(T element)
    {
        return this.Rank(this.root, element);
    }

    private int Rank(Node root, T element)
    {
        if (root == null)
        {
            return 0;
        }

        var diff = root.Value.CompareTo(element);
        if (diff > 0)
        {
            return this.Rank(root.Left, element);
        }
        else if (diff < 0)
        {
            return 1 + this.Count(root.Left) + this.Rank(root.Right, element);
        }

        return this.Count(root.Left);
    }

    public T Select(int rank)
    {
        return this.Select(this.root, rank);
    }

    private T Select(Node node, int targetRank)
    {
        if (node == null)
        {
            throw new InvalidOperationException();
        }
        var rank = this.Rank(node.Value);
        var diffRank = targetRank.CompareTo(rank);

        if (diffRank < 0)
        {
            return this.Select(node.Left, targetRank);
        }
        else if (diffRank > 0)
        {
            return this.Select(node.Right, targetRank);
        }

        return node.Value;
    }

    public T Ceiling(T element)
    {
        CheckForEmptyRoot();
        return this.Select(this.Rank(element) + 1);
    }

    private void CheckForEmptyRoot()
    {
        if (this.root == null)
        {
            throw new InvalidOperationException();
        }
    }

    public T Floor(T element)
    {
        CheckForEmptyRoot();
        return this.Select(this.Rank(element) - 1);
    }

    private class Node
    {
        public Node(T value)
        {
            this.Value = value;
        }

        public T Value { get; }
        public Node Left { get; set; }
        public Node Right { get; set; }
    }
}

public class Launcher
{
    public static void Main(string[] args)
    {
        BinarySearchTree<int> bst = new BinarySearchTree<int>();

        //bst.Insert(10);
        //bst.Insert(5);
        //bst.Insert(3);
        //bst.Insert(1);
        //bst.Insert(4);
        //bst.Insert(8);
        //bst.Insert(7);
        //bst.Insert(9);
        //bst.Insert(37);
        //bst.Insert(39);
        //bst.Insert(45);

        Console.WriteLine(bst.Floor(2));

        //bst.EachInOrder(Console.WriteLine);
        //Console.WriteLine(bst.Count());
    }
}