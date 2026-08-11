namespace AdventOfCode.Puzzles._2022.Day_23___Unstable_Diffusion
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Text;

    public class UnstableDiffusion
    {
        public UnstableDiffusion(string[] input)
        {
            this.Map = new();

            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[0].Length; x++)
                {
                    if (input[y][x] == '#')
                    {
                        this.Map.Add(new(x, y), 1);
                    }
                }
            }
        }

        public UnstableDiffusion(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        public VectorDictionary<int, int> Map { get; private set; }

        public int Round { get; private set; }

        public UnstableDiffusion PrintMap()
        {
            Vector<int> min = new(this.Map.Min(x => x.Key.X), this.Map.Min(x => x.Key.Y));
            Vector<int> max = new(this.Map.Max(x => x.Key.X), this.Map.Max(x => x.Key.Y));

            for (int y = min.Y; y <= max.Y; y++)
            {
                for (int x = min.X; x <= max.X; x++)
                {
                    if (!this.Map.ContainsKey(new(x, y)))
                    {
                        PuzzleConsole.Write(".");
                    }
                    else
                    {
                        PuzzleConsole.Write("#");
                    }
                }

                PuzzleConsole.WriteLine();
            }

            PuzzleConsole.WriteLine();

            return this;
        }

        public int EmptyGround()
        {
            Vector<int> min = new(this.Map.Min(x => x.Key.X), this.Map.Min(x => x.Key.Y));
            Vector<int> max = new(this.Map.Max(x => x.Key.X), this.Map.Max(x => x.Key.Y));

            int result = 0;

            for (int y = min.Y; y <= max.Y; y++)
            {
                for (int x = min.X; x <= max.X; x++)
                {
                    if (!this.Map.ContainsKey(new(x, y)))
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        public UnstableDiffusion Run(bool infinite = false)
        {
            Dictionary<Vector<int>, List<Vector<int>>> moves = new();
            List<Cardinal> directions = CardinalHelper.CardinalTransform<int>().Select(x => x.Key).ToList();
            this.Round = 1;

            while (true)
            {
                foreach (var elf in this.Map)
                {
                    List<VectorCell<int, int>> adjacent = this.Map.AdjacentInterCardinal(elf.Key).ToList();

                    if (adjacent.Any())
                    {
                        foreach (Cardinal cardinal in directions)
                        {
                            if (!HasGroupMove(cardinal, adjacent))
                            {
                                continue;
                            }

                            VectorCell<int, int>? cell = CardinalHelper.AllCells<int, int>().FirstOrDefault(x => x.Direction == cardinal);

                            if (moves.ContainsKey((cell?.Point ?? new(0, 0)) + elf.Key))
                            {
                                moves[(cell?.Point ?? new(0, 0)) + elf.Key].Add(elf.Key);
                            }
                            else
                            {
                                moves.Add((cell?.Point ?? new(0, 0)) + elf.Key, new() { elf.Key });
                            }

                            break;
                        }
                    }
                }

                Cardinal first = directions.First();

                if (!moves.Any())
                {
                    break;
                }

                foreach (var move in moves)
                {
                    if (move.Value.Count == 1)
                    {
                        this.Map.Remove(move.Value.First());
                        this.Map.Add(move.Key, 1);
                    }
                }

                directions.Remove(first);
                directions.Add(first);
                moves.Clear();
                this.Round++;

                if (!infinite && this.Round > 10)
                {
                    break;
                }
            }

            return this;
        }

        public UnstableDiffusion RenderSilver(int renderEvery = 1)
            => this.Render(false, renderEvery);

        public UnstableDiffusion RenderGold(int renderEvery = 4)
            => this.Render(true, renderEvery);

        private UnstableDiffusion Render(bool infinite, int renderEvery)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<(int Round, VectorDictionary<int, int> Map, bool Moved)> frames = [];
            List<Cardinal> directions = CardinalHelper.CardinalTransform<int>().Select(x => x.Key).ToList();

            int round = 0;

            frames.Add((round, CloneMap(this.Map), true));

            while (true)
            {
                Dictionary<Vector<int>, List<Vector<int>>> moves = new();

                foreach (var elf in this.Map)
                {
                    List<VectorCell<int, int>> adjacent = this.Map.AdjacentInterCardinal(elf.Key).ToList();

                    if (!adjacent.Any())
                    {
                        continue;
                    }

                    foreach (Cardinal cardinal in directions)
                    {
                        if (!HasGroupMove(cardinal, adjacent))
                        {
                            continue;
                        }

                        VectorCell<int, int>? cell = CardinalHelper.AllCells<int, int>()
                            .FirstOrDefault(x => x.Direction == cardinal);

                        Vector<int> target = elf.Key + (cell?.Point ?? new(0, 0));

                        if (!moves.ContainsKey(target))
                        {
                            moves.Add(target, []);
                        }

                        moves[target].Add(elf.Key);
                        break;
                    }
                }

                bool moved = moves.Any();

                if (!moved)
                {
                    round++;

                    if (round % renderEvery == 0 || infinite)
                    {
                        frames.Add((round, CloneMap(this.Map), false));
                    }

                    break;
                }

                foreach (var move in moves)
                {
                    if (move.Value.Count == 1)
                    {
                        this.Map.Remove(move.Value.First());
                        this.Map.Add(move.Key, 1);
                    }
                }

                Cardinal first = directions.First();
                directions.Remove(first);
                directions.Add(first);

                round++;

                if (round % renderEvery == 0 || round == 10)
                {
                    frames.Add((round, CloneMap(this.Map), true));
                }

                if (!infinite && round == 10)
                {
                    break;
                }
            }

            Vector<int> min = new(
                frames.SelectMany(f => f.Map.Keys).Min(p => p.X),
                frames.SelectMany(f => f.Map.Keys).Min(p => p.Y));

            Vector<int> max = new(
                frames.SelectMany(f => f.Map.Keys).Max(p => p.X),
                frames.SelectMany(f => f.Map.Keys).Max(p => p.Y));

            foreach ((int frameRound, VectorDictionary<int, int> frameMap, bool moved) in frames)
            {
                string title = infinite
                    ? moved
                        ? $"UNSTABLE DIFFUSION // ROUND {frameRound:0000}"
                        : $"UNSTABLE DIFFUSION // STABLE AFTER ROUND {frameRound:0000}"
                    : $"UNSTABLE DIFFUSION // ROUND {frameRound:0000} // EMPTY {EmptyGround(frameMap):0000}";

                this.Renderer.RenderFrame(new Frame(BuildFrame(frameMap, min, max, title)));
            }

            return this;
        }

        private static VectorDictionary<int, int> CloneMap(VectorDictionary<int, int> source)
        {
            VectorDictionary<int, int> result = new();

            foreach (var elf in source)
            {
                result.Add(elf.Key, elf.Value);
            }

            return result;
        }

        private static int EmptyGround(VectorDictionary<int, int> map)
        {
            Vector<int> min = new(map.Min(x => x.Key.X), map.Min(x => x.Key.Y));
            Vector<int> max = new(map.Max(x => x.Key.X), map.Max(x => x.Key.Y));

            return ((max.X - min.X + 1) * (max.Y - min.Y + 1)) - map.Count;
        }

        private static string[] BuildFrame(
            VectorDictionary<int, int> map,
            Vector<int> min,
            Vector<int> max,
            string title)
        {
            List<string> result = [];

            result.Add(title);
            result.Add(string.Empty);

            for (int y = min.Y; y <= max.Y; y++)
            {
                StringBuilder sb = new();

                for (int x = min.X; x <= max.X; x++)
                {
                    sb.Append(map.ContainsKey(new(x, y)) ? '#' : '.');
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private static bool HasGroupMove(Cardinal direction, List<VectorCell<int, int>> adjacent)
        {
            foreach (var check in CardinalHelper.CardinalGroupMap[direction])
            {
                if (adjacent.Any(x => x.Direction == check))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
