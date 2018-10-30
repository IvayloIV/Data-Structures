namespace BunnyWars.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Wintellect.PowerCollections;

    public class BunnyWarsStructure : IBunnyWarsStructure
    {
        private static IComparer<Bunny> SuffixComparator = new OrdinalSuffixComparator();

        private Dictionary<int, LinkedList<Bunny>[]> rooms;
        private Dictionary<string, Bunny> bunnies;
        private OrderedSet<int> roomsById;
        private Dictionary<int, SortedSet<Bunny>> teams;
        private OrderedSet<Bunny> suffixBunnies;

        public BunnyWarsStructure()
        {
            this.rooms = new Dictionary<int, LinkedList<Bunny>[]>();
            this.bunnies = new Dictionary<string, Bunny>();
            this.roomsById = new OrderedSet<int>();
            this.teams = new Dictionary<int, SortedSet<Bunny>>();
            this.suffixBunnies = new OrderedSet<Bunny>(SuffixComparator);
        }

        public int BunnyCount { get { return this.bunnies.Count; } }

        public int RoomCount { get { return this.rooms.Count; } }

        public void AddRoom(int roomId)
        {
            if (this.rooms.ContainsKey(roomId))
            {
                throw new ArgumentException();
            }

            this.rooms[roomId] = new LinkedList<Bunny>[5];
            this.roomsById.Add(roomId);
        }

        public void AddBunny(string name, int team, int roomId)
        {
            if (this.bunnies.ContainsKey(name) || !this.rooms.ContainsKey(roomId))
            {
                throw new ArgumentException();
            }

            if (team < 0 || team > 4)
            {
                throw new IndexOutOfRangeException();
            }

            var newBunny = new Bunny(name, team, roomId);
            this.bunnies[name] = newBunny;
            if (this.rooms[roomId][newBunny.Team] == null)
            {
                this.rooms[roomId][newBunny.Team] = new LinkedList<Bunny>();
            }

            this.rooms[roomId][newBunny.Team].AddLast(newBunny);
            this.AddByTeam(team, newBunny);
            this.suffixBunnies.Add(newBunny);
        }

        public void Remove(int roomId)
        {
            if (!this.rooms.ContainsKey(roomId))
            {
                throw new ArgumentException();
            }

            var bunniesForDelete = this.rooms[roomId];
            this.DeleteBunnies(bunniesForDelete);
            this.rooms.Remove(roomId);
            this.roomsById.Remove(roomId);
        }

        public void Next(string bunnyName)
        {
            this.CheckExistBunny(bunnyName);
            var currentBunny = this.bunnies[bunnyName];
            this.MoveBunny(currentBunny, 1);
        }

        public void Previous(string bunnyName)
        {
            this.CheckExistBunny(bunnyName);
            var currentBunny = this.bunnies[bunnyName];
            this.MoveBunny(currentBunny, -1);
        }

        public void Detonate(string bunnyName)
        {
            if (!this.bunnies.ContainsKey(bunnyName))
            {
                throw new ArgumentException();
            }

            var currentBunny = this.bunnies[bunnyName];
            var currentBunnyTeam = currentBunny.Team;
            var rooms = this.rooms[currentBunny.RoomId];

            var deadBunnies = new HashSet<Bunny>();
            for (int i = 0; i < rooms.Length; i++)
            {
                var currentTeam = rooms[i];
                if (currentTeam == null || i == currentBunnyTeam)
                {
                    continue;
                }

                var currentNode = currentTeam.First;
                while (currentNode != null)
                {
                    var currentMember = currentNode.Value;
                    currentMember.Health -= 30;
                    if (currentMember.Health <= 0)
                    {
                        currentTeam.Remove(currentNode);
                        this.teams[currentMember.Team].Remove(currentMember);
                        this.bunnies.Remove(currentMember.Name);
                        this.suffixBunnies.Remove(currentMember);
                        currentBunny.Score++;
                    }

                    currentNode = currentNode.Next;
                }
            }
        }

        public IEnumerable<Bunny> ListBunniesByTeam(int team)
        {
            if (!this.teams.ContainsKey(team))
            {
                throw new IndexOutOfRangeException();
            }

            return this.teams[team];
        }

        public IEnumerable<Bunny> ListBunniesBySuffix(string suffix)
        {
            var low = new Bunny(suffix, 0, 0);
            var high = new Bunny(char.MaxValue + suffix, 0, 0);

            return this.suffixBunnies.Range(low, true, high, false);
        }

        private void CheckExistBunny(string bunnyName)
        {
            if (!this.bunnies.ContainsKey(bunnyName))
            {
                throw new ArgumentException();
            }
        }

        private void MoveBunny(Bunny currentBunny, int steep)
        {
            var currentIndex = this.roomsById.IndexOf(currentBunny.RoomId);
            var nextIndex = currentIndex + steep;

            if (nextIndex < 0)
            {
                nextIndex = this.RoomCount - 1;
            }
            else if (nextIndex > this.RoomCount - 1)
            {
                nextIndex = 0;
            }

            var newRoomId = this.roomsById[nextIndex];
            this.rooms[currentBunny.RoomId][currentBunny.Team].Remove(currentBunny);
            currentBunny.RoomId = newRoomId;

            if (this.rooms[newRoomId][currentBunny.Team] == null)
            {
                this.rooms[newRoomId][currentBunny.Team] = new LinkedList<Bunny>();
            }
            this.rooms[newRoomId][currentBunny.Team].AddLast(currentBunny);
        }

        private void AddByTeam(int team, Bunny newBunny)
        {
            if (!this.teams.ContainsKey(team))
            {
                this.teams[team] = new SortedSet<Bunny>();
            }

            this.teams[team].Add(newBunny);
        }

        private void DeleteBunnies(LinkedList<Bunny>[] bunniesForDelete)
        {
            foreach (var bunnyList in bunniesForDelete)
            {
                if (bunnyList != null)
                {
                    foreach (var bunny in bunnyList)
                    {
                        this.teams[bunny.Team].Remove(bunny);
                        this.bunnies.Remove(bunny.Name);
                        this.suffixBunnies.Remove(bunny);
                    }
                }
            }
        }
    }
}
