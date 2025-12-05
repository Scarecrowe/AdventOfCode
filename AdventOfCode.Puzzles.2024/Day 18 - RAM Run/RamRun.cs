namespace AdventOfCode.Puzzles._2024.Day_18___RAM_Run
{
    using AdventOfCode.Core;
    using System.Linq;

    public class RamRun
    {
        public VectorArray<int, char> Ram { get; private set; }

        public HashSet<Vector<int>> Bytes { get; private set; }

        public HashSet<Vector<int>> FallenBytes { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int ByteCount { get; private set; }

        public Vector<int> Finish { get; private set; } 

        public RamRun(string[] input)
        {
            this.Width = 71;
            this.Height = 71;
            this.ByteCount = 1024;
            this.Ram = new VectorArray<int, char>(this.Width, this.Height);
            this.Bytes = input.Select(line => new Vector<int>(line.Split(",").Select(x => int.Parse(x)))).ToHashSet();
            this.FallenBytes = [];
            this.Finish = new(this.Width - 1, this.Height - 1);

            for(int i = 0; i < this.ByteCount; i++)
            {
                this.FallenBytes.Add(this.Bytes.ElementAt(i));
            }

            for(int y = 0; y < this.Height; y++) 
            {
                for (int x = 0; x < this.Width; x++)
                {
                    Vector<int> point = new(x, y);
                    this.Ram[point] = this.FallenBytes.Contains(point) ? '#' : '.';
                }
            }
        }

        public string NonReachable()
        {
            for(int i = this.ByteCount; i < this.Bytes.Count; i++)
            {
                var point = this.Bytes.ElementAt(i);
                this.Ram[point] = '#';

                if (this.ShortestPath() == 0)
                {
                    return $"{point.X},{point.Y}";
                }
            }

            return "0,0";
        }

        public int ShortestPath()
        {
            int result = 0;
            HashSet<Vector<int>> visited = [];
            Queue<(Vector<int> Point, int Distance)> queue = [];
            queue.Enqueue((new(0, 0), 0));

            while(queue.Count > 0)
            {
                var state = queue.Dequeue();

                if (visited.Contains(state.Point))
                {
                    continue;
                }

                visited.Add(state.Point);

                foreach (var cell in this.Ram.AdjacentCardinal(state.Point))
                {
                    if (cell.Value == '.')
                    {
                        if (cell.Point == this.Finish)
                        {
                            state.Distance++;

                            if (result == 0
                                || state.Distance < result)
                            {
                                result = state.Distance;
                            }

                            continue;
                        }

                        queue.Enqueue((cell.Point, state.Distance + 1));
                    }
                }
            }

            return result;
        }
    }
}
