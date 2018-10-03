using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class Hierarchy<T> : IHierarchy<T>
{
    private Node<T> root;
    private Dictionary<T, Node<T>> elements;

    public Hierarchy(T root)
    {
        this.root = new Node<T>(root);
        this.elements = new Dictionary<T, Node<T>>();
        this.elements.Add(root, this.root);
    }

    public int Count
    {
        get
        {
            return this.elements.Count;
        }
    }

    public void Add(T element, T child)
    {
        if (!this.elements.ContainsKey(element))
        {
            throw new ArgumentException();
        }

        if (this.elements.ContainsKey(child))
        {
            throw new ArgumentException();
        }
        var currentParent = this.elements[element];
        var newNode = new Node<T>(child, currentParent);
        currentParent.AddChild(newNode);
        this.elements.Add(child, newNode);
    }

    public void Remove(T element)
    {
        if (!this.elements.ContainsKey(element))
        {
            throw new ArgumentException();
        }

        if (this.elements[element].Parent == null)
        {
            throw new InvalidOperationException();
        }

        var nodeElement = this.elements[element];
        foreach (var child in nodeElement.Children)
        {
            nodeElement.Parent.AddChild(child);
            child.ChangeParent(nodeElement.Parent);
        }
        nodeElement.Parent.RemoveChild(nodeElement);
        this.elements.Remove(nodeElement.Value);
    }

    public IEnumerable<T> GetChildren(T item)
    {
        if (!this.elements.ContainsKey(item))
        {
            throw new ArgumentException();
        }

        var childrens = this.elements[item].Children;
        return childrens.Select(a => a.Value).ToList();
    }

    public T GetParent(T item)
    {
        if (!this.elements.ContainsKey(item))
        {
            throw new ArgumentException();
        }

        if (this.elements[item].Parent == null)
        {
            return default(T);
        }

        Node<T> element = this.elements[item];
        return element.Parent.Value;
    }

    public bool Contains(T value)
    {
        return this.elements.ContainsKey(value);
    }

    public IEnumerable<T> GetCommonElements(Hierarchy<T> other)
    {
        var list = new Stack<T>();
        FindEqualElements(list, other.root);
        return list;
    }

    private void FindEqualElements(Stack<T> list, Node<T> node)
    {
        foreach (var child in node.Children)
        {
            this.FindEqualElements(list, child);
        }

        if (this.elements.ContainsKey(node.Value))
        {
            list.Push(node.Value);
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        var queue = new Queue<Node<T>>();
        queue.Enqueue(this.root);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            foreach (var child in current.Children)
            {
                queue.Enqueue(child);
            }
            yield return current.Value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    private class Node<T>
    {
        public T Value { get; private set; }

        public Node<T> Parent { get; private set; }

        public List<Node<T>> Children { get; private set; }

        public Node(T value)
        {
            this.Value = value;
            this.Children = new List<Node<T>>();
        }

        public Node(T value, Node<T> parent) : this(value)
        {
            this.Parent = parent;
        }

        public void AddChild(Node<T> child)
        {
            this.Children.Add(child);
        }

        public void ChangeParent(Node<T> parent)
        {
            this.Parent = parent;
        }

        public void RemoveChild(Node<T> child)
        {
            this.Children.Remove(child);
        }
    }
}