using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

public class Organization : IOrganization
{
    private HashSet<Person> people;
    private List<Person> byIndex;
    private Dictionary<string, LinkedList<Person>> byName;
    private OrderedDictionary<int, LinkedList<Person>> byNameLength;

    public Organization()
    {
        this.byIndex = new List<Person>();
        this.people = new HashSet<Person>();
        this.byName = new Dictionary<string, LinkedList<Person>>();
        this.byNameLength = new OrderedDictionary<int, LinkedList<Person>>();
    }

    public IEnumerator<Person> GetEnumerator()
    {
        foreach (var person in this.byIndex)
        {
            yield return person;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    public int Count { get { return this.people.Count; } }
    public bool Contains(Person person)
    {
        return this.people.Contains(person);
    }

    public bool ContainsByName(string name)
    {
        return this.byName.ContainsKey(name);
    }

    public void Add(Person person)
    {
        this.byIndex.Add(person);
        this.people.Add(person);
        this.AddByName(person);
        this.AddByNameLength(person);
    }

    public Person GetAtIndex(int index)
    {
        if (index < 0 || index > this.byIndex.Count - 1)
        {
            throw new IndexOutOfRangeException();
        }

        return this.byIndex[index];
    }

    public IEnumerable<Person> GetByName(string name)
    {
        if (!this.byName.ContainsKey(name))
        {
            return Enumerable.Empty<Person>();
        }
        return this.byName[name];
    }

    public IEnumerable<Person> FirstByInsertOrder(int count = 1)
    {
        for (int i = 0; i < Math.Min(count, this.byIndex.Count); i++)
        {
            yield return this.byIndex[i];
        }
    }

    public IEnumerable<Person> SearchWithNameSize(int minLength, int maxLength)
    {
        var range = this.byNameLength.Range(minLength, true, maxLength, true);

        foreach (var kvp in range)
        {
            foreach (var person in kvp.Value)
            {
                yield return person;
            }
        }
    }

    public IEnumerable<Person> GetWithNameSize(int length)
    {
        if (!this.byNameLength.ContainsKey(length))
        {
            throw new ArgumentException();
        }

        return this.byNameLength[length];
    }

    public IEnumerable<Person> PeopleByInsertOrder()
    {
        return this.byIndex;
    }

    private void AddByName(Person person)
    {
        if (!this.byName.ContainsKey(person.Name))
        {
            this.byName[person.Name] = new LinkedList<Person>();
        }
        this.byName[person.Name].AddLast(person);
    }

    private void AddByNameLength(Person person)
    {
        if (!this.byNameLength.ContainsKey(person.Name.Length))
        {
            this.byNameLength[person.Name.Length] = new LinkedList<Person>();
        }
        this.byNameLength[person.Name.Length].AddLast(person);
    }
}