namespace AdventOfCode.Puzzles._2022.Day_14___Regolith_Reservoir
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class RegolithReservoir
    {
        public RegolithReservoir(string[] input, bool hasFloor) => this.Map = Parse(input, hasFloor);

        private VectorArray<int, State> Map { get; set; }

        public int SumOfSand() => this.Map.AxisEnumerator().Sum(x => x.Value == State.Sand ? 1 : 0);

        public RegolithReservoir Run()
        {
            Queue<Vector<int>> queue = new();
            Vector<int> reservoir = new(500, 0);
            queue.Enqueue(reservoir);

            int count = 0;

            while (queue.Count > 0)
            {
                count++;

                Vector<int> current = queue.Dequeue();

                if (current == reservoir && this.Map[0, 500] == State.Sand)
                {
                    return this;
                }

                Vector<int> point = this.MoveSouth(current);

                if (point.Y + 1 >= this.Map.Height || point.X + 1 >= this.Map.Width)
                {
                    break;
                }

                if (this.Map[point.Y + 1, point.X - 1] == State.Air)
                {
                    queue.Enqueue(new(point.X - 1, point.Y + 1));
                    continue;
                }

                if (point.X + 1 < this.Map.Width && this.Map[point.Y + 1, point.X + 1] == State.Air)
                {
                    queue.Enqueue(new(point.X + 1, point.Y + 1));
                    continue;
                }

                this.Map[point.Y, point.X] = State.Sand;
                queue.Enqueue(reservoir);
            }

            return this;
        }

        public void PrintCave()
        {
            Console.Clear();

            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = 450; x < this.Map.Width; x++)
                {
                    switch (this.Map[x, y])
                    {
                        case State.Air:
                            PuzzleConsole.Write(".");
                            break;

                        case State.Rock:
                            PuzzleConsole.Write("#");
                            break;
                        case State.Sand:
                            PuzzleConsole.Write("o");
                            break;
                    }
                }

                PuzzleConsole.WriteLine();
            }

            PuzzleConsole.WriteLine();
        }

        private static VectorArray<int, State> Parse(string[] input, bool hasFloor)
        {
            List<List<Vector<int>>> rocks = new();
            int width = 0;
            int height = 0;

            for (int i = 0; i < input.Length; i++)
            {
                string[] tokens = input[i].Split(" -> ");

                rocks.Add(new());

                foreach (string token in tokens)
                {
                    int[] coordinates = token.Split(",").ToInt();

                    if (coordinates[0] > width)
                    {
                        width = coordinates[0];
                    }

                    if (coordinates[1] > height)
                    {
                        height = coordinates[1];
                    }

                    rocks[i].Add(new(coordinates));
                }
            }

            if (hasFloor)
            {
                long maxRock = rocks.Max(x => x.Max(y => y.Y)) + 2;

                rocks.Add(new());
                var rock = rocks.Last();

                for (int x = 0; x < 1000; x++)
                {
                    rock.Add(new(x, maxRock));
                }

                width = 1000;
                height += 3;
            }
            else
            {
                width++;
                height++;
            }

            VectorArray<int, State> result = new(width, height);

            foreach (List<Vector<int>> rock in rocks)
            {
                for (int i = 0; i < rock.Count - 1; i++)
                {
                    long min = -1;
                    long max = -1;

                    if (rock[i].X == rock[i + 1].X)
                    {
                        if (rock[i].Y < rock[i + 1].Y)
                        {
                            min = rock[i].Y;
                            max = rock[i + 1].Y;
                        }
                        else
                        {
                            min = rock[i + 1].Y;
                            max = rock[i].Y;
                        }

                        for (long k = min; k <= max; k++)
                        {
                            result[k, rock[i].X] = State.Rock;
                        }
                    }
                    else
                    {
                        if (rock[i].X < rock[i + 1].X)
                        {
                            min = rock[i].X;
                            max = rock[i + 1].X;
                        }
                        else
                        {
                            min = rock[i + 1].X;
                            max = rock[i].X;
                        }

                        for (long k = min; k <= max; k++)
                        {
                            result[rock[i].Y, k] = State.Rock;
                        }
                    }
                }
            }

            return result;
        }

        private Vector<int> MoveSouth(Vector<int> point)
        {
            long y = point.Y;
            y++;

            while (y < this.Map.Height)
            {
                if (this.Map[y, point.X] != State.Air)
                {
                    break;
                }

                y++;
            }

            return new(point.X, y - 1);
        }
    }
}
