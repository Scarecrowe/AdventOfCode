namespace AdventOfCode.Puzzles._2022.Day_18___Boiling_Boulders
{
    using AdventOfCode.Core;

    public class Lava : Dictionary<Vector<int>, Cube>
    {
        public Lava(List<Vector<int>> defaultLava)
        {
            this.Min = new(defaultLava.Min(cube => cube.X) - 1, defaultLava.Min(cube => cube.Y) - 1, defaultLava.Min(cube => cube.Z) - 1);
            this.Max = new(defaultLava.Max(cube => cube.X) + 1, defaultLava.Max(cube => cube.Y) + 1, defaultLava.Max(cube => cube.Z) + 1);

            for (int x = this.Min.X; x <= this.Max.X; x++)
            {
                for (int y = this.Min.Y; y <= this.Max.Y; y++)
                {
                    for (int z = this.Min.Z; z <= this.Max.Z; z++)
                    {
                        Cube cube = new(new Vector<int>(x, y, z));
                        cube.SetType(defaultLava.Contains(cube.Point) ? CubeType.Lava : CubeType.Air);
                        this.Add(cube.Point, cube);
                    }
                }
            }
        }

        public Vector<int> Min { get; private set; }

        public Vector<int> Max { get; private set; }

        public void SortAirPockets()
        {
            List<Vector<int>> processed = new();
            Queue<Cube> queue = new();

            queue.Enqueue(new(new Vector<int>(this.Min.X, this.Min.Y, this.Min.Z)));

            while (queue.Count > 0)
            {
                Cube current = queue.Dequeue();

                if (processed.Contains(current.Point))
                {
                    continue;
                }

                this[current.Point].SetType(CubeType.None);

                foreach (Vector<int> cardinal in BoilingBoulders.Cardinals)
                {
                    Cube cube = new(current.Point + cardinal);
                    Vector<int> key = cube.Point;

                    if (this.ContainsKey(key) && this[key].Type == CubeType.Air)
                    {
                        queue.Enqueue(new(current.Point + cardinal));
                    }
                }

                processed.Add(current.Point);
            }
        }
    }
}
