namespace AdventOfCode.Runner.Rudolphs_Test_Track
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.ConsoleMenu;
    using AdventOfCode.Runner.North_Pole_Operations;

    public class RudolphsTestTrackMenu : ConsoleMenu, IConsoleMenu
    {
        public RudolphsTestTrackMenu()
            : base("Rudolph's Test Track")
        {
            this.Puzzle = default!;
            this.Executions = 10;

            this.AddMenuItems();
        }

        public RudolphsTestTrackMenu(IPuzzle puzzle)
            : base("Rudolph's Test Track")
        {
            this.Puzzle = puzzle;
            this.Executions = 10;

            this.AddMenuItems();
        }

        public IPuzzle Puzzle { get; private set; }

        private int Executions { get; set; }

        public async Task<IConsoleMenu> Execute()
        {
            if (this.Puzzle == null)
            {
                return await new RudolphsTestTrackSelectorMenu().Execute();
            }

            while (true)
            {
                this.SetSubTitle($"{this.Puzzle.Year} Day {this.Puzzle.Day} - {this.Puzzle.DayTitle ?? string.Empty}");
                this.Reset();

                this.AddMenuItems();

                IConsoleMenuItem? item = await this.WriteMenu();

                this.SetSubTitle($"{this.Puzzle.Year} Day {this.Puzzle.Day} - {this.Puzzle.DayTitle ?? string.Empty}");
                this.Reset();

                switch (item?.Key)
                {
                    case RudolphsTestTrackMenuType.BenchmarkSilver:
                        {
                            BenchmarkResult result = PuzzleBenchmark.Benchmark(
                                this.Puzzle,
                                "Silver",
                                this.Executions,
                                puzzle => puzzle.Silver());

                            WriteBenchmarkResult(result);
                            this.WaitForUser();
                            break;
                        }

                    case RudolphsTestTrackMenuType.BenchmarkGold:
                        {
                            BenchmarkResult result = PuzzleBenchmark.Benchmark(
                                this.Puzzle,
                                "Gold",
                                this.Executions,
                                puzzle => puzzle.Gold());

                            WriteBenchmarkResult(result);
                            this.WaitForUser();
                            break;
                        }

                    case RudolphsTestTrackMenuType.BenchmarkBoth:
                        {
                            BenchmarkResult silver = PuzzleBenchmark.Benchmark(
                                this.Puzzle,
                                "Silver",
                                this.Executions,
                                puzzle => puzzle.Silver());

                            BenchmarkResult gold = PuzzleBenchmark.Benchmark(
                                this.Puzzle,
                                "Gold",
                                this.Executions,
                                puzzle => puzzle.Gold());

                            WriteBenchmarkResult(silver);
                            PuzzleConsole.WriteLine();
                            WriteBenchmarkResult(gold);
                            this.WaitForUser();
                            break;
                        }

                    case RudolphsTestTrackMenuType.SetExecutionCount:
                        this.Executions = PromptInt("Executions", 10);
                        break;

                    case GenericMenu.Back:
                        return await new RudolphsTestTrackSelectorMenu().Execute();

                    case GenericMenu.MainMenu:
                        this.GotoMainMenu();
                        return new NorthPoleOperationsMenu();

                    case GenericMenu.Exit:
                        return await new ExitMenu().Execute();
                }
            }
        }

        private static void WriteBenchmarkResult(BenchmarkResult result)
        {
            PuzzleConsole.WriteLine($"{result.Solution} Benchmark");
            PuzzleConsole.WriteLine("--------------------------------------------------------------------------------");
            PuzzleConsole.WriteLine($"Answer                 : {result.Answer}");
            PuzzleConsole.WriteLine($"Executions             : {result.Executions:N0}");
            PuzzleConsole.WriteLine($"Average                : {result.AverageMs:F4}ms");
            PuzzleConsole.WriteLine($"Min                    : {result.MinMs:F4}ms");
            PuzzleConsole.WriteLine($"Max                    : {result.MaxMs:F4}ms");
            PuzzleConsole.WriteLine($"CPU Time               : {result.CpuTime}");
            PuzzleConsole.WriteLine($"Allocated              : {FormatBytes(result.AllocatedBytes)}");
            PuzzleConsole.WriteLine($"Allocated Per Run      : {FormatBytes(result.AllocatedBytes / Math.Max(result.Executions, 1))}");
            PuzzleConsole.WriteLine($"Working Set Before     : {FormatBytes(result.WorkingSetBefore)}");
            PuzzleConsole.WriteLine($"Working Set After      : {FormatBytes(result.WorkingSetAfter)}");
            PuzzleConsole.WriteLine($"Private Memory Before  : {FormatBytes(result.PrivateMemoryBefore)}");
            PuzzleConsole.WriteLine($"Private Memory After   : {FormatBytes(result.PrivateMemoryAfter)}");
            PuzzleConsole.WriteLine($"Gen 0 Collections      : {result.Gen0Collections}");
            PuzzleConsole.WriteLine($"Gen 1 Collections      : {result.Gen1Collections}");
            PuzzleConsole.WriteLine($"Gen 2 Collections      : {result.Gen2Collections}");
            PuzzleConsole.WriteLine("--------------------------------------------------------------------------------");
            PuzzleConsole.Flush();
        }

        private static string FormatBytes(long bytes)
        {
            string[] sizes =
            [
                "B",
                "KB",
                "MB",
                "GB",
                "TB",
            ];

            double value = bytes;
            int order = 0;

            while (value >= 1024 && order < sizes.Length - 1)
            {
                order++;
                value /= 1024;
            }

            return $"{value:0.##} {sizes[order]}";
        }

        private void AddMenuItems()
        {
            this.Items.Clear();

            this.Items.Add(
                RudolphsTestTrackMenuType.BenchmarkSilver,
                "Benchmark Silver",
                "Benchmark the silver solution");

            this.Items.Add(
                RudolphsTestTrackMenuType.BenchmarkGold,
                "Benchmark Gold",
                "Benchmark the gold solution");

            this.Items.Add(
                RudolphsTestTrackMenuType.BenchmarkBoth,
                "Benchmark Both",
                "Benchmark silver and gold");

            this.Items.Add(
                RudolphsTestTrackMenuType.SetExecutionCount,
                "Set Execution Count",
                $"Currently {this.Executions}");

            this.AddGenericMenuItems("Choose another puzzle");
        }
    }
}