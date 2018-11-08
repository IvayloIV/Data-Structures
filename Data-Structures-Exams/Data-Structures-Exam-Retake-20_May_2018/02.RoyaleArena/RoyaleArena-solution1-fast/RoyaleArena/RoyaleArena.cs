using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

public class RoyaleArena : IArena
{
    private Dictionary<int, Battlecard> byId;
    private Dictionary<string, HashSet<Battlecard>> byName;
    private HashSet<Battlecard> battlecards;

    public RoyaleArena()
    {
        this.byId = new Dictionary<int, Battlecard>();
        this.byName = new Dictionary<string, HashSet<Battlecard>>();
        this.battlecards = new HashSet<Battlecard>();
    }

    public int Count => this.byId.Count;

    public void Add(Battlecard card)
    {
        this.byId[card.Id] = card;
        this.AddByName(card);
        this.battlecards.Add(card);
    }

    public void ChangeCardType(int id, CardType type)
    {
        if (!this.byId.ContainsKey(id))
        {
            throw new ArgumentException();
        }

        this.byId[id].Type = type;
    }

    public bool Contains(Battlecard card)
    {
        return this.byId.ContainsKey(card.Id);
    }

    public IEnumerable<Battlecard> FindFirstLeastSwag(int n)
    {
        if (n < 0 || n > this.Count)
        {
            throw new InvalidOperationException();
        }

        return this.battlecards.OrderBy(a => a.Swag).ThenBy(a => a.Id).Take(n);
    }

    public IEnumerable<Battlecard> GetAllByNameAndSwag()
    {
        var result = new LinkedList<Battlecard>();

        foreach (var kvp in this.byName)
        {
            if (kvp.Value.Count == 0)
            {
                throw new InvalidOperationException();
            }

            Battlecard maxCard = null;
            foreach (var card in kvp.Value)
            {
                if (maxCard == null || maxCard.Swag < card.Swag)
                {
                    maxCard = card;
                }
            }
            result.AddLast(maxCard);
        }

        return result;
    }

    public IEnumerable<Battlecard> GetAllInSwagRange(double lo, double hi)
    {
        return this.battlecards
            .Where(a => a.Swag >= lo && a.Swag <= hi)
            .OrderBy(a => a.Swag);
    }

    public IEnumerable<Battlecard> GetByCardType(CardType type)
    {
        var result = this.byId
                .Values
                .Where(a => a.Type == type)
                .OrderByDescending(a => a.Damage)
                .ThenBy(a => a.Id);

        this.CheckForEmptyCollection(result);
        return result;
    }

    public IEnumerable<Battlecard> GetByCardTypeAndMaximumDamage(CardType type, double damage)
    {
        var result = this.byId
                .Values
                .Where(a => a.Type == type && a.Damage <= damage)
                .OrderByDescending(a => a.Damage)
                .ThenBy(a => a.Id);

        this.CheckForEmptyCollection(result);
        return result;
    }

    public Battlecard GetById(int id)
    {
        if (!this.byId.ContainsKey(id))
        {
            throw new InvalidOperationException();
        }

        return this.byId[id];
    }

    public IEnumerable<Battlecard> GetByNameAndSwagRange(string name, double lo, double hi)
    {
        this.CheckExistName(name);
        return this.byName[name]
            .Where(a => a.Swag >= lo && a.Swag < hi)
            .OrderByDescending(a => a.Swag)
            .ThenBy(a => a.Id);
    }

    public IEnumerable<Battlecard> GetByNameOrderedBySwagDescending(string name)
    {
        this.CheckExistName(name);
        return this.byName[name].OrderByDescending(a => a.Swag).ThenBy(a => a.Id);
    }

    public IEnumerable<Battlecard> GetByTypeAndDamageRangeOrderedByDamageThenById(CardType type, int lo, int hi)
    {
        var result = this.byId
            .Values
            .Where(a => a.Type == type && a.Damage > lo && a.Damage <= hi)
            .OrderByDescending(a => a.Damage)
            .ThenBy(a => a.Id);

        this.CheckForEmptyCollection(result);
        return result;
    }

    public IEnumerator<Battlecard> GetEnumerator()
    {
        foreach (var battlecard in this.battlecards)
        {
            yield return battlecard;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    public void RemoveById(int id)
    {
        if (!this.byId.ContainsKey(id))
        {
            throw new InvalidOperationException();
        }

        var card = this.byId[id];
        this.byId.Remove(id);
        this.byName[card.Name].Remove(card);
        this.battlecards.Remove(card);
    }

    private void AddByName(Battlecard card)
    {
        if (!this.byName.ContainsKey(card.Name))
        {
            this.byName[card.Name] = new HashSet<Battlecard>();
        }

        this.byName[card.Name].Add(card);
    }

    private void CheckExistName(string name)
    {
        if (!this.byName.ContainsKey(name) || this.byName[name].Count == 0)
        {
            throw new InvalidOperationException();
        }
    }

    private void CheckForEmptyCollection(IOrderedEnumerable<Battlecard> result)
    {
        if (result.Count() == 0)
        {
            throw new InvalidOperationException();
        }
    }
}