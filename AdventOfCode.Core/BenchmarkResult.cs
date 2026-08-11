namespace AdventOfCode.Core
{
    public class BenchmarkResult
    {
        public string Solution { get; set; } = string.Empty;

        public string Answer { get; set; } = string.Empty;

        public int Executions { get; set; }

        public double AverageMs { get; set; }

        public double MinMs { get; set; }

        public double MaxMs { get; set; }

        public long AllocatedBytes { get; set; }

        public long WorkingSetBefore { get; set; }

        public long WorkingSetAfter { get; set; }

        public long PrivateMemoryBefore { get; set; }

        public long PrivateMemoryAfter { get; set; }

        public TimeSpan CpuTime { get; set; }

        public int Gen0Collections { get; set; }

        public int Gen1Collections { get; set; }

        public int Gen2Collections { get; set; }
    }
}
