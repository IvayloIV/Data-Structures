using System;
using System.Collections.Generic;

namespace _01.Sweep_And_Prune
{
    class Program
    {
        static void Main(string[] args)
        {
            var items = new List<Item>();
            var byId = new Dictionary<string, Item>();

            string line;
            while ((line = Console.ReadLine()) != "end")
            {
                var tokens = line.Split();
                if (tokens[0] == "add")
                {
                    AddNewItem(items, byId, tokens);
                }
                else if (tokens[0] == "start")
                {
                    var tickCounter = 1;
                    while ((line = Console.ReadLine()) != "end")
                    {
                        tokens = line.Split();
                        if (tokens[0] == "move")
                        {
                            MoveItem(byId, tokens);
                        }
                        if (tokens[0] == "tick" || tokens[0] == "move")
                        {
                            Sweep(items, tickCounter);
                        }

                        tickCounter++;
                    }
                    break;
                }
            }
        }

        private static void MoveItem(Dictionary<string, Item> byId, string[] tokens)
        {
            var currentItem = byId[tokens[1]];
            currentItem.X1 = int.Parse(tokens[2]);
            currentItem.Y1 = int.Parse(tokens[3]);
        }

        private static void AddNewItem(List<Item> items, Dictionary<string, Item> byId, string[] tokens)
        {
            var id = tokens[1];
            var newItem = new Item(id, int.Parse(tokens[2]), int.Parse(tokens[3]));
            items.Add(newItem);
            byId[id] = newItem;
        }

        private static void Sweep(List<Item> items, int tickCounter)
        {
            OrderCollection(items);

            for (int i = 0; i < items.Count; i++)
            {
                var currentItem = items[i];
                for (int j = i + 1; j < items.Count; j++)
                {
                    var tempItem = items[j];

                    if (tempItem.X1 > currentItem.X2)
                    {
                        break;
                    }

                    if (currentItem.IsOverLapWith(tempItem))
                    {
                        Console.WriteLine($"({tickCounter}) {currentItem.Id} collides with {tempItem.Id}");
                    }
                }
            }
        }

        private static void OrderCollection(List<Item> items)
        {
            for (int i = 1; i < items.Count; i++)
            {
                var currentIndex = i;
                var lastIndex = i - 1;
                while (lastIndex >= 0 && items[currentIndex].X1 < items[lastIndex].X1)
                {
                    SwapElements(items, currentIndex--, lastIndex--);
                }
            }
        }

        private static void SwapElements(List<Item> items, int currentIndex, int lastIndex)
        {
            var temp = items[currentIndex];
            items[currentIndex] = items[lastIndex];
            items[lastIndex] = temp;
        }
    }
}