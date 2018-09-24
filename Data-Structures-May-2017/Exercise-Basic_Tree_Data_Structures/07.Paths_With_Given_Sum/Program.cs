using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _07.Paths_With_Given_Sum
{
    class Program
    {
        static void Main(string[] args)
        {
            var trees = new Dictionary<int, Tree<int>>();
            var pairs = int.Parse(Console.ReadLine());
            for (int i = 1; i < pairs; i++)
            {
                var nums = Console.ReadLine().Split().Select(int.Parse).ToArray();
                var parentValue = nums[0];
                var childrenValue = nums[1];

                Tree<int> currentParrent = GetTree(trees, parentValue);
                Tree<int> currentChild = GetTree(trees, childrenValue);

                SetRelations(currentParrent, currentChild);
            }
            var sum = int.Parse(Console.ReadLine());
            var root = trees.Where(a => a.Value.Parent == null).FirstOrDefault().Value;
            var equalTreeSum = new List<Tree<int>>();
            FindSum(root, sum, equalTreeSum, root.Value);
            PrintResult(equalTreeSum, sum); 
        }

        private static void PrintResult(List<Tree<int>> equalTreeSum, int sum)
        {
            Console.WriteLine($"Paths of sum {sum}:");
            foreach (var tree in equalTreeSum)
            {
                PrintPath(tree);
            }
        }

        private static void FindSum(Tree<int> tree, int sum, List<Tree<int>> equalTreeSum, int currentSum)
        {
            if (currentSum == sum && tree.Children.Count == 0)
            {
                equalTreeSum.Add(tree);
            }

            foreach (var child in tree.Children)
            {
                FindSum(child, sum, equalTreeSum, currentSum + child.Value);
            }
        }

        private static void PrintPath(Tree<int> longestTree)
        {
            var path = new List<Tree<int>>();
            while (longestTree != null)
            {
                path.Add(longestTree);
                longestTree = longestTree.Parent;
            }

            path.Reverse();
            Console.WriteLine($"{string.Join(" ", path.Select(a => a.Value))}");
        }

        private static void SetRelations(Tree<int> currentParrent, Tree<int> currentChild)
        {
            currentParrent.AddNewChild(currentChild);
            currentChild.SetParent(currentParrent);
        }

        private static Tree<int> GetTree(Dictionary<int, Tree<int>> trees, int value)
        {
            if (!trees.ContainsKey(value))
            {
                trees[value] = new Tree<int>(value);
            }
            return trees[value];
        }
    }
}
