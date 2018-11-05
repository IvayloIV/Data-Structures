using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

public class Computer : IComputer
{
    private HashSet<Invader> invaders;

    public Computer(int energy)
    {
        this.invaders = new HashSet<Invader>();
        this.Energy = energy;
    }

    private int energy;

    public int Energy
    {
        get
        {
            if (this.energy < 0)
            {
                return 0;
            }
            return this.energy;
        }

        set 
        {
            if (value < 0)
            {
                throw new ArgumentException();
            }
            this.energy = value;
        }
    }

    public void Skip(int turns)
    {
        var invadersToDelete = new List<Invader>();
        foreach (var invader in this.invaders)
        {
            invader.Distance -= turns;

            if (invader.Distance <= 0)
            {
                invadersToDelete.Add(invader);
                if (this.energy >= 0)
                {
                    this.energy -= invader.Damage;
                }
            }
        }

        this.RemoveInvaders(invadersToDelete);
    }

    public void AddInvader(Invader invader)
    {
        this.invaders.Add(invader);
    }

    public void DestroyHighestPriorityTargets(int count) 
    {
        var toDelete = this.invaders
            .OrderBy(a => a.Distance)
            .ThenByDescending(a => a.Damage)
            .Take(count)
            .ToList();

        this.RemoveInvaders(toDelete);
    }

    public void DestroyTargetsInRadius(int radius)
    {
        var toDelete = this.invaders
            .Where(a => a.Distance <= radius)
            .ToList();
        this.RemoveInvaders(toDelete);
    }

    public IEnumerable<Invader> Invaders()
    {
        return this.invaders;
    }

    private void RemoveInvaders(List<Invader> toDelete)
    {
        foreach (var invader in toDelete)
        {
            this.invaders.Remove(invader);
        }
    }
}
