namespace AdventOfCode.Puzzles._2023.Day_03___Gear_Ratios
{
    using AdventOfCode.Core;

    public class GearRatios
    {
        public GearRatios(string[] input)
        {
            this.Map = new(input, new Func<char, char>(x => x));
            this.Part = new();
            this.Parts = new();
            this.Parse();
        }

        public VectorArray<int, char> Map { get; }

        private List<Part> Part { get; }

        private List<(string Number, char Symbol, Vector<int> Point)> Parts { get; }

        public int Count() => this.Parts.Sum(x => int.Parse(x.Number));

        public int Ratio()
        {
            int result = 0;

            foreach(var group in this.Parts.GroupBy(x => x.Point).Where(x => x.Count() > 1))
            {
                result += int.Parse(group.ElementAt(0).Number) * int.Parse(group.ElementAt(1).Number);
            }

            return result;
        }

        private void Parse()
        {
            int y = 0;

            foreach (VectorCell<int, char> cell in this.Map.AxisEnumerator())
            {
                if (cell.Point.Y > y)
                {
                    y = cell.Point.Y;

                    this.TryAddPart();
                }

                if (char.IsDigit(cell.Value))
                {
                    this.Part.Add(new(cell.Point, cell.Value));
                    continue;
                }

                this.TryAddPart();
            }

            this.TryAddPart();
        }

        private void TryAddPart()
        {
            if (this.Part.Any())
            {
                foreach (Part number in this.Part)
                {
                    IEnumerable<VectorCell<int, char>> adjacent = this.Map.AdjacentInterCardinal(number.Point);

                    if (adjacent.Any(x => x.Value != '.' && !char.IsDigit(x.Value)))
                    {
                        VectorCell<int, char>? symbol = adjacent.FirstOrDefault(x => x.Value != '.' && !char.IsDigit(x.Value));

                        if (symbol != null)
                        {
                            this.Parts.Add((string.Join(string.Empty, this.Part.Select(x => x.Value)), symbol.Value, symbol.Point));
                            break;
                        }
                    }
                }
            }

            this.Part.Clear();
        }
    }
}
