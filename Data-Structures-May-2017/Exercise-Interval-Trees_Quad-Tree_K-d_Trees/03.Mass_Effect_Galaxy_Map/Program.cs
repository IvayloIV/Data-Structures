using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03.Mass_Effect_Galaxy_Map
{
    class Program
    {
        static void Main(string[] args)
        {
            var starsCount = int.Parse(Console.ReadLine());
            var reportsCount = int.Parse(Console.ReadLine());
            var galaxySize = int.Parse(Console.ReadLine());
            var stars = new List<Point2D>();

            var kdStars = new KdTree();
            for (int i = 0; i < starsCount; i++)
            {
                var tokens = Console.ReadLine().Split();
                var id = tokens[0];
                var x = int.Parse(tokens[1]);
                var y = int.Parse(tokens[2]);
                if (IsInsideGalaxy(galaxySize, x, y))
                {
                    kdStars.Insert(new Point2D(id, x, y));
                }
            }

            for (int i = 0; i < reportsCount; i++)
            {
                var tokens = Console.ReadLine().Split();
                var x = int.Parse(tokens[1]);
                var y = int.Parse(tokens[2]);
                var width = int.Parse(tokens[3]);
                var height = int.Parse(tokens[4]);
                int clustersCount = kdStars.GetClustersCount(x, x + width, y, y + height, 0);
                Console.WriteLine(clustersCount);
            }
        }

        private static bool IsInsideGalaxy(int galaxySize, int x, int y)
        {
            return x >= 0 && y >= 0 && x <= galaxySize && y <= galaxySize;
        }
    }
}
