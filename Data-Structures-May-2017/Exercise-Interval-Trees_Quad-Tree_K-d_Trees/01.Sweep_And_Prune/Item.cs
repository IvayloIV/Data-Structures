public class Item
{
    public Item(string id, int x1, int y1)
    {
        this.Id = id;
        this.X1 = x1;
        this.Y1 = y1;
    }

    public string Id { get; set; }
    public int X1 { get; set; }
    public int Y1 { get; set; }
    public int X2 => this.X1 + 10;
    public int Y2 => this.Y1 + 10;

    public bool IsOverLapWith(Item other)
    {
        return this.X1 < other.X2 &&
            this.X2 > other.X1 &&
            this.Y1 < other.Y2 &&
            this.Y2 > other.Y1;
    }
}