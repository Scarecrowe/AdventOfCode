namespace AdventOfCode.Puzzles._2025.Day_04___Printing_Department
{
    using AdventOfCode.Core;

    public class PrintingDepartment(string[] input)
    {
        private VectorArray<long, char> Map { get; set; } = new(input, (c) => c);

        public long AccessableRolls()
        {
            long result = 0;

            foreach (var cell in this.Map.AxisEnumerator())
            {
                if (cell.Value != '@')
                {
                    continue;
                }

                if (this.Map.AdjacentInterCardinal(cell.Point).Count(x => x.Value == '@') < 4)
                {
                    result++;
                }
            }

            return result;
        }

        public long AllAccessableRolls()
        {
            long result = 0;
            List<Vector<long>> rolls = [];

            while (true)
            {
                rolls.Clear();

                foreach (var cell in this.Map.AxisEnumerator())
                {
                    if (cell.Value != '@')
                    {
                        continue;
                    }

                    if (this.Map.AdjacentInterCardinal(cell.Point).Count(x => x.Value == '@') < 4)
                    {
                        rolls.Add(cell.Point);
                    }
                }

                if (rolls.Count == 0)
                {
                    break;
                }

                foreach (var point in rolls)
                {
                    this.Map[point] = '.';
                }

                result += rolls.Count;
            }

            return result;
        }
    }
}
