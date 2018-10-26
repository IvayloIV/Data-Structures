namespace Classes
{
    using Interfaces;

    public class Minion : IMinion
    {
        public Minion(int id, int xCoordinate)
        {
            this.Id = id;
            this.XCoordinate = xCoordinate;
            this.Health = 100;
        }

        public int CompareTo(Minion other)
        {
            var cmp = this.XCoordinate.CompareTo(other.XCoordinate);
            if (cmp == 0) cmp = this.Id.CompareTo(other.Id);

            return cmp;
        }

        public int Id { get; private set; }

        public int XCoordinate { get; private set; }

        public int Health { get; set; }
    }
}
