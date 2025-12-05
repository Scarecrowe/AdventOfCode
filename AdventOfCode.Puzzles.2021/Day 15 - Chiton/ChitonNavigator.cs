namespace AdventOfCode.Puzzles._2021.Day_15___Chiton
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class ChitonNavigator
    {
        public ChitonNavigator(string[] input) => this.Map = Parse(input);

        public VectorArray<int, Chiton> Map { get; private set; }

        public ChitonNavigator Enlarge()
        {
            int width = (int)Math.Sqrt(this.Map.Width * this.Map.Height);

            VectorArray<int, Chiton> enlargedMap = new((int)this.Map.Width * 5, (int)this.Map.Height * 5);

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    foreach (var cell in this.Map.AxisEnumerator())
                    {
                        Vector<int> key = new(cell.Point.X + (width * i), cell.Point.Y + (width * j));
                        enlargedMap[key] = new(new(key), ((cell.Value.Risk + i + j - 1) % 9) + 1);
                    }
                }
            }

            this.Map = enlargedMap;

            return this;
        }

        public int Navigate()
        {
            PriorityQueue<Chiton, int> queue = new();
            queue.Enqueue(this.Map[0, 0], 0);

            while (queue.Count > 0)
            {
                Chiton chiton = queue.Dequeue();

                if (chiton.Visited)
                {
                    continue;
                }

                chiton.Visited = true;

                if (chiton.Point == new Vector<int>(this.Map.Width - 1, this.Map.Height - 1))
                {
                    return this.Map[chiton.Point.Y, chiton.Point.X].TotalRisk - this.Map[0, 0].TotalRisk;
                }

                foreach (VectorCell<int, Chiton> adjacent in this.Map.AdjacentCardinal(chiton.Point))
                {
                    if (!adjacent.Value.Visited)
                    {
                        int risk = chiton.TotalRisk + this.Map[adjacent.Point].Risk;

                        if (risk < adjacent.Value.TotalRisk)
                        {
                            adjacent.Value.TotalRisk = risk;
                        }

                        if (adjacent.Value.TotalRisk != int.MaxValue)
                        {
                            queue.Enqueue(adjacent.Value, adjacent.Value.TotalRisk);
                        }
                    }
                }
            }

            return this.Map[this.Map.Height - 1, this.Map.Width - 1].TotalRisk - this.Map[0, 0].TotalRisk;
        }

        private static VectorArray<int, Chiton> Parse(string[] input)
        {
            VectorArray<int, Chiton> result = new(input[0].Length, input.Length);

            foreach (VectorCell<int, Chiton> cell in result.AxisEnumerator())
            {
                result[cell.Point] = new(new(cell.Point), input[cell.Point.Y][(int)cell.Point.X].ToString().ToInt());
            }

            return result;
        }
    }
}
