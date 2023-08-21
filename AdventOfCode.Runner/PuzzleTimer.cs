namespace AdventOfCode.Runner
{
    using System.Diagnostics;

    public class PuzzleTimer
    {
        public PuzzleTimer()
        {
            this.Watch = new();
            this.Runs = new();
            this.Reset();
        }

        public double Min { get; private set; }

        public double Max { get; private set; }

        private Stopwatch Watch { get; }

        private List<double> Runs { get; set; }

        public void Reset()
        {
            this.Runs.Clear();
            this.Min = double.MaxValue;
            this.Max = 0;
        }

        public void Restart() => this.Watch.Restart();

        public void Stop()
        {
            this.Watch.Stop();
            this.Runs.Add(this.Watch.Elapsed.TotalMilliseconds);

            this.Min = Math.Min(this.Min, this.Watch.Elapsed.TotalMilliseconds);
            this.Max = Math.Max(this.Max, this.Watch.Elapsed.TotalMilliseconds);
        }

        public double Average() => this.Runs.Average();
    }
}
