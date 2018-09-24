using System;
using System.Collections.Generic;
using System.Linq;

namespace _08.Subtrees_With_Given_Sum
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
            FindAndPrintAllSubTrees(root, sum);
        }

        private static void FindAndPrintAllSubTrees(Tree<int> root, int sum)
        {
            var subTrees = new List<Tree<int>>();
            DFS(root, sum, 0, subTrees);
            PrintResult(sum, subTrees);
        }

        private static int DFS(Tree<int> root, int sum, int current, List<Tree<int>> subTrees)
        {
            current = root.Value;

            foreach (var child in root.Children)
            {
                current += DFS(child, sum, current, subTrees);
            }

            if (current == sum)
            {
                subTrees.Add(root);
            }

            return current;
        }

        private static void PrintResult(int sum, List<Tree<int>> subTrees)
        {
            Console.WriteLine($"Subtrees of sum {sum}:");
            foreach (var tree in subTrees)
            {
                PrintTree(tree);
                Console.WriteLine();
            }
        }

        private static void PrintTree(Tree<int> tree)
        {
            Console.Write(tree.Value + " ");

            foreach (var child in tree.Children)
            {
                PrintTree(child);
            }
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