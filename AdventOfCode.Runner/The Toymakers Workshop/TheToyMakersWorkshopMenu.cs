namespace AdventOfCode.Runner.The_Toymakers_Workshop
{
    using System;
    using AdventOfCode.Core;
    using AdventOfCode.Core.ConsoleMenu;
    using AdventOfCode.Runner.North_Pole_Operations;

    public class TheToyMakersWorkshopMenu : ConsoleMenu, IConsoleMenu
    {
        public TheToyMakersWorkshopMenu(IPuzzle puzzle)
           : base("The Toymaker's Workshop")
        {
            this.Puzzle = puzzle;
            this.Executions = 1;

            this.AddMenuItems();
        }

        public IPuzzle Puzzle { get; private set; }

        private int Executions { get; set; }

        public async Task<IConsoleMenu> Execute()
        {
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
                    case TheToyMakersWorkshopMenuType.RunSilverSolution:
                        {
                            string result = await PuzzleRunner.RunSilverAsync(this.Puzzle, this.Executions);
                            this.SaveRun("Silver", result);
                            this.WaitForUser();
                            break;
                        }

                    case TheToyMakersWorkshopMenuType.RunGoldSolution:
                        {
                            string result = await PuzzleRunner.RunGoldAsync(this.Puzzle, this.Executions);
                            this.SaveRun("Gold", result);
                            this.WaitForUser();
                            break;
                        }

                    case TheToyMakersWorkshopMenuType.RunBothParts:
                        await PuzzleRunner.RunAsync(this.Puzzle, this.Executions, false);
                        this.WaitForUser();
                        break;

                    case TheToyMakersWorkshopMenuType.SetExecutionCount:
                        this.Executions = PromptInt("Executions", 1);
                        break;

                    case GenericMenu.Back:
                        return await new TheToyMakersWorkshopSelectorMenu().Execute();

                    case GenericMenu.MainMenu:
                        this.GotoMainMenu();
                        return new NorthPoleOperationsMenu();

                    case GenericMenu.Exit:
                        return await new ExitMenu().Execute();
                }
            }
        }

        private void SaveRun(
            string solution,
            string result)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Runs");

            Directory.CreateDirectory(path);

            string fileName =
                $"toymakers-workshop-{this.Puzzle.Year}-day-{this.Puzzle.Day:00}-{solution.ToLower()}-{DateTime.Now:yyyyMMdd-HHmmss}.txt";

            string filePath = Path.Combine(path, fileName);

            List<string> lines =
            [
                "The Toymaker's Workshop",
                $"Run       : {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                $"Puzzle    : {this.Puzzle.Year} Day {this.Puzzle.Day:00}",
                $"Title     : {this.Puzzle.DayTitle}",
                $"Solution  : {solution}",
                $"Executions: {this.Executions}",
                $"Result    : {result}"
            ];

            File.WriteAllLines(filePath, lines);
        }

        private void AddMenuItems()
        {
            this.Items.Clear();

            this.Items.Add(TheToyMakersWorkshopMenuType.RunSilverSolution, "Run Silver Solution", "Run the silver solution");
            this.Items.Add(TheToyMakersWorkshopMenuType.RunGoldSolution, "Run Gold Solution", "Run the gold solution");
            this.Items.Add(TheToyMakersWorkshopMenuType.RunBothParts, "Run Both Parts", "Run silver and gold");
            this.Items.Add(TheToyMakersWorkshopMenuType.SetExecutionCount, "Set Execution Count", $"Currently {this.Executions}");

            this.AddGenericMenuItems("Choose another puzzle");
        }
    }
}