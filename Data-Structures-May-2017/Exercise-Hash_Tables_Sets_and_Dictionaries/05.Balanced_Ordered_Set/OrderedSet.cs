using System;
using System.Collections;
using System.Collections.Generic;

public class OrderedSet<T> : IEnumerable<T> where T : IComparable<T>
{
    private AVL<T> tree;

    public OrderedSet()
    {
        this.tree = new AVL<T>();
    }

    public void Add(T element)
    {
        this.tree.Add(element);
    }

    public bool Contains(T element)
    {
        return this.tree.Contains(element);
    }

    public void Remove(T element)
    {
        this.tree.Remove(element);
    }

    public int Count => this.tree.Count;

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var item in this.tree.EachInOrder())
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}