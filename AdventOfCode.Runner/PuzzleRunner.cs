namespace AdventOfCode.Runner
{
    using System.Diagnostics;
    using System.Reflection;
    using System.Text.RegularExpressions;
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
            RunPuzzle(this.Args.Year, this.Args.Day, this.Args.Iterations);

            Console.ReadLine();
        }

        private ICommandArguments Args { get; }

        public static Assembly GetPuzzleAssembly(int year)
        {
            return Assembly.LoadFrom($"AdventOfCode.Puzzles.{year}.dll");
        }

        public static List<string> GetPuzzleDays(int year)
        {
            List<string> result = new();

            IOrderedEnumerable<string?> namespaces = GetPuzzleAssembly(year)
                .GetTypes()
                .Select(t => t.Namespace)
                .Where(ns => !string.IsNullOrEmpty(ns))
                .Distinct()
                .OrderBy(@namespace => @namespace);

            foreach (string? @namespace in namespaces)
            {
                string current = @namespace?.Replace($"AdventOfCode.Puzzles._{year}.", string.Empty) ?? string.Empty;
                int day = GetDayFromNamespace(@namespace ?? string.Empty);

                if (day == -1)
                {
                    continue;
                }

                for (int i = 1; i <= 25; i++)
                {
                    current = current.Replace($"Day_{i:D2}", string.Empty);
                }

                current = current.Replace("_", " ");

                result.Add($"{day,2}. {current.Trim()}");
            }

            return result;
        }

        public static int GetDayFromNamespace(string @namespace)
        {
            Match? match = Regex.Match(@namespace, @"Day_(\d{2})");

            if (!match.Success)
            {
                return -1;
            }

            return int.Parse(match.Groups[1].Value);
        }

        public static async Task RunAsync(
            int year,
            int day,
            int iterations = 1,
            bool printTitle = true,
            CancellationToken cancellationToken = default)
        {
            string silver = string.Empty;
            string gold = string.Empty;
            PuzzleTimer timer = new();
            IPuzzle? puzzle = Puzzle.GetPuzzle(year, day);

            if (puzzle == null)
            {
                throw new InvalidOperationException($"No puzzle found with the year: {year} and day: {day}");
            }

            SetupProcess();
            CollectAndFinalize();

            if (printTitle)
            {
                PrintTitle(day, year, puzzle);
            }

            for (int i = 1; i <= iterations; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                timer.Restart();
                silver = puzzle.Silver() ?? string.Empty;
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

                timer.Restart();
                gold = puzzle.Gold() ?? string.Empty;
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

        public static async Task<string> RunSilverAsync(
            int year,
            int day,
            int executions = 1,
            CancellationToken cancellationToken = default)
        {
            string silver = string.Empty;

            PuzzleTimer timer = new();
            IPuzzle? puzzle = Puzzle.GetPuzzle(year, day);

            if (puzzle == null)
            {
                throw new InvalidOperationException($"No puzzle found with the year: {year} and day: {day}");
            }

            SetupProcess();
            CollectAndFinalize();

            for (int i = 1; i <= executions; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                timer.Restart();
                silver = puzzle.Silver() ?? string.Empty;
                timer.Stop();

                if (i != executions)
                {
                    PuzzleConsole.Clear();
                    await Task.Yield();
                }
            }

            PrintResult($"Silver: {silver}", timer);

            PuzzleConsole.WriteLine($"Executed {(executions > 1 ? $"{executions} times" : "once")}:");

            return silver;
        }

        public static async Task<string> RunGoldAsync(
           int year,
           int day,
           int executions = 1,
           CancellationToken cancellationToken = default)
        {
            string gold = string.Empty;
            PuzzleTimer timer = new();
            IPuzzle? puzzle = Puzzle.GetPuzzle(year, day);

            if (puzzle == null)
            {
                throw new InvalidOperationException($"No puzzle found with the year: {year} and day: {day}");
            }

            SetupProcess();
            CollectAndFinalize();

            timer.Reset();

            for (int i = 1; i <= executions; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                timer.Restart();
                gold = puzzle.Gold() ?? string.Empty;
                timer.Stop();

                if (i > 1 && string.IsNullOrEmpty(gold))
                {
                    break;
                }

                if (i != executions)
                {
                    PuzzleConsole.Clear();
                    await Task.Yield();
                }
            }

            PrintResult($"Gold: {gold}", timer);

            PuzzleConsole.WriteLine($"Executed {(executions > 1 ? $"{executions} times" : "once")}:");

            return gold;
        }

        public static void RunPuzzle(int year, int day, int iterations = 1)
        {
            string silver = string.Empty;
            string gold = string.Empty;
            PuzzleTimer timer = new();
            IPuzzle? puzzle = Puzzle.GetPuzzle(year, day);

            if (puzzle == null)
            {
                throw new InvalidOperationException($"No puzzle found with the year: {year} and day: {day}");
            }

            SetupProcess();
            CollectAndFinalize();
            PrintTitle(day, year, puzzle);

            for (int i = 1; i <= iterations; i++)
            {
                timer.Restart();
                silver = puzzle?.Silver() ?? string.Empty;
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
                timer.Restart();
                gold = puzzle?.Gold() ?? string.Empty;
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

        private static void SetupProcess()
        {
            Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(1); // consistent timing // single processor // single cache
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
        }

        private static void CollectAndFinalize()
        {
            GC.Collect();
            GC.Collect(); // 2nd forces root objects to 2nd level
            GC.WaitForPendingFinalizers(); // wait until collection has happened
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

        private static void PrintTitle(int day, int year, IPuzzle? puzzle)
        {
            Console.WriteLine();
            Console.WriteLine($" {$"--- Advent Of Code {year} Day {day}: {puzzle?.DayTitle}"} ---");
            Console.WriteLine();
        }
    }
}
