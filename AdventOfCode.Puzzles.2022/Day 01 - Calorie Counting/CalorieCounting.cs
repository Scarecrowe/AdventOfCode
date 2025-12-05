namespace AdventOfCode.Puzzles._2022.Day_01___Calorie_Counting
{
    using AdventOfCode.Core.Extensions;

    public class CalorieCounting
    {
        public CalorieCounting(string[] input) => this.Elves = Parse(input);

        public List<int> Elves { get; }

        public int MaxCallories() => this.Elves.Max();

        public int TopThreeMaxCallories() => this.Elves.OrderByDescending(x => x).Take(3).Sum();

        private static List<int> Parse(string[] input)
        {
            List<int> result = new();
            int callories = 0;

            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    result.Add(callories);
                    callories = 0;
                    continue;
                }

                callories += line.ToInt();
            }

            if (callories > 0)
            {
                result.Add(callories);
            }

            return result;
        }
    }
}
