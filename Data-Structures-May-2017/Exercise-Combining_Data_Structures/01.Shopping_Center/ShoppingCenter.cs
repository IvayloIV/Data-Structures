using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

public class ShoppingCenter
{
    Dictionary<string, OrderedBag<Product>> byProducer;
    Dictionary<string, Bag<Product>> byNameAndProducer;
    Dictionary<string, OrderedBag<Product>> byName;
    OrderedDictionary<decimal, Bag<Product>> byPrice;

    public ShoppingCenter()
    {
        this.byProducer = new Dictionary<string, OrderedBag<Product>>();
        this.byNameAndProducer = new Dictionary<string, Bag<Product>>();
        this.byName = new Dictionary<string, OrderedBag<Product>>();
        this.byPrice = new OrderedDictionary<decimal, Bag<Product>>();
    }

    public void AddProduct(string name, decimal price, string producer)
    {
        var newProduct = new Product(name, price, producer);
        this.AddByProducer(producer, newProduct);
        this.AddByNameAndProducer(name, producer, newProduct);
        this.AddByName(name, newProduct);
        this.AddByPrice(price, newProduct);
    }

    public string DeleteProducts(string producer)
    {
        if (!this.byProducer.ContainsKey(producer))
        {
            return "No products found";
        }

        var deleteProducts = this.byProducer[producer];
        if (deleteProducts.Count == 0)
        {
            return "No products found";
        }

        this.byProducer.Remove(producer);
        foreach (var product in deleteProducts)
        {
            this.byNameAndProducer[product.Name + product.Producer].Remove(product);
            this.byName[product.Name].Remove(product);
            this.byPrice[product.Price].Remove(product);
        }


        return $"{deleteProducts.Count} products deleted";
    }

    public string DeleteProducts(string name, string producer)
    {
        if (!this.byNameAndProducer.ContainsKey(name + producer))
        {
            return "No products found";
        }

        var deleteProducts = this.byNameAndProducer[name + producer];
        if (deleteProducts.Count == 0)
        {
            return "No products found";
        }

        this.byNameAndProducer.Remove(name + producer);
        foreach (var product in deleteProducts)
        {
            this.byProducer[producer].Remove(product);
            this.byName[product.Name].Remove(product);
            this.byPrice[product.Price].Remove(product);
        }

        return $"{deleteProducts.Count} products deleted";
    }

    public IEnumerable<Product> FindProductsByProducer(string producer)
    {
        if (!this.byProducer.ContainsKey(producer) || this.byProducer[producer].Count == 0)
        {
            Console.WriteLine($"No products found");
            return Enumerable.Empty<Product>();
        }
        return this.byProducer[producer];
    }

    public IEnumerable<Product> FindProductsByName(string name)
    {
        if (!this.byName.ContainsKey(name) || this.byName[name].Count == 0)
        {
            Console.WriteLine($"No products found");
            return Enumerable.Empty<Product>();
        }
        return this.byName[name];
    }

    public IEnumerable<Product> FindProductsByPriceRange(decimal fromPrice, decimal toPrice)
    {
        var range = this.byPrice.Range(fromPrice, true, toPrice, true);

        var bag = new OrderedBag<Product>();
        foreach (var kvp in range)
        {
            foreach (var product in kvp.Value)
            {
                bag.Add(product);
            }
        }

        if (bag.Count == 0)
        {
            Console.WriteLine($"No products found");
            return Enumerable.Empty<Product>();
        }

        return bag;
    }

    private void AddByProducer(string producer, Product newProduct)
    {
        if (!this.byProducer.ContainsKey(producer))
        {
            this.byProducer[producer] = new OrderedBag<Product>();
        }

        this.byProducer[producer].Add(newProduct);
    }

    private void AddByNameAndProducer(string name, string producer, Product newProduct)
    {
        if (!this.byNameAndProducer.ContainsKey(name + producer))
        {
            this.byNameAndProducer[name + producer] = new Bag<Product>();
        }

        this.byNameAndProducer[name + producer].Add(newProduct);
    }

    private void AddByName(string name, Product newProduct)
    {
        if (!this.byName.ContainsKey(name))
        {
            this.byName[name] = new OrderedBag<Product>();
        }
        this.byName[name].Add(newProduct);
    }

    private void AddByPrice(decimal price, Product newProduct)
    {
        if (!this.byPrice.ContainsKey(price))
        {
            this.byPrice[price] = new Bag<Product>();
        }

        this.byPrice[price].Add(newProduct);
    }
}