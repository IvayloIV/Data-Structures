using System;
using System.Collections.Generic;
using Wintellect.PowerCollections;

public class TextEditor : ITextEditor
{
    private Trie<BigList<char>> userString;
    private Trie<Stack<string>> userStack;

    public TextEditor()
    {
        this.userString = new Trie<BigList<char>>();
        this.userStack = new Trie<Stack<string>>();
    }

    public void Clear(string username)
    {
        if (!this.userString.Contains(username))
        {
            return;
        }

        UpdateHistory(username);
        this.userString.GetValue(username).Clear();
    }

    private void UpdateHistory(string username)
    {
        var userStack = this.userStack.GetValue(username);
        userStack.Push(string.Join("", this.userString.GetValue(username)));
    }

    public void Delete(string username, int startIndex, int length)
    {
        if (!this.userString.Contains(username))
        {
            return;
        }

        UpdateHistory(username);
        this.userString.GetValue(username).RemoveRange(startIndex, length);
    }

    public void Insert(string username, int index, string str)
    {
        if (!this.userString.Contains(username))
        {
            return;
        }

        this.UpdateHistory(username);
        this.userString.GetValue(username).InsertRange(index, str);
    }

    public int Length(string username)
    {
        if (!this.userString.Contains(username))
        {
            return 0;
        }

        return this.userString.GetValue(username).Count;
    }

    public void Login(string username)
    {
        this.userString.Insert(username, new BigList<char>());
        this.userStack.Insert(username, new Stack<string>());
    }

    public void Logout(string username)
    {
        if (!this.userString.Contains(username))
        {
            return;
        }
        this.userString.Delete(username);
        this.userStack.Delete(username);
    }

    public void Prepend(string username, string str)
    {
        if (!this.userString.Contains(username))
        {
            return;
        }

        this.UpdateHistory(username);
        this.userString.GetValue(username).AddRangeToFront(str);
    }

    public string Print(string username)
    {
        if (!this.userString.Contains(username))
        {
            return "";
        }
        return String.Join("", this.userString.GetValue(username));
    }

    public void Substring(string username, int startIndex, int length)
    {
        if (!this.userString.Contains(username))
        {
            return;
        }

        this.UpdateHistory(username);
        var userString = this.userString.GetValue(username);
        var elements = userString.GetRange(startIndex, length);
        this.userString.Insert(username, new BigList<char>(elements));
    }

    public void Undo(string username)
    {
        if (!this.userString.Contains(username))
        {
            return;
        }

        var userStack = this.userStack.GetValue(username);
        if (userStack.Count == 0)
        {
            return;
        }
        var lastUserString = userStack.Pop();
        this.UpdateHistory(username);
        this.userString.Insert(username, new BigList<char>(lastUserString));
    }

    public IEnumerable<string> Users(string prefix = "")
    {
        foreach (var user in userString.GetByPrefix(prefix))
        {
            yield return user;
        }
    }
}