namespace AdventOfCode.Puzzles._2023.Day_09___Mirage_Maintenance
{
    using AdventOfCode.Core.Extensions;

    public class MirageMaintenance
    {
        public MirageMaintenance(string[] input)
            => this.Report = input.Select(x => x.Split(" ").ToInt()).ToList();

        public List<int[]> Report { get; }

        public int Start()
            => this.ExtrapolateHistory((history, value) => history[0] - value);

        public int End()
            => this.ExtrapolateHistory((history, value) => history[^1] + value);

        private static int[] Diff(int[] history)
            => history[0..^1].Select((x, i) => history[i + 1] - x).ToArray();

        private static int Predict(int[] history, Func<int[], int, int> result)
            => history.Length == 0 ? 0 : result(history, Predict(Diff(history), result));

        private int ExtrapolateHistory(Func<int[], int, int> result)
            => this.Report.Aggregate(0, (sum, history) => sum += Predict(history, result));
    }
}
