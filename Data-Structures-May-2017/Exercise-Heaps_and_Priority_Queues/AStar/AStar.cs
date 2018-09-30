using System;
using System.Collections.Generic;

public class AStar
{
    private char[,] map;
    private PriorityQueue<Node> priorityQueue;
    private Dictionary<Node, Node> parents;
    private Dictionary<Node, int> cost;

    public AStar(char[,] map)
    {
        this.map = map;
        this.parents = new Dictionary<Node, Node>();
        this.priorityQueue = new PriorityQueue<Node>();
        this.cost = new Dictionary<Node, int>();
    }

    public static int GetH(Node current, Node goal)
    {
        var diffY = Math.Abs(current.Row - goal.Row);
        var diffX = Math.Abs(current.Col - goal.Col);
        return diffX + diffY;
    }

    public IEnumerable<Node> GetPath(Node start, Node goal)
    {
        this.priorityQueue.Enqueue(start);
        this.parents[start] = null;
        this.cost[start] = 0;

        while (this.priorityQueue.Count > 0)
        {
            var currentNode = priorityQueue.Dequeue();

            if (currentNode.Equals(goal))
            {
                break;
            }

            var neighbors = CreateNeighbors(currentNode);
            foreach (var neighbor in neighbors)
            {
                var newCost = this.cost[currentNode] + 1;
                if (!this.cost.ContainsKey(neighbor) || newCost < this.cost[neighbor])
                {
                    this.cost[neighbor] = newCost;
                    neighbor.F = newCost + GetH(neighbor, goal);
                    this.priorityQueue.Enqueue(neighbor);
                    this.parents[neighbor] = currentNode;
                }
            }
        }

        return this.GetCorrectPath(start, goal);
    }

    private IEnumerable<Node> GetCorrectPath(Node start, Node goal)
    {
        var path = new Stack<Node>();
        if (!this.parents.ContainsKey(goal))
        {
            path.Push(start);
        }
        else
        {
            while (goal != null)
            {
                path.Push(goal);
                goal = this.parents[goal];
            }
        }

        return path;
    }

    private IEnumerable<Node> CreateNeighbors(Node currentNode)
    {
        var neighbots = new List<Node>();
        var row = currentNode.Row;
        var col = currentNode.Col;

        AddNewNeighbor(row - 1, col, neighbots);
        AddNewNeighbor(row, col + 1, neighbots);
        AddNewNeighbor(row + 1, col, neighbots);
        AddNewNeighbor(row, col - 1, neighbots);

        return neighbots;
    }

    private void AddNewNeighbor(int row, int col, List<Node> neightbors)
    {
        if (IsInside(row, col))
        {
            var newNeighbor = new Node(row, col);
            neightbors.Add(newNeighbor);
        }
    }

    private void FillPath(Stack<Node> path, Node lastNode)
    {
        while (this.parents.ContainsKey(lastNode))
        {
            path.Push(lastNode);
            lastNode = this.parents[lastNode];
        }
    }
    
    private bool IsInside(int row, int col)
    {
        return row >= 0 && col >= 0 && row < this.map.GetLength(0) && col < this.map.GetLength(1) 
            && this.map[row, col] != 'W';
    }
}

