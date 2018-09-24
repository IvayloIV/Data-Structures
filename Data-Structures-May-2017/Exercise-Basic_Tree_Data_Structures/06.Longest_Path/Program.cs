using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06.Longest_Path
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
            var root = trees.Where(a => a.Value.Parent == null).FirstOrDefault().Value;
            FindAndPrintLongestPath(root);
        }

        private static void FindAndPrintLongestPath(Tree<int> root)
        {
            Tree<int> longestTree = root;
            var queue = new Queue<Tree<int>>();
            root.SetHeight(1);
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var currentTree = queue.Dequeue();
                var currentHeight = currentTree.Height;

                foreach (var child in currentTree.Children)
                {
                    child.SetHeight(currentHeight + 1);
                    queue.Enqueue(child);
                }

                if (currentHeight > longestTree.Height)
                {
                    longestTree = currentTree;
                }
            }

            PrintLongestPath(longestTree);
        }

        private static void PrintLongestPath(Tree<int> longestTree)
        {
            var path = new List<Tree<int>>();
            while (longestTree != null)
            {
                path.Add(longestTree);
                longestTree = longestTree.Parent;
            }

            path.Reverse();
            Console.WriteLine($"Longest path: {string.Join(" ", path.Select(a => a.Value))}");
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
