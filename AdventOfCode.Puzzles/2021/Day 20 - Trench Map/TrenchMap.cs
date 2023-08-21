namespace AdventOfCode.Puzzles._2021.Day_20___Trench_Map
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class TrenchMap
    {
        public TrenchMap(string[] input)
        {
            this.Algorithm = Array.Empty<bool>();
            this.Map = new();
            this.IsSample = false;
            this.Parse(input);
        }

        public bool[] Algorithm { get; private set; }

        public VectorDictionary<int, int> Map { get; private set; }

        public bool IsSample { get; private set; }

        public int EnhanceImage(int steps)
        {
            for (int step = 0; step < steps; step++)
            {
                this.Map = this.EnhanceImage(this.Map, step);
            }

            return this.Map.Count;
        }

        private static HashSet<Vector<int>> Adjacent(Vector<int> pixel) => new (int x, int y)[] { (-1, -1), (0, -1), (1, -1), (-1, 0), (0, 0), (1, 0), (-1, 1), (0, 1), (1, 1) }.Select(x => new Vector<int>(x.x, x.y) + pixel).ToHashSet();

        private static bool IsPixel(Vector<int> pixel, Vector<int> min, Vector<int> max)
            => pixel.X >= min.X && pixel.X <= max.X && pixel.Y >= min.Y && pixel.Y <= max.Y;

        private void Parse(string[] input)
        {
            bool algorithm = true;

            for (int y = 0; y < input.Length; y++)
            {
                string line = input[y];

                if (string.IsNullOrEmpty(line))
                {
                    algorithm = false;
                    this.Map = new();

                    continue;
                }

                if (algorithm)
                {
                    this.IsSample = line[0] != '#';
                    this.Algorithm = new bool[line.Length];
                    this.Algorithm = line.Select(x => x == '#').ToArray();
                }
                else
                {
                    for (int x = 0; x < line.Length; x++)
                    {
                        if (line[x] == '#')
                        {
                            this.Map.Add(new(x, y + 2), 1);
                        }
                    }
                }
            }
        }

        private int AlgorithmIndex(Vector<int> pixel, Vector<int> min, Vector<int> max, int step, VectorDictionary<int, int> map)
        {
            List<bool> bits = Adjacent(pixel).Select(
                adjacent => step % 2 == 0
                    ? map.ContainsKey(adjacent)
                    : (!IsPixel(adjacent, min, max)
                    && !this.IsSample)
                    || map.ContainsKey(adjacent)).ToList();

            return bits.Select(x => x ? '1' : '0').Join().ToInt(2);
        }

        private VectorDictionary<int, int> EnhanceImage(VectorDictionary<int, int> map, int step)
        {
            VectorDictionary<int, int> result = new();
            Vector<int> min = new(map.Min(x => x.Key.X), map.Min(x => x.Key.Y));
            Vector<int> max = new(map.Max(x => x.Key.X), map.Max(x => x.Key.Y));

            for (long y = min.Y - 1; y <= max.Y + 1; ++y)
            {
                for (long x = min.X - 1; x <= max.X + 1; ++x)
                {
                    if (this.Algorithm[this.AlgorithmIndex(new(x, y), min, max, step, map)])
                    {
                        result.Add(new(x, y), 1);
                    }
                }
            }

            return result;
        }
    }
}
