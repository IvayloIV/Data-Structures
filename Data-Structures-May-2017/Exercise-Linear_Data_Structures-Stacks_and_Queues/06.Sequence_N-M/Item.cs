class Item<T>
{
    public T Value { get; private set; }

    public Item<T> PrevItem { get; private set; }

    public Item(T value, Item<T> prevItem)
    {
        this.Value = value;
        this.PrevItem = prevItem;
    }

    public Item(T value)
    {
        this.Value = value;
    }
}