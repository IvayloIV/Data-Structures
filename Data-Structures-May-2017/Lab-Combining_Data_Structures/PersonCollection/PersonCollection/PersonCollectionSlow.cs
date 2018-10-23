using System;
using System.Collections.Generic;
using System.Linq;

public class PersonCollectionSlow : IPersonCollection
{
    List<Person> people;

    public PersonCollectionSlow()
    {
        this.people = new List<Person>();
    }

    public bool AddPerson(string email, string name, int age, string town)
    {
        if (this.FindPerson(email) != null)
        {
            return false;
        }
        this.people.Add(new Person(email, name, age, town));
        return true;
    }

    public int Count
    {
        get
        {
            return this.people.Count;
        }
    }

    public Person FindPerson(string email)
    {
        return this.people.FirstOrDefault(a => a.Email == email);
    }

    public bool DeletePerson(string email)
    {
        var person = this.FindPerson(email);
        if (person == null)
        {
            return false;
        }

        this.people.Remove(person);
        return true;
    }

    public IEnumerable<Person> FindPersons(string emailDomain)
    {
        return this.people.Where(a => a.Email.Substring(a.Email.IndexOf('@') + 1) == emailDomain)
            .OrderBy(a => a.Email);
    }

    public IEnumerable<Person> FindPersons(string name, string town)
    {
        return this.people.Where(a => a.Name == name && a.Town == town)
            .OrderBy(a => a.Email);
    }

    public IEnumerable<Person> FindPersons(int startAge, int endAge)
    {
        return this.people.Where(a => a.Age >= startAge && a.Age <= endAge)
            .OrderBy(a => a.Age)
            .ThenBy(a => a.Email);
    }

    public IEnumerable<Person> FindPersons(int startAge, int endAge, string town)
    {
        return this.people.Where(a => a.Age >= startAge && a.Age <= endAge && a.Town == town)
            .OrderBy(a => a.Age)
            .ThenBy(a => a.Email);
    }
}
