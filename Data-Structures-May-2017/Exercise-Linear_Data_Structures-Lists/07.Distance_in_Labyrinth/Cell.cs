public class Cell
{
    public Cell(int row, int cow, int step)
    {
        this.Row = row;
        this.Cow = cow;
        this.Step = step;
    }

    public int Row { get; private set; }

    public int Cow { get; private set; }

    public int Step { get; set; }
}