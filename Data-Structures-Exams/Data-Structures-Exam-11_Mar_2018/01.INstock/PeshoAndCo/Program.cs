
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        var instock = new Instock();
        instock.Add(new Product("bob1", 23.1, 2));
        instock.Add(new Product("bob2", 23.5, 2));
        instock.Add(new Product("bob3", 21.2, 2));
        System.Console.WriteLine(string.Join(" - ", instock.FindAllInRange(21.3, 24).Select(a => a.Label)));
    }
}

