﻿using System;

public class KdTree
{
    private Node root;

    public class Node
    {
        public Node(Point2D point)
        {
            this.Point = point;
        }

        public Point2D Point { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
    }

    public Node Root
    {
        get
        {
            return this.root;
        }
    }

    public bool Contains(Point2D point)
    {
        var node = GetNode(this.root, point, 0);
        return node != null;
    }

    private Node GetNode(Node node, Point2D point, int depth)
    {
        if (node == null) 
        {
            return null;
        }

        var cmp = this.Compare(node.Point, point, depth);

        if (cmp > 0)
        {
            return this.GetNode(node.Left, point, depth + 1);
        }
        else if (cmp < 0)
        {
            return this.GetNode(node.Right, point, depth + 1);
        }

        return node;
    }

    public void Insert(Point2D point)
    {
        this.root = this.Insert(this.root, point, 0);
    }

    private Node Insert(Node node, Point2D point, int depth)
    {
        if (node == null)
        {
            return new Node(point);
        }

        var cmp = this.Compare(node.Point, point, depth);

        if (cmp > 0)
        {
            node.Left = this.Insert(node.Left, point, depth + 1);
        }
        else if (cmp < 0) 
        {
            node.Right = this.Insert(node.Right, point, depth + 1);
        }

        return node;
    }

    private int Compare(Point2D nodePoint, Point2D point, int depth)
    {
        int cmp = 0;
        if (depth % 2 == 0)
        {
            cmp = nodePoint.X.CompareTo(point.X);
            if (cmp == 0)
            {
                cmp = nodePoint.Y.CompareTo(point.Y);
            }
        }
        else if (depth % 2 == 1)
        {
            cmp = nodePoint.Y.CompareTo(point.Y);
            if (cmp == 0)
            {
                cmp = nodePoint.X.CompareTo(point.X);
            }
        }

        return cmp;
    }

    public void EachInOrder(Action<Point2D> action)
    {
        this.EachInOrder(this.root, action);
    }

    private void EachInOrder(Node node, Action<Point2D> action)
    {
        if (node == null)
        {
            return;
        }

        this.EachInOrder(node.Left, action);
        action(node.Point);
        this.EachInOrder(node.Right, action);
    }
}
