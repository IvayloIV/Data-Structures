using System;
using System.Collections.Generic;

public class IntervalTree
{
    private class Node
    {
        internal Interval interval;
        internal double max;
        internal Node right;
        internal Node left;

        public Node(Interval interval)
        {
            this.interval = interval;
            this.max = interval.Hi;
        }
    }

    private Node root;

    public void Insert(double lo, double hi)
    {
        this.root = this.Insert(this.root, lo, hi);
    }

    public void EachInOrder(Action<Interval> action)
    {
        EachInOrder(this.root, action);
    }

    public Interval SearchAny(double lo, double hi)
    {
        var node = this.root;

        while (node != null && !(node.interval.Intersects(lo, hi)))
        {
            if (node.left != null && node.left.max > lo)
            {
                node = node.left;
            }
            else
            {
                node = node.right;
            }
        }

        if (node == null)
        {
            return null;
        }
        return node.interval;
    }

    public IEnumerable<Interval> SearchAll(double lo, double hi)
    {
        var result = new List<Interval>();
        this.SearchAll(this.root, result, lo, hi);
        return result;
    }

    private void SearchAll(Node node, List<Interval> result, double lo, double hi)
    {
        if (node == null)
        {
            return;
        }

        if (node.left != null && node.left.max > lo)
        {
            this.SearchAll(node.left, result, lo, hi);
        }

        if (node.interval.Intersects(lo, hi))
        {
            result.Add(node.interval);
        }

        if (node.right != null && node.right.interval.Lo < hi)
        {
            this.SearchAll(node.right, result, lo, hi);
        }
    }

    private void EachInOrder(Node node, Action<Interval> action)
    {
        if (node == null)
        {
            return;
        }

        EachInOrder(node.left, action);
        action(node.interval);
        EachInOrder(node.right, action);
    }

    private Node Insert(Node node, double lo, double hi)
    {
        if (node == null)
        {
            return new Node(new Interval(lo, hi));
        }

        int cmp = lo.CompareTo(node.interval.Lo);
        if (cmp < 0)
        {
            node.left = Insert(node.left, lo, hi);
        }
        else if (cmp > 0)
        {
            node.right = Insert(node.right, lo, hi);
        }

        this.UpdateMax(node);
        return node;
    }

    private void UpdateMax(Node node)
    {
        var maxChild = this.GetMax(node.left, node.right);
        node.max = this.GetMax(node, maxChild).max;
    }

    private Node GetMax(Node left, Node right)
    {
        if (left == null)
        {
            return right;
        }

        if (right == null)
        {
            return left;
        }

        return left.max > right.max ? left : right;
    }
}
