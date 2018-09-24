using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Tree<T>
{
    public T Value { get; private set; }

    public Tree<T> Parent { get; private set; }

    public List<Tree<T>> Children { get; private set; }

    public Tree(T value, params Tree<T>[] children)
    {
        this.Value = value;
        this.Children = new List<Tree<T>>();
        foreach (var child in children)
        {
            this.Children.Add(child);
            child.Parent = this;
        }
    }

    public void AddNewChild(Tree<T> child)
    {
        this.Children.Add(child);
    }

    public void SetParent(Tree<T> parent)
    {
        this.Parent = parent;
    }

    public void PrintTree(int indented = 0)
    {
        Console.WriteLine(new string(' ', indented * 2) + this.Value);

        foreach (var child in this.Children)
        {
            child.PrintTree(indented + 1);
        }
    }
}