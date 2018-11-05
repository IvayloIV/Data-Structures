using System;

public static class JudgeLauncher
{
    public static void Main()
    {
        var judje = new Judge();
        judje.AddContest(3);
        judje.AddContest(2);
        judje.AddContest(1);
        judje.AddContest(-4);
        Console.WriteLine(string .Join(" ", judje.GetContests()));
    }
}

