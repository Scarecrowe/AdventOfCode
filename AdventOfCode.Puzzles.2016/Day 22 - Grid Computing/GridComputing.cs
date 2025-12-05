namespace AdventOfCode.Puzzles._2016.Day_22___Grid_Computing
{
    using AdventOfCode.Core.Extensions;

    public class GridComputing
    {
        public GridComputing(string[] input) => this.Nodes = Parse(input);

        public List<GridNode> Nodes { get; }

        public int FindAvailablePairs() => this.Nodes.Permutations(2).Sum(x => x[0].Used != 0 && x[0].Used <= x[1].Avail ? 1 : 0);

        public long MoveData()
        {
            long maxX = this.Nodes.Max(node => node.Point.X);
            long maxY = this.Nodes.Max(node => node.Point.Y);

            GridNode? start = null;
            GridNode? hole = null;

            GridNode[,] nodes = new GridNode[maxX + 1, maxY + 1];

            foreach (GridNode node in this.Nodes)
            {
                nodes[node.Point.X, node.Point.Y] = node;
            }

            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    GridNode node = nodes[x, y];

                    if (node.Used == 0)
                    {
                        hole = node;
                    }
                    else if (node.Size > 250)
                    {
                        if (start == null)
                        {
                            start = nodes[x - 1, y];
                        }
                    }
                }
            }

            if (hole == null || start == null)
            {
                throw new InvalidOperationException();
            }

            long result = hole.Point.Distance(start.Point);

            result += Math.Abs(start.Point.X - maxX) + start.Point.Y;

            return result + (5 * (maxX - 1));
        }

        private static List<GridNode> Parse(string[] input)
        {
            List<GridNode> result = new();

            for (int i = 2; i < input.Length; i++)
            {
                string[] tokens = input[i].SplitSpace();
                string[] position = tokens[0].Replace("/dev/grid/node-", string.Empty).Split("-");

                int x = position[0].Replace("x", string.Empty).ToInt();
                int y = position[1].Replace("y", string.Empty).ToInt();
                int size = tokens[1].Replace("T", string.Empty).ToInt();
                int used = tokens[2].Replace("T", string.Empty).ToInt();
                int avail = tokens[3].Replace("T", string.Empty).ToInt();
                int use = tokens[4].Replace("%", string.Empty).ToInt();

                result.Add(new(new(x, y), size, used, avail, use));
            }

            return result;
        }
    }
}
