using System;
using System.Collections.Generic;

namespace _07.Distance_in_Labyrinth
{
    class Program
    {
        static void Main(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            string[,] matrix = new string[n, n];
            var row = 0;
            var cow = 0;
            for (int i = 0; i < n; i++)
            {
                var tokens = Console.ReadLine().ToCharArray();
                for (int k = 0; k < tokens.Length; k++)
                {
                    matrix[i, k] = tokens[k].ToString();
                    if (tokens[k].ToString() == "*")
                    {
                        row = i;
                        cow = k;
                    }
                }
            }

            var cells = new Queue<Cell>();
            cells.Enqueue(new Cell(row, cow, 0));

            while (cells.Count != 0)
            {
                var currentCell = cells.Dequeue();
                row = currentCell.Row;
                cow = currentCell.Cow;
                var step = currentCell.Step;

                if (matrix[row, cow] != "*")
                {
                    matrix[row, cow] = step.ToString();
                }


                CheckCell(cells, matrix, row - 1, cow, step);
                CheckCell(cells, matrix, row, cow + 1, step);
                CheckCell(cells, matrix, row + 1, cow, step);
                CheckCell(cells, matrix, row, cow - 1, step);
            }

            PrintMatrix(matrix);
        }

        private static void CheckCell(Queue<Cell> cells, string[,] matrix, int row, int cow, int step)
        {
            if (IsInside(row, cow, matrix) && matrix[row, cow] == "0")
            {
                cells.Enqueue(new Cell(row, cow, step + 1));
            }
        }

        private static void PrintMatrix(string[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int k = 0; k < matrix.GetLength(1); k++)
                {
                    if (matrix[i, k] == "0")
                    {
                        Console.Write("u");
                    }
                    else
                    {
                        Console.Write(matrix[i, k]);
                    }
                }
                Console.WriteLine();
            }
        }

        private static bool IsInside(int row, int cow, string[,] matrix)
        {
            return row >= 0 && cow >= 0 && row < matrix.GetLength(0) && cow < matrix.GetLength(1);
        }
    }
}