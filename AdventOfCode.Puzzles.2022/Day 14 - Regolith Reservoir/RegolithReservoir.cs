namespace AdventOfCode.Puzzles._2022.Day_14___Regolith_Reservoir
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using System.Text;

    public class RegolithReservoir
    {
        private const int SourceX = 500;

        private static readonly Vector<int> Source = new(SourceX, 0);

        public RegolithReservoir(string[] input, bool hasFloor)
        {
            this.Map = Parse(input, hasFloor, out int renderLeft, out int renderRight);
            this.RenderLeft = renderLeft;
            this.RenderRight = renderRight;
        }

        public RegolithReservoir(string[] input, bool hasFloor, IFrameRenderer renderer)
            : this(input, hasFloor)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private VectorArray<int, State> Map { get; set; }

        private int RenderLeft { get; set; }

        private int RenderRight { get; set; }

        public int SumOfSand()
            => this.Map.AxisEnumerator().Sum(x => x.Value == State.Sand ? 1 : 0);

        public RegolithReservoir Run()
        {
            while (this.DropSand())
            {
            }

            return this;
        }

        public RegolithReservoir RenderSilver(int renderEvery = 2)
            => this.Render(hasFloor: false, renderEvery);

        public RegolithReservoir RenderGold(int renderEvery = 8)
            => this.Render(hasFloor: true, renderEvery);

        private bool DropSand()
        {
            if (this.Map[0, SourceX] == State.Sand)
            {
                return false;
            }

            Vector<int> current = Source;

            while (true)
            {
                Vector<int>? next = this.Next(current);

                if (next == null)
                {
                    this.Map[current.Y, current.X] = State.Sand;
                    return true;
                }

                current = next.Clone();

                if (current.Y + 1 >= this.Map.Height ||
                    current.X <= 0 ||
                    current.X + 1 >= this.Map.Width)
                {
                    return false;
                }
            }
        }

        private RegolithReservoir Render(bool hasFloor, int renderEvery)
        {
            if (this.Renderer == null)
            {
                return this.Run();
            }

            int sand = 0;
            int frame = 0;

            while (true)
            {
                if (this.Map[0, SourceX] == State.Sand)
                {
                    this.Renderer.RenderFrame(new Frame(
                        this.BuildFrame(
                            Source,
                            [],
                            $"SOURCE BLOCKED // SAND {sand:00000}")));

                    return this;
                }

                Vector<int> current = Source;
                List<Vector<int>> path = [];

                while (true)
                {
                    path.Add(current);

                    if (frame++ % renderEvery == 0)
                    {
                        this.Renderer.RenderFrame(new Frame(
                            this.BuildFrame(
                                current,
                                path,
                                hasFloor
                                    ? $"REGOLITH RESERVOIR // FLOOR // SAND {sand:00000}"
                                    : $"REGOLITH RESERVOIR // ABYSS // SAND {sand:00000}")));
                    }

                    Vector<int>? next = this.Next(current);

                    if (next == null)
                    {
                        this.Map[current.Y, current.X] = State.Sand;
                        sand++;

                        this.Renderer.RenderFrame(new Frame(
                            this.BuildFrame(
                                current,
                                [],
                                hasFloor
                                    ? $"REGOLITH RESERVOIR // FLOOR // SAND {sand:00000}"
                                    : $"REGOLITH RESERVOIR // ABYSS // SAND {sand:00000}")));

                        break;
                    }

                    current = next.Clone();

                    if (!hasFloor &&
                        (current.Y + 1 >= this.Map.Height ||
                         current.X <= 0 ||
                         current.X + 1 >= this.Map.Width))
                    {
                        this.Renderer.RenderFrame(new Frame(
                            this.BuildFrame(
                                current,
                                path,
                                $"SAND FALLS INTO THE ABYSS // RESTED {sand:00000}")));

                        return this;
                    }
                }
            }
        }

        private Vector<int>? Next(Vector<int> point)
        {
            Vector<int>[] options =
            [
                new(point.X, point.Y + 1),
                new(point.X - 1, point.Y + 1),
                new(point.X + 1, point.Y + 1)
            ];

            foreach (Vector<int> option in options)
            {
                if (option.Y >= this.Map.Height ||
                    option.X < 0 ||
                    option.X >= this.Map.Width)
                {
                    return option;
                }

                if (this.Map[option.Y, option.X] == State.Air)
                {
                    return option;
                }
            }

            return null;
        }

        private string[] BuildFrame(
            Vector<int> current,
            List<Vector<int>> path,
            string title)
        {
            List<string> result = [];

            result.Add(title);
            result.Add(string.Empty);

            HashSet<Vector<int>> trail = [.. path];

            for (int y = 0; y < this.Map.Height; y++)
            {
                StringBuilder sb = new();

                for (int x = this.RenderLeft; x <= this.RenderRight; x++)
                {
                    Vector<int> point = new(x, y);

                    if (point == current)
                    {
                        sb.Append('@');
                    }
                    else if (point == Source && this.Map[y, x] != State.Sand)
                    {
                        sb.Append('+');
                    }
                    else if (trail.Contains(point))
                    {
                        sb.Append('~');
                    }
                    else
                    {
                        sb.Append(this.Map[y, x] switch
                        {
                            State.Rock => '#',
                            State.Sand => 'o',
                            _ => '.'
                        });
                    }
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private static VectorArray<int, State> Parse(
            string[] input,
            bool hasFloor,
            out int renderLeft,
            out int renderRight)
        {
            List<List<Vector<int>>> rocks = [];
            int width = SourceX + 1;
            int height = 0;
            int minX = SourceX;
            int maxX = SourceX;

            for (int i = 0; i < input.Length; i++)
            {
                string[] tokens = input[i].Split(" -> ");

                rocks.Add([]);

                foreach (string token in tokens)
                {
                    int[] coordinates = token.Split(",").ToInt();

                    minX = Math.Min(minX, coordinates[0]);
                    maxX = Math.Max(maxX, coordinates[0]);
                    width = Math.Max(width, coordinates[0] + 1);
                    height = Math.Max(height, coordinates[1] + 1);

                    rocks[i].Add(new(coordinates));
                }
            }

            if (hasFloor)
            {
                int floorY = height + 1;
                height += 2;

                int spread = floorY + 2;
                renderLeft = SourceX - spread;
                renderRight = SourceX + spread;

                width = Math.Max(width, renderRight + 1);

                rocks.Add([new(0, floorY), new(width - 1, floorY)]);
            }
            else
            {
                renderLeft = Math.Max(0, minX - 4);
                renderRight = Math.Min(width - 1, maxX + 4);
            }

            VectorArray<int, State> result = new(width, height);

            foreach (List<Vector<int>> rock in rocks)
            {
                for (int i = 0; i < rock.Count - 1; i++)
                {
                    Vector<int> a = rock[i];
                    Vector<int> b = rock[i + 1];

                    if (a.X == b.X)
                    {
                        for (long y = Math.Min(a.Y, b.Y); y <= Math.Max(a.Y, b.Y); y++)
                        {
                            result[y, a.X] = State.Rock;
                        }
                    }
                    else
                    {
                        for (long x = Math.Min(a.X, b.X); x <= Math.Max(a.X, b.X); x++)
                        {
                            result[a.Y, x] = State.Rock;
                        }
                    }
                }
            }

            return result;
        }

        public void PrintCave()
        {
            Console.Clear();

            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = this.RenderLeft; x <= this.RenderRight; x++)
                {
                    switch (this.Map[y, x])
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
    }
}