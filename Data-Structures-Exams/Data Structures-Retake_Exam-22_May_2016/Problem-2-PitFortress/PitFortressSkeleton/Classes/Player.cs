namespace Classes
{
    using Interfaces;

    public class Player : IPlayer
    {
        public Player(string name, int radius)
        {
            this.Name = name;
            this.Radius = radius;
            this.Score = 0;
        }

        public int CompareTo(Player other)
        {
            var cmp = this.Score.CompareTo(other.Score);

            if (cmp == 0)
            {
                cmp = this.Name.CompareTo(other.Name);
            }

            return cmp;
        }

        public string Name { get; private set; }

        public int Radius { get; private set; }

        public int Score { get; set; }
    }
}
