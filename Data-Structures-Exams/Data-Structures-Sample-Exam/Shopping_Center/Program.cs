using System;
using System.Collections.Generic;
using System.Linq;

namespace Shopping_Center
{
    class Program
    {
        static void Main(string[] args)
        {
            var shoppingCenter = new ShoppingCenter();
            var n = int.Parse(Console.ReadLine());

            for (int i = 0; i < n; i++)
            {
                var line = Console.ReadLine();
                var command = line.Substring(0, line.IndexOf(" "));
                var elements = line.Substring(line.IndexOf(" ") + 1).Split(new[] { ";" }, StringSplitOptions.None);
                CallCommand(shoppingCenter, command, elements);
            }
        }

        private static void CallCommand(ShoppingCenter shoppingCenter, string command, string[] elements)
        {
            switch (command)
            {
                case "AddProduct":
                    AddProduct(elements, shoppingCenter);
                    break;
                case "DeleteProducts":
                    DeleteProducts(elements, shoppingCenter);
                    break;
                case "FindProductsByName":
                    FindProductsByName(elements, shoppingCenter);
                    break;
                case "FindProductsByProducer":
                    FindProductsByProducer(elements, shoppingCenter);
                    break;
                case "FindProductsByPriceRange":
                    FindProductsByPriceRange(elements, shoppingCenter);
                    break;
            }
        }

        private static void FindProductsByPriceRange(string[] elements, ShoppingCenter shoppingCenter)
        {
            var fromPrice = decimal.Parse(elements[0]);
            var toPrice = decimal.Parse(elements[1]);
            var products = shoppingCenter.FindProductsByPriceRange(fromPrice, toPrice);
            PrintProducts(products);
        }

        private static void FindProductsByProducer(string[] elements, ShoppingCenter shoppingCenter)
        {
            var producer = elements[0];
            var products = shoppingCenter.FindProductsByProducer(producer);
            PrintProducts(products);
        }

        private static void FindProductsByName(string[] elements, ShoppingCenter shoppingCenter)
        {
            var name = elements[0];
            var products = shoppingCenter.FindProductsByName(name);
            PrintProducts(products);
        }

        private static void DeleteProducts(string[] elements, ShoppingCenter shoppingCenter)
        {
            var count = 0;
            if (elements.Length == 2)
            {
                count = shoppingCenter.DeleteProducts(elements[0], elements[1]);
            }
            else
            {
                count = shoppingCenter.DeleteProducts(elements[0]);
            }

            if (count == 0)
            {
                Console.WriteLine("No products found");
            }
            else
            {
                Console.WriteLine($"{count} products deleted");
            }
        }

        private static void AddProduct(string[] elements, ShoppingCenter shoppingCenter)
        {
            shoppingCenter.AddProduct(elements[0], decimal.Parse(elements[1]), elements[2]);
            Console.WriteLine("Product added");
        }

        private static void PrintProducts(IEnumerable<Product> products)
        {
            if (products.Count() == 0)
            {
                Console.WriteLine($"No products found");
            }
            else
            {
                foreach (var product in products)
                {
                    Console.WriteLine($"{{{product.Name};{product.Producer};{product.Price.ToString("F2")}}}");
                }
            }
        }
    }
}