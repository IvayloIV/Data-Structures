using System.Linq;

namespace BunnyWars.Core
{
    class BunnyWarsTestingGround
    {
        static void Main(string[] args)
        {
            var bunnyWar = new BunnyWarsStructure();
            bunnyWar.AddRoom(4);
            bunnyWar.AddRoom(1);
            bunnyWar.AddRoom(5);
            bunnyWar.AddBunny("Gosho", 1, 5);
            bunnyWar.AddBunny("Pesho", 1, 1);
            bunnyWar.Next("Gosho");
            bunnyWar.Remove(1);
            //bunnyWar.Previous("Pesho");
            System.Console.WriteLine(string.Join(" - ", bunnyWar.ListBunniesByTeam(2).Select(a => a.Name)));
            System.Console.WriteLine(bunnyWar.RoomCount);
        }
    }
}
