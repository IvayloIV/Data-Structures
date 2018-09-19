using System;
using System.Collections;
using System.Collections.Generic;

public class ReversedList<T> : IEnumerable<T>
{
    public ReversedList()
    {
        this.data = new T[2];
    }

    private T[] data;

    public int Capacity => this.data.Length;

    public int Count;

    public T this[int index]
    {
        get
        {
            ValidateIndex(index);
            return this.data[this.Count - 1 - index];
        }
        set { this.data[this.Count - 1 - index] = value; }
    }

    private void ValidateIndex(int index)
    {
        if (index < 0 || index > this.Count - 1)
        {
            throw new IndexOutOfRangeException();
        }
    }

    public void Add(T item)
    {
        if (this.Count >= this.data.Length)
        {
            this.ResizeArray();
        }
        this.data[this.Count++] = item;
    }

    public T RemoveAt(int index)
    {
        this.ValidateIndex(index);
        if (this.Count == 0)
        {
            throw new ArgumentOutOfRangeException();
        }
        var element = this[index];
        this.Count--;
        for (int arrIndex = this.Count - index; arrIndex < this.Count; arrIndex++)
        {
            this.data[arrIndex] = this.data[arrIndex + 1];
        }
        return element;
    }

    private void ResizeArray()
    {
        var newArray = new T[this.data.Length * 2];
        Array.Copy(this.data, newArray, this.Count);
        this.data = newArray;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = this.Count - 1; i >= 0; i--)
        {
            yield return this.data[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}