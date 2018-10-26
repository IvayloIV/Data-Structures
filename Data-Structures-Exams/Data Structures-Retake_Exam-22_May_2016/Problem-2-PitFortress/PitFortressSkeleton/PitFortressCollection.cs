using System;
using System.Collections.Generic;
using System.Linq;
using Classes;
using Interfaces;
using Wintellect.PowerCollections;

public class PitFortressCollection : IPitFortress
{
    private Dictionary<string, Player> byPlayerName;
    private SortedSet<Player> sortedPlayers;
    private OrderedDictionary<int, HashSet<Minion>> minions;
    private SortedSet<Minion> sortedMinions;
    private SortedSet<Mine> mines;

    public PitFortressCollection()
    {
        this.byPlayerName = new Dictionary<string, Player>();
        this.sortedPlayers = new SortedSet<Player>();
        this.minions = new OrderedDictionary<int, HashSet<Minion>>();
        this.sortedMinions = new SortedSet<Minion>();
        this.mines = new SortedSet<Mine>();
        this.MinionsIdCount = 1;
        this.MinesIdCount = 1;
    }

    public int PlayersCount { get; private set; }

    public int MinionsCount { get; private set; }

    public int MinesCount { get; private set; }

    public int MinionsIdCount { get; private set; }

    public int MinesIdCount { get; private set; }

    public void AddPlayer(string name, int mineRadius)
    {
        if (this.byPlayerName.ContainsKey(name) || mineRadius < 0)
        {
            throw new ArgumentException();
        }

        var newPlayer = new Player(name, mineRadius);
        this.byPlayerName[name] = newPlayer;
        this.sortedPlayers.Add(newPlayer);
        this.PlayersCount++;
    }

    public void AddMinion(int xCoordinate)
    {
        if (xCoordinate < 0 || xCoordinate > 1000000)
        {
            throw new ArgumentException();
        }

        if (!this.minions.ContainsKey(xCoordinate))
        {
            this.minions[xCoordinate] = new HashSet<Minion>();
        }

        var newMinion = new Minion(this.MinionsIdCount++, xCoordinate);
        this.minions[xCoordinate].Add(newMinion);
        this.sortedMinions.Add(newMinion);
        this.MinionsCount++;
    }

    public void SetMine(string playerName, int xCoordinate, int delay, int damage)
    {
        if (!this.byPlayerName.ContainsKey(playerName) || xCoordinate < 0 || xCoordinate > 1000000
            || delay < 1 || delay > 10000 || damage < 0 || damage > 100)
        {
            throw new ArgumentException();
        }

        var newMine = new Mine(this.MinesIdCount++, delay, damage, xCoordinate, this.byPlayerName[playerName]);
        this.mines.Add(newMine);
        this.MinesCount++;
    }

    public IEnumerable<Minion> ReportMinions()
    {
        return this.sortedMinions;
    }

    public IEnumerable<Player> Top3PlayersByScore()
    {
        this.CheckCountOfPlayers();

        return this.sortedPlayers.Reverse().Take(3);
    }

    public IEnumerable<Player> Min3PlayersByScore()
    {
        this.CheckCountOfPlayers();

        return this.sortedPlayers.Take(3);
    }

    public IEnumerable<Mine> GetMines()
    {
        return this.mines;
    }

    public void PlayTurn()
    {
        var explodeMines = new SortedSet<Mine>(this.mines);
        foreach (var mine in this.mines)
        {
            mine.Delay--;
            if (mine.Delay <= 0)
            {
                this.Explode(mine);
                explodeMines.Remove(mine);
                this.MinesCount--;
            }
        }
        this.mines = explodeMines;
    }

    private void Explode(Mine mine)
    {
        var startRange = mine.XCoordinate - mine.Player.Radius;
        var endRange = mine.XCoordinate + mine.Player.Radius;

        var range = this.minions.Range(startRange, true, endRange, true);
        var deadMinion = new HashSet<Minion>();
        ReduceHealthOfMinions(mine, range, deadMinion);

        this.UpdateMinions(deadMinion);
    }

    private void ReduceHealthOfMinions(Mine mine, OrderedDictionary<int, 
        HashSet<Minion>>.View range, 
        HashSet<Minion> deadMinion)
    {
        foreach (var kvp in range)
        {
            foreach (var minion in kvp.Value)
            {
                minion.Health -= mine.Damage;
                if (minion.Health <= 0)
                {
                    this.UpdatePlayer(mine);
                    deadMinion.Add(minion);
                    this.MinionsCount--;
                }
            }
        }
    }

    private void UpdatePlayer(Mine mine)
    {
        this.sortedPlayers.Remove(mine.Player);
        mine.Player.Score++;
        this.sortedPlayers.Add(mine.Player);
    }

    private void UpdateMinions(HashSet<Minion> deadMinion)
    {
        foreach (var minion in deadMinion)
        {
            this.minions[minion.XCoordinate].Remove(minion);
            this.sortedMinions.Remove(minion);
        }
    }

    private void CheckCountOfPlayers()
    {
        if (this.PlayersCount < 3)
        {
            throw new ArgumentException();
        }
    }
}