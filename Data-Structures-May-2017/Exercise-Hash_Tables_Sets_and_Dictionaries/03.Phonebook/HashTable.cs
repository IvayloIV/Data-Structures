using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HashTable<TKey, TValue> : IEnumerable<KeyValue<TKey, TValue>>
{
    private const int InitialCapacity = 4;
    private const double LoadFactor = 0.75;

    private int MaxCapacity => (int)(this.Capacity * LoadFactor);

    private LinkedList<KeyValue<TKey, TValue>>[] hashTable;

    public int Count { get; private set; }

    public int Capacity
    {
        get
        {
            return this.hashTable.Length;
        }
    }

    public HashTable()
    {
        this.hashTable = new LinkedList<KeyValue<TKey, TValue>>[InitialCapacity];
    }

    public HashTable(int capacity)
    {
        this.hashTable = new LinkedList<KeyValue<TKey, TValue>>[capacity];
    }

    public void Add(TKey key, TValue value)
    {
        this.TryToGrow();    
        var hash = Math.Abs(key.GetHashCode()) % this.Capacity;

        if (this.hashTable[hash] == null)
        {
            this.hashTable[hash] = new LinkedList<KeyValue<TKey, TValue>>();
        }

        foreach (var node in this.hashTable[hash])
        {
            if (node.Key.Equals(key))
            {
                throw new ArgumentException();
            }
        }

        this.hashTable[hash].AddLast(new KeyValue<TKey, TValue>(key, value));
        this.Count++;
    }

    private void TryToGrow()
    {
        if (this.Count < this.MaxCapacity)
        {
            return;
        }

        HashTable<TKey, TValue> newHashTable = new HashTable<TKey, TValue>(this.Capacity * 2);

        foreach (var item in this)
        {
            newHashTable.Add(item.Key, item.Value);
        }

        this.hashTable = newHashTable.hashTable;
    }

    public bool AddOrReplace(TKey key, TValue value)
    {
        var node = this.Find(key);

        if (node == null)
        {
            this.Add(key, value);
        }
        else
        {
            node.Value = value;
        }
        return true;
    }

    public TValue Get(TKey key)
    {
        var node = this.Find(key);
        if (node == null)
        {
            throw new KeyNotFoundException();
        }

        return node.Value;
    }

    public TValue this[TKey key]
    {
        get
        {
            return this.Get(key);
        }
        set
        {
            this.AddOrReplace(key, value);
        }
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        var item = this.Find(key);

        value = default(TValue);
        if (item == null)
        {
            return false;
        }
        value = item.Value;
        return true;
    }

    public KeyValue<TKey, TValue> Find(TKey key)
    {
        var hash = Math.Abs(key.GetHashCode()) % this.Capacity;
        var node = this.hashTable[hash];

        if (node != null)
        {
            foreach (var item in node)
            {
                if (item.Key.Equals(key))
                {
                    return item;
                }
            }
        }

        return null;
    }

    public bool ContainsKey(TKey key)
    {
        return this.Find(key) != null;
    }

    public bool Remove(TKey key)
    {
        var hash = Math.Abs(key.GetHashCode()) % this.Capacity;
        var node = this.hashTable[hash];
        var item = this.Find(key);

        if (item == null)
        {
            return false;
        }

        node.Remove(item);
        this.Count--;
        return true;
    }

    public void Clear()
    {
        this.hashTable = new LinkedList<KeyValue<TKey, TValue>>[InitialCapacity];
        this.Count = 0;
    }

    public IEnumerable<TKey> Keys => this.Select(a => a.Key);

    public IEnumerable<TValue> Values => this.Select(a => a.Value);

    public IEnumerator<KeyValue<TKey, TValue>> GetEnumerator()
    {
        foreach (var node in this.hashTable)
        {
            if (node != null)
            {
                foreach (var item in node)
                {
                    yield return item;
                }
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
