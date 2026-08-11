using AdventOfCode.Animation.Renderers;
using AdventOfCode.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public ALongWalk(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private readonly record struct HikeNode(int Row, int Col, int Length);

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

        public ALongWalk RenderSilver(int renderEvery = 8)
        {
            return this.RenderPath(obeySlopes: true, renderEvery);
        }

        public ALongWalk RenderGold(int renderEvery = 16)
        {
            return this.RenderPath(obeySlopes: false, renderEvery);
        }

        private ALongWalk RenderPath(bool obeySlopes, int renderEvery)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<HikeNode> path = this.FindLongestPath(obeySlopes);

            if (path.Count == 0)
            {
                this.Renderer.RenderFrame(new Frame(this.BuildFrame(
                    new HikeNode(0, this.GetStartColumn(), 0),
                    [],
                    obeySlopes ? "A LONG WALK // NO SILVER PATH FOUND" : "A LONG WALK // NO GOLD PATH FOUND")));

                return this;
            }

            HashSet<(int Row, int Col)> trail = [];

            for (int i = 0; i < path.Count; i++)
            {
                HikeNode node = path[i];
                trail.Add((node.Row, node.Col));

                if (i % renderEvery == 0 || i == path.Count - 1)
                {
                    this.Renderer.RenderFrame(new Frame(this.BuildFrame(
                        node,
                        trail,
                        obeySlopes
                            ? $"A LONG WALK // ICY SLOPES // STEPS {node.Length:0000}"
                            : $"A LONG WALK // DRY TRAILS // STEPS {node.Length:0000}")));
                }
            }

            HikeNode last = path.Last();

            for (int i = 0; i < 24; i++)
            {
                this.Renderer.RenderFrame(new Frame(this.BuildFrame(
                    last,
                    trail,
                    obeySlopes
                        ? $"LONGEST ICY HIKE // STEPS {last.Length:0000}"
                        : $"LONGEST DRY HIKE // STEPS {last.Length:0000}")));
            }

            return this;
        }

        private List<HikeNode> FindLongestPath(bool obeySlopes)
        {
            List<HikeNode> bestPath = [];
            int startCol = this.GetStartColumn();

            var stack = new Stack<(int row, int col, bool[,] visited, List<HikeNode> path)>();
            stack.Push((0, startCol, new bool[rows, cols], [new HikeNode(0, startCol, 0)]));

            while (stack.Count > 0)
            {
                var (row, col, visited, path) = stack.Pop();
                visited[row, col] = true;

                HikeNode current = path[^1];

                if (current.Length > (bestPath.Count == 0 ? -1 : bestPath[^1].Length))
                {
                    bestPath = path;
                }

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

                    if (tile != '.' && obeySlopes && tile != symbol)
                    {
                        continue;
                    }

                    if (tile == '#')
                    {
                        continue;
                    }

                    var newVisited = (bool[,])visited.Clone();
                    var newPath = new List<HikeNode>(path)
                    {
                        new(newRow, newCol, current.Length + 1)
                    };

                    stack.Push((newRow, newCol, newVisited, newPath));
                }
            }

            return bestPath;
        }

        private int GetStartColumn()
        {
            for (int c = 0; c < cols; c++)
            {
                if (map[0, c] == '.')
                {
                    return c;
                }
            }

            throw new InvalidOperationException("No start tile found in the top row.");
        }

        private int GetEndColumn()
        {
            for (int c = 0; c < cols; c++)
            {
                if (map[rows - 1, c] == '.')
                {
                    return c;
                }
            }

            throw new InvalidOperationException("No end tile found in the bottom row.");
        }

        private string[] BuildFrame(
            HikeNode current,
            HashSet<(int Row, int Col)> trail,
            string title)
        {
            const int viewportRows = 45;
            const int viewportCols = 95;

            int startCol = this.GetStartColumn();
            int endCol = this.GetEndColumn();

            int top = Math.Clamp(current.Row - viewportRows / 2, 0, Math.Max(0, rows - viewportRows));
            int left = Math.Clamp(current.Col - viewportCols / 2, 0, Math.Max(0, cols - viewportCols));
            int bottom = Math.Min(rows, top + viewportRows);
            int right = Math.Min(cols, left + viewportCols);

            List<string> result = [];

            result.Add(title);
            result.Add($"WINDOW R{top:000}-{bottom - 1:000} C{left:000}-{right - 1:000}");
            result.Add(string.Empty);

            for (int r = top; r < bottom; r++)
            {
                StringBuilder sb = new();

                for (int c = left; c < right; c++)
                {
                    if (current.Row == r && current.Col == c)
                    {
                        sb.Append('@');
                    }
                    else if (r == 0 && c == startCol)
                    {
                        sb.Append('S');
                    }
                    else if (r == rows - 1 && c == endCol)
                    {
                        sb.Append('E');
                    }
                    else if (trail.Contains((r, c)))
                    {
                        sb.Append('O');
                    }
                    else
                    {
                        sb.Append(map[r, c]);
                    }
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }
    }
}
