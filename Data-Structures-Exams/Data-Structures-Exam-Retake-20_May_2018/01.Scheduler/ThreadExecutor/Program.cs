using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class Program
{
    static void Main(string[] args)
    {
        var executor = new ThreadExecutor();
        executor.Execute(new Task(12, 5, Priority.EXTREME));
        executor.Execute(new Task(13, 10, Priority.EXTREME));
        executor.Execute(new Task(14, 15, Priority.EXTREME));

        Console.WriteLine(executor.Cycle(5));
    }
}