using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class Launcher
{
    static void Main(string[] args)
    {
        IEnterprise enterprise = new Enterprise();

        Stopwatch watch = new Stopwatch();
        watch.Start();

        for (int i = 0; i < 100000; i++)
        {
            Employee employee = new Employee("a" + i, "a" + i, 1, Position.Hr, DateTime.Now);

            enterprise.Add(employee);
        }

        watch.Stop();
        long l1 = watch.ElapsedMilliseconds;

        int count = enterprise.Count;
        Console.WriteLine(100000 == count);

        Console.WriteLine(l1 < 350);

    }
}