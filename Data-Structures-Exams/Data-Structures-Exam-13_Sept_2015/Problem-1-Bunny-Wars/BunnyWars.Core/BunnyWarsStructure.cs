namespace BunnyWars.Core
{
    using System;
    using System.Collections.Generic;
    using Wintellect.PowerCollections;

    public class BunnyWarsStructure : IBunnyWarsStructure
    {
        private Dictionary<int, LinkedList<Bunny>[]> rooms;
        private OrderedSet<int> roomsByIndex;
        private Dictionary<string, Bunny> bunnies;
        private Dictionary<int, SortedSet<Bunny>> teams;
        private OrderedSet<Bunny> suffixBunnies;

        public BunnyWarsStructure()
        {
            this.rooms = new Dictionary<int, LinkedList<Bunny>[]>();
            this.roomsByIndex = new OrderedSet<int>();
            this.bunnies = new Dictionary<string, Bunny>();
            this.teams = new Dictionary<int, SortedSet<Bunny>>();
            this.suffixBunnies = new OrderedSet<Bunny>(new SuffixOrder());
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
            this.roomsByIndex.Add(roomId);
        }

        public void AddBunny(string name, int team, int roomId)
        {
            if (this.bunnies.ContainsKey(name) || !this.rooms.ContainsKey(roomId))
            {
                throw new ArgumentException();
            }

            var newBunny = new Bunny(name, team, roomId);
            this.bunnies[name] = newBunny;
            this.AddBunnyInRoom(team, roomId, newBunny);
            this.AddBunnyByTeam(team, newBunny);
            this.suffixBunnies.Add(newBunny);
        }

        public void Remove(int roomId)
        {
            if (!this.rooms.ContainsKey(roomId))
            {
                throw new ArgumentException();
            }

            this.RemoveAllBunniesInRoom(roomId);

            this.rooms.Remove(roomId);
            this.roomsByIndex.Remove(roomId);
        }

        public void Next(string bunnyName)
        {
            if (!this.bunnies.ContainsKey(bunnyName))
            {
                throw new ArgumentException();
            }

            var currentBunny = this.bunnies[bunnyName];
            this.MoveBunny(bunnyName, currentBunny, 1);

        }

        public void Previous(string bunnyName)
        {
            if (!this.bunnies.ContainsKey(bunnyName))
            {
                throw new ArgumentException();
            }

            var currentBunny = this.bunnies[bunnyName];
            this.MoveBunny(bunnyName, currentBunny, -1);
        }

        public void Detonate(string bunnyName)
        {
            if (!this.bunnies.ContainsKey(bunnyName))
            {
                throw new ArgumentException();
            }

            var currentBunny = this.bunnies[bunnyName];
            var killedBunnies = new LinkedList<Bunny>();
            this.ReduceHealth(currentBunny, killedBunnies);
            currentBunny.Score += killedBunnies.Count;
            this.RemoveKilledBunnies(killedBunnies);
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

        private void AddBunnyInRoom(int team, int roomId, Bunny newBunny)
        {
            if (this.rooms[roomId][team] == null)
            {
                this.rooms[roomId][team] = new LinkedList<Bunny>();
            }
            this.rooms[roomId][team].AddLast(newBunny);
        }

        private void AddBunnyByTeam(int team, Bunny newBunny)
        {
            if (!this.teams.ContainsKey(team))
            {
                this.teams[team] = new SortedSet<Bunny>();
            }

            this.teams[team].Add(newBunny);
        }

        private void MoveBunny(string bunnyName, Bunny currentBunny, int step)
        {
            var oldIndex = this.roomsByIndex.IndexOf(currentBunny.RoomId);
            var newIndex = oldIndex + step;

            if (newIndex > this.roomsByIndex.Count - 1)
            {
                newIndex = 0;
            }
            else if (newIndex < 0)
            {
                newIndex = this.roomsByIndex.Count - 1;
            }

            this.rooms[currentBunny.RoomId][currentBunny.Team].Remove(currentBunny);
            var newRoom = this.roomsByIndex[newIndex];
            this.AddBunnyInRoom(currentBunny.Team, newRoom, currentBunny);
            currentBunny.RoomId = newRoom;
        }

        private void RemoveKilledBunnies(LinkedList<Bunny> killedBunnies)
        {
            foreach (var bunny in killedBunnies)
            {
                this.bunnies.Remove(bunny.Name);
                this.rooms[bunny.RoomId][bunny.Team].Remove(bunny);
                this.teams[bunny.Team].Remove(bunny);
                this.suffixBunnies.Remove(bunny);
            }
        }

        private void ReduceHealth(Bunny currentBunny, LinkedList<Bunny> killedBunnies)
        {
            for (int i = 0; i < this.rooms[currentBunny.RoomId].Length; i++)
            {
                if (i == currentBunny.Team || this.rooms[currentBunny.RoomId][i] == null)
                {
                    continue;
                }
                var currentTeam = this.rooms[currentBunny.RoomId][i];

                foreach (var bunny in currentTeam)
                {
                    bunny.Health -= 30;

                    if (bunny.Health <= 0)
                    {
                        killedBunnies.AddLast(bunny);
                    }
                }
            }
        }

        private void RemoveAllBunniesInRoom(int roomId)
        {
            foreach (var arr in this.rooms[roomId])
            {
                if (arr != null)
                {
                    foreach (var bunny in arr)
                    {
                        this.bunnies.Remove(bunny.Name);
                        this.teams[bunny.Team].Remove(bunny);
                        this.suffixBunnies.Remove(bunny);
                    }
                }
            }
        }
    }
}