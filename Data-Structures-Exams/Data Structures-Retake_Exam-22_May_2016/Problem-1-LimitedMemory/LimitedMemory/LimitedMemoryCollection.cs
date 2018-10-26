using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace LimitedMemory
{
    public class LimitedMemoryCollection<K, V> : ILimitedMemoryCollection<K, V>
    {
        private Dictionary<K, LinkedListNode<Pair<K, V>>> elements;
        private LinkedList<Pair<K, V>> linkedList;

        public LimitedMemoryCollection(int capacity)
        {
            this.elements = new Dictionary<K, LinkedListNode<Pair<K, V>>>();
            this.linkedList = new LinkedList<Pair<K, V>>();
            this.Capacity = capacity;
        } 

        public IEnumerator<Pair<K, V>> GetEnumerator()
        {
            foreach (var element in this.linkedList)
            {
                yield return element;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int Capacity { get; private set; }

        public int Count { get; private set; }

        public void Set(K key, V value)
        {
            if (this.elements.ContainsKey(key))
            {
                this.ChangeValue(key, value);
            }
            else
            {
                if (this.Count >= this.Capacity)
                {
                    this.RemoveLastElement();
                }
                this.AddNewElement(key, value);
            }
        }

        private void AddNewElement(K key, V value)
        {
            this.linkedList.AddFirst(new Pair<K, V>(key, value));
            this.elements[key] = this.linkedList.First;
            this.Count++;
        }

        private void RemoveLastElement()
        {
            var lastPair = this.linkedList.Last;
            this.linkedList.RemoveLast();
            this.elements.Remove(lastPair.Value.Key);
            this.Count--;
        }

        private void ChangeValue(K key, V value)
        {
            var el = this.elements[key];
            this.linkedList.Remove(el);
            el.Value.Value = value;
            this.linkedList.AddFirst(el);
        }

        public V Get(K key)
        {
            if (!this.elements.ContainsKey(key))
            {
                throw new KeyNotFoundException();
            }

            var el = this.elements[key];
            this.linkedList.Remove(el);
            this.linkedList.AddFirst(el);
            return el.Value.Value;
        }
    }
}
