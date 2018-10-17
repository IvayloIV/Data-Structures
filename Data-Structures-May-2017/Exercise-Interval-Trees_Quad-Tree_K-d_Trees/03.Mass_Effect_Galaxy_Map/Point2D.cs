using System;

public class Point2D : IComparable<Point2D>
{
    public Point2D(string id, double x, double y)
    {
        this.X = x;
        this.Y = y;
        this.Id = id;
    }

    public string Id { get; set; }
    public double X { get; set; }
    public double Y { get; set; }

    public override string ToString()
    {
        return string.Format("{0} - ({1}, {2})", this.Id, this.X, this.Y);
    }

    public override bool Equals(object obj)
    {
        if (obj == this) return true;
        if (obj == null) return false;
        if (obj.GetType() != this.GetType()) return false; 
        Point2D that = (Point2D)obj;
        return this.X == that.X && this.Y == that.Y;
    }

    public bool IsInto(int x1, int y1, int x2, int y2)
    {
        return X >= x1 && X <= x2 && Y >= y1 && Y <= y2;
    }

    public override int GetHashCode()
    {
        int hashX = this.X.GetHashCode();
        int hashY = this.Y.GetHashCode();
        return 31 * hashX + hashY;
    }

    public int CompareTo(Point2D that)
    {
        if (this.Y < that.Y) return -1;
        if (this.Y > that.Y) return +1;
        if (this.X < that.X) return -1;
        if (this.X > that.X) return +1;
        return 0;
    }
}
