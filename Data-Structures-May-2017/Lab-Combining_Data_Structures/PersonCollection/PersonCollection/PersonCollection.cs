using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

public class PersonCollection : IPersonCollection
{
    Dictionary<string, Person> byEmail;
    Dictionary<string, SortedSet<Person>> byDomain;
    Dictionary<string, SortedSet<Person>> byNameAndTown;
    OrderedDictionary<int, SortedSet<Person>> byAge;
    Dictionary<string, OrderedDictionary<int, SortedSet<Person>>> byTownAndAge;

    public PersonCollection()
    {
        this.byEmail = new Dictionary<string, Person>();
        this.byDomain = new Dictionary<string, SortedSet<Person>>();
        this.byNameAndTown = new Dictionary<string, SortedSet<Person>>();
        this.byAge = new OrderedDictionary<int, SortedSet<Person>>();
        this.byTownAndAge = new Dictionary<string, OrderedDictionary<int, SortedSet<Person>>>();
    }

    public bool AddPerson(string email, string name, int age, string town)
    {
        if (this.FindPerson(email) != null)
        {
            return false;
        }

        var newPerson = new Person(email, name, age, town);
        this.byEmail[email] = newPerson;
        this.AddInDomainCollection(email, newPerson);
        this.AddByNameAndTown(name, town, newPerson);
        this.AddByAge(age, newPerson);
        this.AddByTownAndName(age, town, newPerson);

        return true;
    }

    public int Count
    {
        get
        {
            return this.byEmail.Count;
        }
    }

    public Person FindPerson(string email)
    {
        if (!this.byEmail.ContainsKey(email))
        {
            return null;
        }
        return this.byEmail[email];
    }

    public bool DeletePerson(string email)
    {
        var person = this.FindPerson(email);
        if (person == null)
        {
            return false;
        }

        this.byEmail.Remove(email);
        this.byDomain[this.GetDomainByEmail(person.Email)].Remove(person);
        this.byNameAndTown[this.GetNameAndTown(person.Name, person.Town)].Remove(person);
        this.byAge[person.Age].Remove(person);
        this.byTownAndAge[person.Town][person.Age].Remove(person);
        return true;
    }

    public IEnumerable<Person> FindPersons(string emailDomain)
    {
        if (!this.byDomain.ContainsKey(emailDomain))
        {
            return Enumerable.Empty<Person>();
        }

        return this.byDomain[emailDomain];
    }

    public IEnumerable<Person> FindPersons(string name, string town)
    {
        var nameAndTown = this.GetNameAndTown(name, town);
        if (!this.byNameAndTown.ContainsKey(nameAndTown))
        {
            return Enumerable.Empty<Person>();
        }

        return this.byNameAndTown[nameAndTown];
    }

    public IEnumerable<Person> FindPersons(int startAge, int endAge)
    {
        var range = this.byAge.Range(startAge, true, endAge, true);

        foreach (var kvp in range)
        {
            foreach (var person in kvp.Value)
            {
                yield return person;
            }
        }
    }

    public IEnumerable<Person> FindPersons(int startAge, int endAge, string town)
    {
        if (!this.byTownAndAge.ContainsKey(town))
        {
            return Enumerable.Empty<Person>();
        }

        var range = this.byTownAndAge[town].Range(startAge, true, endAge, true);
        var result = new List<Person>();

        foreach (var kvp in range)
        {
            foreach (var person in kvp.Value)
            {
                result.Add(person);
            }
        }

        return result;
    }

    private void AddInDomainCollection(string email, Person newPerson)
    {
        var domain = this.GetDomainByEmail(email);
        if (!this.byDomain.ContainsKey(domain))
        {
            this.byDomain[domain] = new SortedSet<Person>();
        }
        this.byDomain[domain].Add(newPerson);
    }

    private void AddByNameAndTown(string name, string town, Person newPerson)
    {
        var nameAndTown = this.GetNameAndTown(name, town);
        if (!this.byNameAndTown.ContainsKey(nameAndTown))
        {
            this.byNameAndTown[nameAndTown] = new SortedSet<Person>();
        }

        this.byNameAndTown[nameAndTown].Add(newPerson);
    }

    private void AddByAge(int age, Person newPerson)
    {
        if (!this.byAge.ContainsKey(age))
        {
            this.byAge[age] = new SortedSet<Person>();
        }

        this.byAge[age].Add(newPerson);
    }

    private void AddByTownAndName(int age, string town, Person newPerson)
    {
        if (!this.byTownAndAge.ContainsKey(town))
        {
            this.byTownAndAge[town] = new OrderedDictionary<int, SortedSet<Person>>();
        }

        if (!this.byTownAndAge[town].ContainsKey(age))
        {
            this.byTownAndAge[town][age] = new SortedSet<Person>();
        }

        this.byTownAndAge[town][age].Add(newPerson);
    }

    private string GetDomainByEmail(string email)
    {
        return email.Substring(email.IndexOf('@') + 1);
    }

    private string GetNameAndTown(string name, string town)
    {
        return name + town;
    }
}
