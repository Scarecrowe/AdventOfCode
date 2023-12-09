namespace AdventOfCode.Puzzles._2023.Day_09___Mirage_Maintenance
{
    using AdventOfCode.Core.Extensions;

    public class MirageMaintenance
    {
        public MirageMaintenance(string[] input)
            => this.Report = input.Select(x => x.Split(" ").ToInt()).ToList();

        public List<int[]> Report { get; }

        public int End()
            => this.ExtrapolateHistory((history, result) => history[^1] + result);

        public int Beginning()
            => this.ExtrapolateHistory((history, result) => history[0] - result);

        private static int Predict(int[] history, Func<int[], int, int> func)
            => history.Length == 0 ? 0 : func(history, Predict(Diff(history), func));

        private static int[] Diff(int[] history)
            => history[0..^1].Select((x, i) => history[i + 1] - x).ToArray();

        private int ExtrapolateHistory(Func<int[], int, int> func)
            => this.Report.Aggregate(0, (total, history) => total += Predict(history, func));
    }
}
