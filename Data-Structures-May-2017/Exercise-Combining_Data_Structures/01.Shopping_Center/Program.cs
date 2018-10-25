using System;
using System.Collections.Generic;

namespace _01.Shopping_Center
{
    class Program
    {
        static void Main(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            var shoppingCenter = new ShoppingCenter();

            for (int i = 0; i < n; i++)
            {
                var line = Console.ReadLine();
                var tokens = line.Split(new[] { " " }, StringSplitOptions.None);

                switch (tokens[0])
                {
                    case "AddProduct":
                        AddProduct(shoppingCenter, line, tokens);
                        break;
                    case "DeleteProducts":
                        DeleteProducts(shoppingCenter, line, tokens);
                        break;
                    case "FindProductsByProducer":
                        FindProductsByProducer(shoppingCenter, line);
                        break;
                    case "FindProductsByName":
                        FindProductsByName(shoppingCenter, line);
                        break;
                    case "FindProductsByPriceRange":
                        FindProductsByPriceRange(shoppingCenter, line);
                        break;
                }
            }
        }

        private static void FindProductsByPriceRange(ShoppingCenter shoppingCenter, string line)
        {
            var productTokens = SplitLine(line);
            var fromPrice = productTokens[0].Substring(productTokens[0].IndexOf(" ") + 1);
            var products = shoppingCenter.FindProductsByPriceRange(decimal.Parse(fromPrice), decimal.Parse(productTokens[1]));
            PrintProducts(products);
        }

        private static void FindProductsByProducer(ShoppingCenter shoppingCenter, string line)
        {
            var productTokens = SplitLine(line);
            var producer = productTokens[0].Substring(productTokens[0].IndexOf(" ") + 1);
            var products = shoppingCenter.FindProductsByProducer(producer);
            PrintProducts(products);
        }

        private static void FindProductsByName(ShoppingCenter shoppingCenter, string line)
        {
            var name = line.Substring(line.IndexOf(" ") + 1);
            var products = shoppingCenter.FindProductsByName(name);
            PrintProducts(products);
        }

        private static void PrintProducts(IEnumerable<Product> products)
        {
            foreach (var product in products)
            {
                Console.WriteLine(product);
            }
        }

        private static string[] SplitLine(string line)
        {
            return line.Split(new[] { ";" }, StringSplitOptions.None);
        }

        private static void DeleteProducts(ShoppingCenter shoppingCenter, string line, string[] tokens)
        {
            var productTokens = SplitLine(line);
            var name = productTokens[0].Substring(productTokens[0].IndexOf(" ") + 1);
            if (productTokens.Length == 1)
            {
                Console.WriteLine(shoppingCenter.DeleteProducts(name));
            }
            else
            {
                Console.WriteLine(shoppingCenter.DeleteProducts(name, productTokens[1]));
            }
        }

        private static void AddProduct(ShoppingCenter shoppingCenter, string line, string[] tokens)
        {
            var productTokens = SplitLine(line);
            var name = productTokens[0].Substring(productTokens[0].IndexOf(" ") + 1);
            shoppingCenter.AddProduct(name, decimal.Parse(productTokens[1]), productTokens[2]);
            Console.WriteLine("Product added");
        }
    }
}