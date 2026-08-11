namespace AdventOfCode.Runner.Santas_Gauntlet
{
    using System;
    using System.Diagnostics;
    using AdventOfCode.Core;
    using AdventOfCode.Core.ConsoleMenu;
    using AdventOfCode.Runner.North_Pole_Operations;

    public class SantasGauntletMenu : ConsoleMenu, IConsoleMenu
    {
        public SantasGauntletMenu()
            : base("Santa's Gauntlet")
        {
            this.Items.Add(SantasGauntletMenuType.RunAllPuzzles, "Run All Puzzles", "Run silver and gold for every puzzle");
            this.AddGenericMenuItems("Return to North Pole Operations");
        }

        public async Task<IConsoleMenu> Execute()
        {
            this.Reset();

            IConsoleMenuItem? item = await this.WriteMenu();

            switch ((SantasGauntletMenuType)(item?.Index ?? 0))
            {
                case SantasGauntletMenuType.RunAllPuzzles:
                    this.SetSubTitle("Running all puzzles");
                    this.Reset();
                    this.RunAllPuzzles();
                    this.WaitForUser();
                    break;

                case SantasGauntletMenuType.Back:
                case SantasGauntletMenuType.MainMenu:
                    return new NorthPoleOperationsMenu();

                case SantasGauntletMenuType.Exit:
                    return await new ExitMenu().Execute();
            }

            return new SantasGauntletMenu();
        }

        private void RunAllPuzzles()
        {
            List<string> report = [];
            Stopwatch totalTimer = Stopwatch.StartNew();

            int total = 0;
            int passed = 0;
            int failed = 0;

            PuzzleConsole.WriteLine("Running Santa's Gauntlet...");
            PuzzleConsole.WriteLine();

            for (int year = 2015; year <= 2025; year++)
            {
                for (int day = 1; day <= 25; day++)
                {
                    IPuzzle? puzzle = Puzzle.GetPuzzle(year, day);

                    if (puzzle == null)
                    {
                        continue;
                    }

                    total++;

                    bool ok = this.RunAndReport(report, year, day);

                    if (ok)
                    {
                        passed++;
                    }
                    else
                    {
                        failed++;
                    }
                }
            }

            totalTimer.Stop();

            PuzzleConsole.WriteLine();
            PuzzleConsole.WriteLine("--------------------------------------------------------------------------------");
            PuzzleConsole.WriteLine($"Puzzles: {total}");
            PuzzleConsole.WriteLine($"Passed : {passed}");
            PuzzleConsole.WriteLine($"Failed : {failed}");
            PuzzleConsole.WriteLine($"Total  : {totalTimer.Elapsed:hh\\:mm\\:ss\\.ffff}");
            PuzzleConsole.WriteLine("--------------------------------------------------------------------------------");
            PuzzleConsole.Flush();

            this.SaveRun(report, total, passed, failed, totalTimer.Elapsed);
        }

        private bool RunAndReport(
            List<string> report,
            int year,
            int day)
        {
            Stopwatch silverTimer = new();
            Stopwatch goldTimer = new();

            string line;

            try
            {
                IPuzzle? puzzle = Puzzle.GetPuzzle(year, day);

                if (puzzle == null)
                {
                    line = $"{year} Day {day:00} // FAILED // Could not create puzzle";
                    this.WriteReportLine(report, line);
                    return false;
                }

                string title = puzzle.DayTitle ?? string.Empty;

                string silver;
                string gold;

                silverTimer.Restart();
                silver = puzzle.Silver() ?? string.Empty;
                silverTimer.Stop();

                puzzle = Puzzle.GetPuzzle(year, day);

                if (puzzle == null)
                {
                    line = $"{year} Day {day:00} // FAILED // Could not create puzzle for gold";
                    this.WriteReportLine(report, line);
                    return false;
                }

                goldTimer.Restart();
                gold = puzzle.Gold() ?? string.Empty;
                goldTimer.Stop();

                line =
                    $"{year} Day {day:00} " +
                    $"// {title,-40} " +
                    $"// Silver: {silver,-20} ({silverTimer.Elapsed.TotalMilliseconds,10:F4}ms) " +
                    $"// Gold: {gold,-20} ({goldTimer.Elapsed.TotalMilliseconds,10:F4}ms)";

                this.WriteReportLine(report, line);
                return true;
            }
            catch (Exception ex)
            {
                line =
                    $"{year} Day {day:00} " +
                    $"// FAILED // {ex.Message}";

                this.WriteReportLine(report, line);
                return false;
            }
        }

        private void SaveRun(
            List<string> report,
            int total,
            int passed,
            int failed,
            TimeSpan elapsed)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Runs");

            Directory.CreateDirectory(path);

            string fileName = $"santas-gauntlet-{DateTime.Now:yyyyMMdd-HHmmss}.txt";
            string filePath = Path.Combine(path, fileName);

            List<string> lines = [];

            lines.Add("Santa's Gauntlet");
            lines.Add($"Run    : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            lines.Add($"Puzzles: {total}");
            lines.Add($"Passed : {passed}");
            lines.Add($"Failed : {failed}");
            lines.Add($"Total  : {elapsed:hh\\:mm\\:ss\\.ffff}");
            lines.Add("--------------------------------------------------------------------------------");
            lines.AddRange(report);

            File.WriteAllLines(filePath, lines);
        }

        private void WriteReportLine(
            List<string> report,
            string line)
        {
            report.Add(line);
            PuzzleConsole.WriteLine(line);
            PuzzleConsole.Flush();
        }
    }
}