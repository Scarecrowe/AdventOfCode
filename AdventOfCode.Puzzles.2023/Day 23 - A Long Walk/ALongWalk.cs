using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Puzzles._2023.Day_23___A_Long_Walk
{
    public class ALongWalk
    {
        private readonly char[,] map;
        private readonly int rows;
        private readonly int cols;

        private readonly (int dx, int dy, char symbol)[] directions =
        {
            (-1, 0, '^'),
            (1, 0, 'v'),
            (0, -1, '<'),
            (0, 1, '>')
        };

        public ALongWalk(string[] input)
        {
            rows = input.Length;
            cols = input[0].Length;
            map = new char[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    map[r, c] = input[r][c];
                }
            }                
        }

        public int LongestHike() => Solve(true);

        public int UniqueLongestHike() => Solve(false);

        private int Solve(bool obeySlopes)
        {
            int maxLength = 0;

            for (int c = 0; c < cols; c++)
            {
                if (map[0, c] != '.')
                {
                    continue;
                }

                var stack = new Stack<(int row, int col, bool[,] visited, int length)>();
                stack.Push((0, c, new bool[rows, cols], 0));

                while (stack.Count > 0)
                {
                    var (row, col, visited, length) = stack.Pop();
                    visited[row, col] = true;
                    maxLength = Math.Max(maxLength, length);

                    foreach (var (dx, dy, symbol) in directions)
                    {
                        int newRow = row + dx;
                        int newCol = col + dy;

                        if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols)
                        {
                            continue;
                        }

                        if (visited[newRow, newCol])
                        {
                            continue;
                        }

                        char tile = map[newRow, newCol];

                        if (tile == '.' || !obeySlopes || (obeySlopes && tile == symbol))
                        {
                            var newVisited = (bool[,])visited.Clone();
                            stack.Push((newRow, newCol, newVisited, length + 1));
                        }
                    }
                }
            }

            return maxLength;
        }
    }
}
