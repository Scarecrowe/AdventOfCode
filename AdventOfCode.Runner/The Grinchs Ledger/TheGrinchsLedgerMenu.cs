namespace AdventOfCode.Runner.The_Grinchs_Ledger
{
    using AdventOfCode.Animation;
    using AdventOfCode.Core;
    using AdventOfCode.Core.ConsoleMenu;
    using AdventOfCode.Runner.North_Pole_Operations;

    public class TheGrinchsLedgerMenu : ConsoleMenu, IConsoleMenu
    {
        private const int FirstYear = 2015;

        private const int LastYear = 2025;

        private const int DaysPerYear = 25;

        private const int DaysPerYearAfter2024 = 12;

        private static readonly string RunsPath =
            Path.Combine(AppContext.BaseDirectory, "Runs");

        private static readonly string AnimationsPath =
            Path.Combine(AppContext.BaseDirectory, "Animations");

        public TheGrinchsLedgerMenu()
            : base("The Grinch's Ledger")
        {
            this.Items.Add(
                TheGrinchsLedgerMenuType.ViewSystemStats,
                "View System Stats",
                "View puzzle, run, animation and storage stats");

            this.AddGenericMenuItems("Return to North Pole Operations");
        }

        public async Task<IConsoleMenu> Execute()
        {
            this.Reset();

            IConsoleMenuItem? item = await this.WriteMenu();

            switch (item?.Key)
            {
                case TheGrinchsLedgerMenuType.ViewSystemStats:
                    this.Reset();
                    WriteLedger();
                    this.WaitForUser();
                    break;

                case GenericMenu.Back:
                case GenericMenu.MainMenu:
                    return new NorthPoleOperationsMenu();

                case GenericMenu.Exit:
                    return await new ExitMenu().Execute();
            }

            return new TheGrinchsLedgerMenu();
        }

        private static void WriteLedger()
        {
            int totalPuzzleSlots = GetTotalPossiblePuzzles();
            int implementedPuzzles = CountImplementedPuzzles();
            int missingPuzzles = totalPuzzleSlots - implementedPuzzles;

            decimal completion = totalPuzzleSlots == 0
                ? 0
                : ((decimal)implementedPuzzles / totalPuzzleSlots) * 100;

            string[] runFiles = GetFiles(RunsPath);
            string[] animationFiles = GetFiles(AnimationsPath);

            int toymakerRuns = runFiles.Count(x => Path.GetFileName(x).StartsWith("toymakers-workshop-", StringComparison.OrdinalIgnoreCase));
            int gauntletRuns = runFiles.Count(x => Path.GetFileName(x).StartsWith("santas-gauntlet-", StringComparison.OrdinalIgnoreCase));
            int benchmarkRuns = runFiles.Count(x => Path.GetFileName(x).Contains("benchmark", StringComparison.OrdinalIgnoreCase));

            long runsSize = GetDirectorySize(RunsPath);
            long animationsSize = GetDirectorySize(AnimationsPath);
            long totalGeneratedSize = runsSize + animationsSize;

            DateTime? mostRecentRun = GetMostRecentWriteTime(runFiles);

            int asciiAnimations = Animation.GetByType(AnimationType.Ascii).Count;
            int consoleAnimations = Animation.GetByType(AnimationType.Console).Count;
            int twoDAnimations = Animation.GetByType(AnimationType.TwoDimension).Count;
            int threeDAnimations = Animation.GetByType(AnimationType.ThreeDimension).Count;
            int totalAnimationClasses = asciiAnimations + consoleAnimations + twoDAnimations + threeDAnimations;

            PuzzleConsole.WriteLine("The Grinch's Ledger");
            PuzzleConsole.WriteLine("--------------------------------------------------------------------------------");
            PuzzleConsole.WriteLine();

            PuzzleConsole.WriteLine("Puzzle Ledger");
            PuzzleConsole.WriteLine("--------------------------------------------------------------------------------");
            PuzzleConsole.WriteLine($"Implemented Puzzles     : {implementedPuzzles:N0} / {totalPuzzleSlots:N0}");
            PuzzleConsole.WriteLine($"Missing Puzzles         : {missingPuzzles:N0}");
            PuzzleConsole.WriteLine($"Completion              : {completion:0.00}%");
            PuzzleConsole.WriteLine($"Years Covered           : {FirstYear} - {LastYear}");
            PuzzleConsole.WriteLine($"Format                  : 25 days until 2024, 12 days from 2025");
            PuzzleConsole.WriteLine();

            PuzzleConsole.WriteLine("Runs Ledger");
            PuzzleConsole.WriteLine("--------------------------------------------------------------------------------");
            PuzzleConsole.WriteLine($"Saved Runs              : {runFiles.Length:N0}");
            PuzzleConsole.WriteLine($"Toymaker Runs           : {toymakerRuns:N0}");
            PuzzleConsole.WriteLine($"Gauntlet Runs           : {gauntletRuns:N0}");
            PuzzleConsole.WriteLine($"Benchmark Runs          : {benchmarkRuns:N0}");
            PuzzleConsole.WriteLine($"Runs Archive Size       : {FormatBytes(runsSize)}");
            PuzzleConsole.WriteLine($"Most Recent Run         : {FormatDateTime(mostRecentRun)}");
            PuzzleConsole.WriteLine();

            PuzzleConsole.WriteLine("Animation Ledger");
            PuzzleConsole.WriteLine("--------------------------------------------------------------------------------");
            PuzzleConsole.WriteLine($"Ascii Animations        : {asciiAnimations:N0}");
            PuzzleConsole.WriteLine($"Console Animations      : {consoleAnimations:N0}");
            PuzzleConsole.WriteLine($"2D Animations           : {twoDAnimations:N0}");
            PuzzleConsole.WriteLine($"3D Animations           : {threeDAnimations:N0}");
            PuzzleConsole.WriteLine($"Total Animation Classes : {totalAnimationClasses:N0}");
            PuzzleConsole.WriteLine($"Generated Files         : {animationFiles.Length:N0}");
            PuzzleConsole.WriteLine($"Generated Size          : {FormatBytes(animationsSize)}");
            PuzzleConsole.WriteLine();

            PuzzleConsole.WriteLine("Storage Ledger");
            PuzzleConsole.WriteLine("--------------------------------------------------------------------------------");
            PuzzleConsole.WriteLine($"Runs Folder             : {FormatBytes(runsSize)}");
            PuzzleConsole.WriteLine($"Animations Folder       : {FormatBytes(animationsSize)}");
            PuzzleConsole.WriteLine($"Total Generated Data    : {FormatBytes(totalGeneratedSize)}");
            PuzzleConsole.WriteLine("--------------------------------------------------------------------------------");

            PuzzleConsole.Flush();
        }

        private static int CountImplementedPuzzles()
        {
            int count = 0;

            for (int year = FirstYear; year <= LastYear; year++)
            {
                for (int day = 1; day <= GetPuzzleCountForYear(year); day++)
                {
                    if (Puzzle.GetPuzzle(year, day) != null)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private static int GetTotalPossiblePuzzles()
        {
            int total = 0;

            for (int year = FirstYear; year <= LastYear; year++)
            {
                total += GetPuzzleCountForYear(year);
            }

            return total;
        }

        private static int GetPuzzleCountForYear(int year)
        {
            return year > 2024
                ? DaysPerYearAfter2024
                : DaysPerYear;
        }

        private static string[] GetFiles(string path)
        {
            if (!Directory.Exists(path))
            {
                return [];
            }

            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
        }

        private static long GetDirectorySize(string path)
        {
            if (!Directory.Exists(path))
            {
                return 0;
            }

            return Directory
                .GetFiles(path, "*.*", SearchOption.AllDirectories)
                .Sum(file => new FileInfo(file).Length);
        }

        private static DateTime? GetMostRecentWriteTime(string[] files)
        {
            if (files.Length == 0)
            {
                return null;
            }

            return files
                .Select(File.GetLastWriteTime)
                .Max();
        }

        private static string FormatDateTime(DateTime? value)
        {
            return value.HasValue
                ? value.Value.ToString("yyyy-MM-dd HH:mm:ss")
                : "Never";
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
    }
}