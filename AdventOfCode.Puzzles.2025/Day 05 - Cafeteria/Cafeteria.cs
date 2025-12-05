namespace AdventOfCode.Puzzles._2025.Day_05___Cafeteria
{
    public class Cafeteria(string[] input)
    {
        private List<IngredientRange> Fresh { get; set; } = ParseFresh(input);

        private List<long> Available { get; set; } = ParseAvailable(input);

        private static List<IngredientRange> ParseFresh(string[] input)
        {
            return input
               .Take(Array.IndexOf(input, ""))
               .Select(line =>
               {
                   string[] tokens = line.Split('-');
                   return new IngredientRange
                   {
                       Start = long.Parse(tokens[0]),
                       End = long.Parse(tokens[1])
                   };
               })
               .ToList();
        }

        private static List<long> ParseAvailable(string[] input)
        {
            return input
                .Skip(Array.IndexOf(input, "") + 1)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(long.Parse)
                .ToList();
        }

        private List<IngredientRange> Merge(List<IngredientRange> ranges)
        {
            List<IngredientRange> sorted =
            [
                .. ranges
                .OrderBy(r => r.Start)
                .ThenBy(r => r.End)
,
            ];

            List<IngredientRange> merged = new();

            foreach (var range in sorted)
            {
                if (!merged.Any())
                {
                    merged.Add(new IngredientRange { Start = range.Start, End = range.End });
                    continue;
                }

                IngredientRange last = merged.Last();

                if (range.Start <= last.End + 1)
                {
                    last.End = Math.Max(last.End, range.End);
                }
                else
                {
                    merged.Add(new IngredientRange { Start = range.Start, End = range.End });
                }
            }

            return merged;
        }

        public long AvailableCount() => this.Available.Count(id => this.Fresh.Any(r => r.Contains(id)));

        public long FreshCount() => Merge(this.Fresh).Sum(r => r.End - r.Start + 1);
    }
}
