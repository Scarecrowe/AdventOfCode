namespace AdventOfCode.Runner
{
    using System.Diagnostics;
    using AdventOfCode.Core;

    public class PuzzleRunner
    {
        public PuzzleRunner(string[] args)
        {
            this.Args = new CommandArguments(args);

            if (!this.Args.Valid)
            {
                return;
            }

            this.Puzzle = Core.Puzzle.GetPuzzle(this.Args.Year, this.Args.Day);

            if (this.Puzzle == null)
            {
                return;
            }

            SetupProcess();
            this.RunPreBatch();
            CollectAndFinalize();
            this.PrintTitle();
            this.RunPuzzle();

            Console.ReadLine();
        }

        private ICommandArguments Args { get; }

        private IPuzzle? Puzzle { get; }

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

        private static void PrintTree(int count = 1)
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

        private void RunPuzzle()
        {
            string silver = string.Empty;
            string gold = string.Empty;
            PuzzleTimer timer = new();

            for (int i = 1; i <= this.Args.BatchCount; i++)
            {
                timer.Restart();
                silver = this.Puzzle?.Silver() ?? string.Empty;
                timer.Stop();

                if (i != this.Args.BatchCount)
                {
                    PuzzleConsole.Clear();
                }
            }

            PrintResult($"Silver: {silver}", timer);

            timer.Reset();

            for (int i = 1; i <= this.Args.BatchCount; i++)
            {
                timer.Restart();
                gold = this.Puzzle?.Gold() ?? string.Empty;
                timer.Stop();

                if (i > 1 && string.IsNullOrEmpty(gold))
                {
                    break;
                }

                if (i != this.Args.BatchCount)
                {
                    PuzzleConsole.Clear();
                }
            }

            PrintResult($"Gold: {gold}", timer);
            Console.WriteLine($" Executed {(this.Args.BatchCount > 1 ? $"{this.Args.BatchCount} times" : "once")}:");
            CopyResultToClipboard(string.IsNullOrEmpty(gold) ? silver : gold);
        }

        private void RunPreBatch()
        {
            if (this.Args.BatchCount > 1)
            {
                for (int i = 1; i <= 2; i++)
                {
                    this.Puzzle?.Silver();
                    this.Puzzle?.Gold();
                }
            }
        }

        private void PrintTitle()
        {
            PrintTree(13);
            Console.WriteLine();
            Console.WriteLine($" {$"Advent Of Code {this.Args.Year} Day {this.Args.Day}: {this.Puzzle?.DayTitle}"}");
            Console.WriteLine();
        }
    }
}
