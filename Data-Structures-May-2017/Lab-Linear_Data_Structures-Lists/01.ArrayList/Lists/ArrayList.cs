using System;

public class ArrayList<T>
{
    public ArrayList()
    {
        this.data = new T[2];
    }

    private T[] data;

    public int Count { get; set; }

    public T this[int index]
    {
        get
        {
            this.ValidateIndex(index);
            return this.data[index];
        }

        set
        {
            this.ValidateIndex(index);
            this.data[index] = value;
        }
    }

    private void ValidateIndex(int index)
    {
        if (index < 0 || index > this.Count - 1)
        {
            throw new ArgumentOutOfRangeException();
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

    private void ResizeArray()
    {
        var newArray = new T[this.data.Length * 2];
        Array.Copy(this.data, newArray, this.Count);
        this.data = newArray;
    }

    public T RemoveAt(int index)
    {
        this.ValidateIndex(index);

        var element = this.data[index];
        for (int i = index; i < this.Count - 1; i++)
        {
            this.data[i] = this.data[i + 1];
        }

        this.Count--;

        if (this.Count <= this.data.Length / 4)
        {
            this.ShrinkArray();
        }

        return element;
    }

    private void ShrinkArray()
    {
        var newArray = new T[this.data.Length / 2];
        Array.Copy(this.data, newArray, this.Count);
        this.data = newArray;
    }
}