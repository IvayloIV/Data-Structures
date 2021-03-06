﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace _05.Deepest_Node
{
    class Program
    {
        public static int maxDepth = 0;
        public static int maxElement = 0;
        
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
            FindMaxDeepElement(root);
            Console.WriteLine($"Deepest node: {maxElement}");
        }

        private static void FindMaxDeepElement(Tree<int> tree, int currentDepth = 1)
        {
            foreach (var child in tree.Children)
            {
                FindMaxDeepElement(child, currentDepth + 1);
            }

            if (currentDepth > maxDepth)
            {
                maxElement = tree.Value;
                maxDepth = currentDepth;
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
