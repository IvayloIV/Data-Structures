using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

public class Scoreboard : IScoreboard
{
    private Dictionary<string, string> users;
    private Dictionary<string, string> games;
    private Dictionary<string, OrderedBag<ScoreboardEntry>> scores;
    private OrderedSet<string> prefixNames;

    public Scoreboard(int maxEntriesToKeep = 10)
    {
        this.users = new Dictionary<string, string>();
        this.games = new Dictionary<string, string>();
        this.scores = new Dictionary<string, OrderedBag<ScoreboardEntry>>();
        this.prefixNames = new OrderedSet<string>(string.CompareOrdinal);
    }

    public bool RegisterUser(string username, string password)
    {
        if (this.users.ContainsKey(username))
        {
            return false;
        }

        this.users[username] = password;
        return true;
    }

    public bool RegisterGame(string game, string password)
    {
        if (this.games.ContainsKey(game))
        {
            return false;
        }

        this.games[game] = password;
        this.scores[game] = new OrderedBag<ScoreboardEntry>();
        this.prefixNames.Add(game);
        return true;
    }

    public bool AddScore(string username, string userPassword, string game, string gamePassword, int score)
    {
        if (!this.users.ContainsKey(username) || !this.games.ContainsKey(game) ||
            this.users[username] != userPassword || this.games[game] != gamePassword)
        {
            return false;
        }

        this.scores[game].Add(new ScoreboardEntry(username, score));
        return true;
    }

    public IEnumerable<ScoreboardEntry> ShowScoreboard(string game)
    {
        if (!this.games.ContainsKey(game))
        {
            return null;
        }

        return this.scores[game].Take(10);
    }

    public bool DeleteGame(string game, string gamePassword)
    {
        if (!this.games.ContainsKey(game) || this.games[game] != gamePassword)
        {
            return false;
        }

        this.games.Remove(game);
        this.scores.Remove(game);
        this.prefixNames.Remove(game);
        return true;
    }

    public IEnumerable<string> ListGamesByPrefix(string gameNamePrefix)
    {
        return this.prefixNames.Range(gameNamePrefix, true, gameNamePrefix + char.MaxValue, false).Take(10);
    }
}