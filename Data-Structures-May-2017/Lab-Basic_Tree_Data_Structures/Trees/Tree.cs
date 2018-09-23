using System;
using System.Collections.Generic;

public class Tree<T>
{
    private T value;

    private List<Tree<T>> children;

    public Tree(T value, params Tree<T>[] children)
    {
        this.value = value;
        this.children = new List<Tree<T>>(children);
    }

    public void Print(int indent = 0)
    {
        Console.WriteLine(new string(' ', indent) + this.value);

        foreach (var child in this.children)
        {
            child.Print(indent + 2);
        }
    }

    public void Each(Action<T> action)
    {
        action(this.value);

        foreach (var child in this.children)
        {
            child.Each(action);
        }
    }

    public IEnumerable<T> OrderDFS()
    {
        var elements = new List<T>();

        this.DFS(this, elements);

        return elements;
    }

    private void DFS(Tree<T> tree, List<T> elements)
    {
        foreach (var child in tree.children)
        {
            this.DFS(child, elements);
        }

        elements.Add(tree.value);
    }

    public IEnumerable<T> OrderBFS()
    {
        var elements = new List<T>();
        var queue = new Queue<Tree<T>>();
        queue.Enqueue(this);

        while (queue.Count > 0)
        {
            var currentTree = queue.Dequeue();

            foreach (var child in currentTree.children)
            {
                queue.Enqueue(child);
            }

            elements.Add(currentTree.value);
        }

        return elements;
    }
}