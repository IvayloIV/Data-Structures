using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Dictionary<TKey, TValue> : IEnumerable<KeyValue<TKey, TValue>>
{
    private HashTable<TKey, TValue> hashTable;

    public Dictionary()
    {
        this.hashTable = new HashTable<TKey, TValue>();
    }

    public void Add(TKey key, TValue value)
    {
        this.hashTable.AddOrReplace(key, value);
    }

    public bool ContainsKey(TKey key)
    {
        return hashTable.ContainsKey(key);
    }

    public IEnumerator<KeyValue<TKey, TValue>> GetEnumerator()
    {
        foreach (var item in this.hashTable)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    public TValue this[TKey key]
    {
        get 
        {
            return this.hashTable.Get(key);
        }

        set 
        {
            this.hashTable[key] = value;
        }
    }
}