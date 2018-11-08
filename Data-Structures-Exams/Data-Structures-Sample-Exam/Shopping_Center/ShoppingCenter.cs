using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

public class ShoppingCenter
{
    private Dictionary<string, OrderedBag<Product>> byProducer;
    private Dictionary<string, Bag<Product>> byNameAndProducer;
    private Dictionary<string, OrderedBag<Product>> byName;
    private OrderedDictionary<decimal, HashSet<Product>> byPrice;

    public ShoppingCenter()
    {
        this.byProducer = new Dictionary<string, OrderedBag<Product>>();
        this.byNameAndProducer = new Dictionary<string, Bag<Product>>();
        this.byName = new Dictionary<string, OrderedBag<Product>>();
        this.byPrice = new OrderedDictionary<decimal, HashSet<Product>>();
    }

    public void AddProduct(string name, decimal price, string producer)
    {
        var newProduct = new Product(name, price, producer);
        this.AddByProducer(producer, newProduct);
        this.AddByNameAndProducer(name, producer, newProduct);
        this.AddByName(name, newProduct);
        this.AddByPrice(price, newProduct);
    }

    public int DeleteProducts(string producer)
    {
        if (!this.byProducer.ContainsKey(producer))
        {
            return 0;
        }

        var count = this.byProducer[producer].Count;
        foreach (var product in this.byProducer[producer])
        {
            this.byNameAndProducer[this.GetNameAndProducer(product.Name, product.Producer)].Remove(product);
            this.byName[product.Name].Remove(product);
            this.byPrice[product.Price].Remove(product);
        }

        this.byProducer.Remove(producer);
        return count;
    }

    public int DeleteProducts(string name, string producer)
    {
        var nameAndProducer = this.GetNameAndProducer(name, producer);
        if (!this.byNameAndProducer.ContainsKey(nameAndProducer))
        {
            return 0;
        }

        var count = this.byNameAndProducer[nameAndProducer].Count;
        foreach (var product in this.byNameAndProducer[nameAndProducer])
        {
            this.byProducer[product.Producer].Remove(product);
            this.byName[product.Name].Remove(product);
            this.byPrice[product.Price].Remove(product);
        }
        this.byNameAndProducer.Remove(nameAndProducer);
        return count;
    }

    public IEnumerable<Product> FindProductsByName(string name)
    {
        if (!this.byName.ContainsKey(name))
        {
            return Enumerable.Empty<Product>();
        }

        return this.byName[name];
    }

    public IEnumerable<Product> FindProductsByProducer(string producer)
    {
        if (!this.byProducer.ContainsKey(producer))
        {
            return Enumerable.Empty<Product>();
        }

        return this.byProducer[producer];
    }

    public IEnumerable<Product> FindProductsByPriceRange(decimal fromPrice, decimal toPrice)
    {
        var range = this.byPrice.Range(fromPrice, true, toPrice, true);

        var result = new OrderedBag<Product>();
        foreach (var kvp in range)
        {
            foreach (var product in kvp.Value)
            {
                result.Add(product);
            }
        }

        return result;
    }

    private void AddByProducer(string producer, Product newProduct)
    {
        if (!this.byProducer.ContainsKey(producer))
        {
            this.byProducer[producer] = new OrderedBag<Product>();
        }

        this.byProducer[producer].Add(newProduct);
    }

    private string GetNameAndProducer(string name, string producer)
    {
        return name + producer;
    }

    private void AddByNameAndProducer(string name, string producer, Product newProduct)
    {
        var nameAndProducer = this.GetNameAndProducer(name, producer);
        if (!this.byNameAndProducer.ContainsKey(nameAndProducer))
        {
            this.byNameAndProducer[nameAndProducer] = new Bag<Product>();
        }
        this.byNameAndProducer[nameAndProducer].Add(newProduct);
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
            this.byPrice[price] = new HashSet<Product>();
        }
        this.byPrice[price].Add(newProduct);
    }
}