using System;
using System.Collections.Generic;
using Wintellect.PowerCollections;

public class FirstLastList<T> : IFirstLastList<T> where T : IComparable<T>
{
    private Node<T> Head;
    private Node<T> Tail;
    private int count;
    private OrderedBag<T> bag;
    private OrderedBag<Node<T>> bagReversed;

    public FirstLastList()
    {
        this.count = 0;
        this.bag = new OrderedBag<T>();
        this.bagReversed = new OrderedBag<Node<T>>((a, b) => b.Value.CompareTo(a.Value));
    }

    public int Count
    {
        get
        {
            return this.count;
        }
    }

    public void Add(T element)
    {
        var newNode = new Node<T>(element);
        if (this.Count == 0)
        {
            this.Head = this.Tail = newNode;
        }
        else
        {
            newNode.Next = this.Head;
            this.Head.Prev = newNode;
            this.Head = newNode;
        }
        bag.Add(element);
        bagReversed.Add(newNode);
        this.count++;
    }

    public void Clear()
    {
        this.Head = this.Tail = null;
        this.count = 0;
        this.bag.Clear();
        this.bagReversed.Clear();
    }

    public IEnumerable<T> First(int count)
    {
        this.ValidateCount(count);
        var copyTail = this.Tail;
        while (count > 0)
        {
            yield return copyTail.Value;
            copyTail = copyTail.Prev;
            count--;
        }
    }

    private void ValidateCount(int count)
    {
        if (this.count < count)
        {
            throw new ArgumentOutOfRangeException();
        }
    }

    public IEnumerable<T> Last(int count)
    {
        this.ValidateCount(count);
        var copyHead = this.Head;
        while (count > 0)
        {
            yield return copyHead.Value;
            copyHead = copyHead.Next;
            count--;
        }
    }

    public IEnumerable<T> Max(int count)
    {
        this.ValidateCount(count);
        for (int i = 0; i < count; i++)
        {
            yield return this.bagReversed[i].Value;
        }
    }

    public IEnumerable<T> Min(int count)
    {
        this.ValidateCount(count);

        var currentIndex = 0;
        while (count > currentIndex)
        {
            yield return bag[currentIndex++];
        }
    }

    public int RemoveAll(T element)
    {
        var node = new Node<T>(element);
        var items = bagReversed.Range(node, true, node, true);
        foreach (var item in items)
        {
            if (this.count == 1)
            {
                this.Head = this.Tail = null;
            }
            else
            {
                if (item.Prev == null)
                {
                    this.Head = this.Head.Next;
                    this.Head.Prev = null;
                }
                else if (item.Next == null)
                {
                    this.Tail = this.Tail.Prev;
                    this.Tail.Next = null;
                }
                else
                {
                    item.Prev.Next = item.Next;
                    item.Next.Prev = item.Prev;
                }
            }
            this.count--;
        }

        this.bag.RemoveAllCopies(element);
        var count = this.bagReversed.RemoveAllCopies(node);
        return count;
    }
}