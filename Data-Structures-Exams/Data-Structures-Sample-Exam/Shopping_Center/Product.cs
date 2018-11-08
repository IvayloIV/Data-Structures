using System;

public class Product : IComparable<Product>
{
    public string Name { get; private set; }

    public decimal Price { get; private set; }

    public string Producer { get; private set; }

    public Product(string name, decimal price, string producer)
    {
        this.Name = name;
        this.Price = price;
        this.Producer = producer;
    }

    public int CompareTo(Product other)
    {
        var cmp = this.Name.CompareTo(other.Name);

        if (cmp == 0) 
        {
            cmp = this.Producer.CompareTo(other.Producer);
        }

        if (cmp == 0)
        {
            cmp = this.Price.CompareTo(other.Price);
        }

        return cmp;
    }
}