using System;
using System.Collections.Generic;

public class BinaryHeap<T> where T : IComparable<T>
{
    private List<T> heap;

    public BinaryHeap()
    {
        this.heap = new List<T>();
    }

    public int Count
    {
        get
        {
            return this.heap.Count;
        }
    }

    public void Insert(T item)
    {
        this.heap.Add(item);
        this.HeapifyUp(this.heap.Count - 1);
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            var parentIndex = (index - 1) / 2;

            if (CompareElementWith(index, parentIndex))
            {
                this.SwapElements(index, parentIndex);
            }
            else
            {
                break;
            }

            index = parentIndex;
        }
    }

    private bool CompareElementWith(int index1, int index2)
    {
        return this.heap[index2].CompareTo(this.heap[index1]) < 0;
    }

    private void SwapElements(int a, int b)
    {
        var temp = this.heap[a];
        this.heap[a] = this.heap[b];
        this.heap[b] = temp;
    }

    public T Peek()
    {
        this.CheckEmptyHeap();
        return this.heap[0];
    }

    public T Pull()
    {
        this.CheckEmptyHeap();
        var removedElement = this.heap[0];
        this.SwapElements(0, this.Count - 1);
        this.heap.RemoveAt(this.Count - 1);
        this.HeapifyDown(0);
        return removedElement;
    }

    private void HeapifyDown(int index)
    {
        while (index < this.Count / 2)
        {
            var childIndex = index * 2 + 1;
            if (childIndex + 1 < this.Count && this.CompareElementWith(childIndex + 1, childIndex))
            {
                childIndex++;
            }

            if (this.CompareElementWith(childIndex, index))
            {
                this.SwapElements(index, childIndex);
            }
            else
            {
                break;
            }

            index = childIndex;
        }
    }

    private void CheckEmptyHeap()
    {
        if (this.Count == 0)
        {
            throw new InvalidOperationException();
        }
    }
}
