namespace AdventOfCode.Puzzles._2022.Day_09___Rope_Bridge
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class RopeBridge
    {
        public RopeBridge(string[] moves, int knotCount)
        {
            this.Input = moves;
            this.Knots = new();
            this.Visits = new();
            this.AddKnots(knotCount);
        }

        private List<Vector<int>> Knots { get; set; }

        private int KnotCount { get; set; }

        private List<Vector<int>> Visits { get; set; }

        private string[] Input { get; set; }

        public int Visited()
        {
            foreach ((Cardinal direction, int count) in this.Moves())
            {
                for (int i = 1; i <= count; i++)
                {
                    this.MoveHead(direction);

                    for (int j = 1; j < this.KnotCount; j++)
                    {
                        if (!IsAdjacent(this.Knots[j - 1].X, this.Knots[j - 1].Y, this.Knots[j].X, this.Knots[j].Y)
                            & !(this.Knots[j].X == this.Knots[j - 1].X && this.Knots[j].Y == this.Knots[j - 1].Y))
                        {
                            List<(long y, long x)> adjacent = Adjacent((this.Knots[j].Y, this.Knots[j].X)).ToList();

                            foreach ((int y, int x) coord in Adjacent((this.Knots[j - 1].Y, this.Knots[j - 1].X)))
                            {
                                if (adjacent.Contains(coord))
                                {
                                    this.Knots[j] = new(coord.x, coord.y);
                                    break;
                                }
                            }
                        }

                        if (!this.Visits.Contains(this.Knots[j]) && j == this.KnotCount - 1)
                        {
                            this.Visits.Add(this.Knots[j]);
                        }
                    }
                }
            }

            return this.Visits.Count;
        }

        private static bool IsAdjacent(long x, long y, long x1, long y1)
        {
            return (y1 == y - 1 && x1 == x)
                || (y1 == y + 1 && x1 == x)
                || (x1 == x + 1 && y1 == y)
                || (x1 == x - 1 && y1 == y)
                || (y1 == y - 1 && x1 == x + 1)
                || (y1 == y - 1 && x1 == x - 1)
                || (y1 == y + 1 && x1 == x + 1)
                || (y1 == y + 1 && x1 == x - 1);
        }

        private static IEnumerable<(long y, long x)> Adjacent((long y, long x) knot)
        {
            yield return (knot.y - 1, knot.x);
            yield return (knot.y + 1, knot.x);
            yield return (knot.y, knot.x + 1);
            yield return (knot.y, knot.x - 1);
            yield return (knot.y - 1, knot.x - 1);
            yield return (knot.y - 1, knot.x + 1);
            yield return (knot.y + 1, knot.x - 1);
            yield return (knot.y + 1, knot.x + 1);
        }

        private void AddKnots(int knotCount)
        {
            this.KnotCount = knotCount;
            this.Knots = new();

            for (int i = 0; i < knotCount; i++)
            {
                this.Knots.Add(new(0, 0));
            }
        }

        private IEnumerable<(Cardinal, int)> Moves()
        {
            foreach (string move in this.Input)
            {
                string[] tokens = move.SplitSpace();

                Cardinal direction;

                if (tokens[0] == "U")
                {
                    direction = Cardinal.North;
                }
                else if (tokens[0] == "D")
                {
                    direction = Cardinal.South;
                }
                else if (tokens[0] == "R")
                {
                    direction = Cardinal.East;
                }
                else
                {
                    direction = Cardinal.West;
                }

                yield return (direction, int.Parse(tokens[1]));
            }
        }

        private void MoveHead(Cardinal direction)
        {
            switch (direction)
            {
                case Cardinal.North:
                    this.Knots[0] = new(this.Knots[0].X, this.Knots[0].Y - 1);
                    break;
                case Cardinal.South:
                    this.Knots[0] = new(this.Knots[0].X, this.Knots[0].Y + 1);
                    break;
                case Cardinal.West:
                    this.Knots[0] = new(this.Knots[0].X - 1, this.Knots[0].Y);
                    break;
                case Cardinal.East:
                    this.Knots[0] = new(this.Knots[0].X + 1, this.Knots[0].Y);
                    break;
            }
        }

        private void Print()
        {
            long minY = 0;
            long minX = 0;
            long maxX = 0;
            long maxY = 0;

            foreach (Vector<int> point in this.Visits)
            {
                if (point.Y > maxY)
                {
                    maxY = point.Y;
                }

                if (point.Y < minY)
                {
                    minY = point.Y;
                }

                if (point.X > maxX)
                {
                    maxX = point.X;
                }

                if (point.X < minX)
                {
                    minX = point.X;
                }
            }

            for (long y = minY; y <= maxY; y++)
            {
                for (long x = minX; x <= maxX; x++)
                {
                    if (this.Visits.Contains(new(y, x)))
                    {
                        PuzzleConsole.Write("#");
                        continue;
                    }

                    PuzzleConsole.Write(".");
                }

                PuzzleConsole.WriteLine();
            }

            PuzzleConsole.WriteLine();
            PuzzleConsole.Flush();
        }
    }
}
