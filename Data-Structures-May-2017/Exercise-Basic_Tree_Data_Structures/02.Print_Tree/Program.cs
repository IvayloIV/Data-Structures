﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02.Print_Tree
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
            var parent = trees.Where(a => a.Value.Parent == null).FirstOrDefault();
            parent.Value.PrintTree();
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
