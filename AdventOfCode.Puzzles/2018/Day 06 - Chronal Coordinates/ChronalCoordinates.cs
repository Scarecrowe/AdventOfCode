namespace AdventOfCode.Puzzles._2018.Day_06___Chronal_Coordinates
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class ChronalCoordinates
    {
        public ChronalCoordinates(string[] input)
        {
            this.Coordinates = new();
            this.Locations = new();
            this.Map = new();

            this.Parse(input);
        }

        public Dictionary<char, Vector<int>> Coordinates { get; private set; }

        public List<Vector<int>> Locations { get; private set; }

        public VectorDictionary<int, char> Map { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int Dangerous()
        {
            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    Vector<int> point = new(x, y);
                    Dictionary<char, int> distances = this.DistanceToCoordinates(point);
                    int min = distances.Min(c => c.Value);

                    if (distances.Count(c => c.Value == min) == 1)
                    {
                        this.Map.Add(point, distances.FirstOrDefault(c => c.Value == min).Key);
                    }
                    else
                    {
                        this.Map.Add(point, '.');
                    }
                }
            }

            int result = 0;

            foreach (KeyValuePair<char, Vector<int>> coordinate in this.Coordinates)
            {
                if (!this.Map.IsEdge(coordinate.Key))
                {
                    result = Math.Max(result, this.Map.Count(x => x.Value == coordinate.Key));
                }
            }

            return result;
        }

        public int Safe(int distance)
        {
            int max = distance / 25;
            int min = max * -1;
            int count = 0;

            for (int y = min; y < max; y++)
            {
                for (int x = min; x < max; x++)
                {
                    if (this.Locations.Sum(point => new Vector<int>(x, y).Distance(point)) < distance)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public ChronalCoordinates Print()
        {
            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    PuzzleConsole.Write(this.Map[new(x, y)]);
                }

                PuzzleConsole.WriteLine();
            }

            PuzzleConsole.WriteLine();

            return this;
        }

        private void Parse(string[] input)
        {
            this.Map = new();
            this.Coordinates = new();
            this.Locations = new();

            int letter = 'A';

            foreach (string line in input)
            {
                int[] coordinates = line.Split(", ").ToInt();
                this.Coordinates.Add((char)letter, new(coordinates));
                this.Width = Math.Max(this.Width, coordinates[0]);
                this.Height = Math.Max(this.Height, coordinates[1]);
                this.Map.Add(new(coordinates), (char)letter);
                this.Locations.Add(new(coordinates));
                letter++;
            }
        }

        private Dictionary<char, int> DistanceToCoordinates(Vector<int> point)
        {
            Dictionary<char, int> result = new();

            foreach (KeyValuePair<char, Vector<int>> coordinate in this.Coordinates)
            {
                result.Add(coordinate.Key, (int)point.Distance(coordinate.Value));
            }

            return result;
        }
    }
}