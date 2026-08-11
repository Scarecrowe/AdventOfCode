namespace AdventOfCode.Runner
{
    using System.Diagnostics;
    using System.Windows.Forms;
    using AdventOfCode.Core;

    public class PuzzleRunner
    {
        public PuzzleRunner(string[] args)
        {
            this.Args = new CommandArguments(args);

            if (!this.Args.Valid)
            {
                PuzzleMenu menu = new();
                menu.Execute();
                return;
            }

            PrintTree(13);
            IPuzzle? puzzle = Puzzle.GetPuzzle(this.Args.Year, this.Args.Day);

            if (puzzle == null)
            {
                PuzzleConsole.WriteLine($"No puzzle found with the year: {this.Args.Year} and day: {this.Args.Day}");
                return;
            }

            RunPuzzle(puzzle, this.Args.Iterations);

            Console.ReadLine();
        }

        private ICommandArguments Args { get; }

        public static async Task RunAsync(
            IPuzzle puzzle,
            int iterations = 1,
            bool printTitle = true,
            CancellationToken cancellationToken = default)
        {
            string silver = string.Empty;
            string gold = string.Empty;
            PuzzleTimer timer = new();

            SetupProcess();
            CollectAndFinalize();

            if (printTitle)
            {
                PrintTitle(puzzle);
            }

            for (int i = 1; i <= iterations; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                IPuzzle freshPuzzle = GetFreshPuzzle(puzzle);

                timer.Restart();
                silver = freshPuzzle.Silver() ?? string.Empty;
                timer.Stop();

                if (i != iterations)
                {
                    PuzzleConsole.Clear();
                    await Task.Yield();
                }
            }

            PrintResult($"Silver: {silver}", timer);

            timer.Reset();

            for (int i = 1; i <= iterations; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                IPuzzle freshPuzzle = GetFreshPuzzle(puzzle);

                timer.Restart();
                gold = freshPuzzle.Gold() ?? string.Empty;
                timer.Stop();

                if (i > 1 && string.IsNullOrEmpty(gold))
                {
                    break;
                }

                if (i != iterations)
                {
                    PuzzleConsole.Clear();
                    await Task.Yield();
                }
            }

            PrintResult($"Gold: {gold}", timer);

            Console.WriteLine($" Executed {(iterations > 1 ? $"{iterations} times" : "once")}:");

            CopyResultToClipboard(string.IsNullOrEmpty(gold) ? silver : gold);
        }

        public static Task<string> RunSilverAsync(
            IPuzzle puzzle,
            int executions = 1,
            CancellationToken cancellationToken = default)
        {
            string silver = string.Empty;
            PuzzleTimer timer = new();

            SetupProcess();
            CollectAndFinalize();

            for (int i = 1; i <= executions; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                IPuzzle freshPuzzle = GetFreshPuzzle(puzzle);

                timer.Restart();
                silver = freshPuzzle.Silver() ?? string.Empty;
                timer.Stop();

                if (i != executions)
                {
                    PuzzleConsole.Clear();
                }
            }

            PrintResult($"Silver: {silver}", timer);

            PuzzleConsole.WriteLine($"Executed {(executions > 1 ? $"{executions} times" : "once")}:");

            return Task.FromResult(silver);
        }

        public static Task<string> RunGoldAsync(
           IPuzzle puzzle,
           int executions = 1,
           CancellationToken cancellationToken = default)
        {
            string gold = string.Empty;
            PuzzleTimer timer = new();

            SetupProcess();
            CollectAndFinalize();

            for (int i = 1; i <= executions; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                IPuzzle freshPuzzle = GetFreshPuzzle(puzzle);

                timer.Restart();
                gold = freshPuzzle.Gold() ?? string.Empty;
                timer.Stop();

                if (i > 1 && string.IsNullOrEmpty(gold))
                {
                    break;
                }

                if (i != executions)
                {
                    PuzzleConsole.Clear();
                }
            }

            PrintResult($"Gold: {gold}", timer);

            PuzzleConsole.WriteLine($"Executed {(executions > 1 ? $"{executions} times" : "once")}:");

            return Task.FromResult(gold);
        }

        public static void RunPuzzle(IPuzzle puzzle, int iterations = 1)
        {
            string silver = string.Empty;
            string gold = string.Empty;
            PuzzleTimer timer = new();

            SetupProcess();
            CollectAndFinalize();
            PrintTitle(puzzle);

            for (int i = 1; i <= iterations; i++)
            {
                IPuzzle freshPuzzle = GetFreshPuzzle(puzzle);

                timer.Restart();
                silver = freshPuzzle.Silver() ?? string.Empty;
                timer.Stop();

                if (i != iterations)
                {
                    PuzzleConsole.Clear();
                }
            }

            PrintResult($"Silver: {silver}", timer);

            timer.Reset();

            for (int i = 1; i <= iterations; i++)
            {
                IPuzzle freshPuzzle = GetFreshPuzzle(puzzle);

                timer.Restart();
                gold = freshPuzzle.Gold() ?? string.Empty;
                timer.Stop();

                if (i > 1 && string.IsNullOrEmpty(gold))
                {
                    break;
                }

                if (i != iterations)
                {
                    PuzzleConsole.Clear();
                }
            }

            PrintResult($"Gold: {gold}", timer);

            Console.WriteLine($" Executed {(iterations > 1 ? $"{iterations} times" : "once")}:");

            CopyResultToClipboard(string.IsNullOrEmpty(gold) ? silver : gold);
        }

        public static void PrintTree(int count = 1)
        {
            string[] tokens = new string[9];
            tokens[0] = @"         ";
            tokens[1] = @"    *    ";
            tokens[2] = @"   /.\   ";
            tokens[3] = @"  /..'\  ";
            tokens[4] = @"  /'.'\  ";
            tokens[5] = @" /.''.'\ ";
            tokens[6] = @" /.'.'.\ ";
            tokens[7] = @"/'.''.'.\";
            tokens[8] = @"^^^[_]^^^";

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    if (j == 0)
                    {
                        Console.Write(" ");
                    }

                    Console.Write(tokens[i]);
                }

                Console.WriteLine();
            }
        }

        private static IPuzzle GetFreshPuzzle(IPuzzle puzzle)
        {
            IPuzzle? freshPuzzle = Puzzle.GetPuzzle(puzzle.Year, puzzle.Day);

            if (freshPuzzle == null)
            {
                throw new InvalidOperationException($"No puzzle found with the year: {puzzle.Year} and day: {puzzle.Day}");
            }

            return freshPuzzle;
        }

        private static void SetupProcess()
        {
            Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(1);
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
        }

        private static void CollectAndFinalize()
        {
            GC.Collect();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private static void PrintResult(string result, PuzzleTimer timer)
        {
            Console.WriteLine($" {result}");
            Console.WriteLine();
            PuzzleConsole.Flush();
            Console.WriteLine($" \tAvg: {Math.Round(timer.Average(), 4, MidpointRounding.AwayFromZero)}ms");
            Console.WriteLine($" \tMin: {Math.Round(timer.Min, 4, MidpointRounding.AwayFromZero)}ms");
            Console.WriteLine($" \tMax: {Math.Round(timer.Max, 4, MidpointRounding.AwayFromZero)}ms");
            Console.WriteLine();
        }

        private static void CopyResultToClipboard(string result)
        {
            if (string.IsNullOrEmpty(result))
            {
                return;
            }

            Thread thread = new(() => Clipboard.SetText(result));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        private static void PrintTitle(IPuzzle puzzle)
        {
            Console.WriteLine();
            Console.WriteLine($" {$"--- Advent Of Code {puzzle.Year} Day {puzzle.Day}: {puzzle.DayTitle}"} ---");
            Console.WriteLine();
        }
    }
}