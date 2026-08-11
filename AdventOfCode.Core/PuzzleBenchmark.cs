namespace AdventOfCode.Core
{
    using System.Diagnostics;

    public class PuzzleBenchmark
    {
        public static BenchmarkResult Benchmark(
            IPuzzle puzzle,
            string solution,
            int executions,
            Func<IPuzzle, string?> runner)
        {
            List<double> timings = [];

            Process process = Process.GetCurrentProcess();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long workingSetBefore = process.WorkingSet64;
            long privateMemoryBefore = process.PrivateMemorySize64;
            TimeSpan cpuBefore = process.TotalProcessorTime;

            int gen0Before = GC.CollectionCount(0);
            int gen1Before = GC.CollectionCount(1);
            int gen2Before = GC.CollectionCount(2);

            long allocatedBefore = GC.GetAllocatedBytesForCurrentThread();

            string answer = string.Empty;

            for (int i = 1; i <= executions; i++)
            {
                IPuzzle freshPuzzle = Puzzle.GetPuzzle(puzzle.Year, puzzle.Day)!;

                Stopwatch stopwatch = Stopwatch.StartNew();
                answer = runner(freshPuzzle) ?? string.Empty;
                stopwatch.Stop();

                timings.Add(stopwatch.Elapsed.TotalMilliseconds);
            }

            long allocatedAfter = GC.GetAllocatedBytesForCurrentThread();

            process.Refresh();

            return new BenchmarkResult
            {
                Solution = solution,
                Answer = answer,
                Executions = executions,
                AverageMs = timings.Average(),
                MinMs = timings.Min(),
                MaxMs = timings.Max(),
                AllocatedBytes = allocatedAfter - allocatedBefore,
                WorkingSetBefore = workingSetBefore,
                WorkingSetAfter = process.WorkingSet64,
                PrivateMemoryBefore = privateMemoryBefore,
                PrivateMemoryAfter = process.PrivateMemorySize64,
                CpuTime = process.TotalProcessorTime - cpuBefore,
                Gen0Collections = GC.CollectionCount(0) - gen0Before,
                Gen1Collections = GC.CollectionCount(1) - gen1Before,
                Gen2Collections = GC.CollectionCount(2) - gen2Before,
            };
        }
    }
}
