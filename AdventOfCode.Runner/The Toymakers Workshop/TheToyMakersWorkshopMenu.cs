namespace AdventOfCode.Runner.The_Toymakers_Workshop
{
    using System.Threading.Tasks;
    using AdventOfCode.Core;
    using AdventOfCode.Runner.Menus;
    using AdventOfCode.Runner.North_Pole_Operations;

    public class TheToyMakersWorkshopMenu : Menu, IMenu
    {
        public TheToyMakersWorkshopMenu(int year, int day)
           : base("The Toymaker's Workshop")
        {
            this.Year = year;
            this.Day = day;
            this.Executions = 1;
        }

        public int Year { get; private set; }

        public int Day { get; private set; }

        private int Executions { get; set; }

        public async Task<IMenu> Execute()
        {
            while (true)
            {
                this.Reset();

                IPuzzle? puzzle = Puzzle.GetPuzzle(this.Year, this.Day);
                this.PrintSubTitle($"{this.Year} Day {this.Day} - {puzzle?.DayTitle ?? string.Empty}");
                PuzzleConsole.WriteLine();

                PuzzleConsole.WriteLine("1. Run Silver Solution");
                PuzzleConsole.WriteLine("2. Run Gold Solution");
                PuzzleConsole.WriteLine("3. Run Both Parts");
                PuzzleConsole.WriteLine($"4. Set Execution Count ({this.Executions})");
                PuzzleConsole.WriteLine($"5. Choose Another Puzzle");
                PuzzleConsole.WriteLine($"6. Return to North Pole Operations");
                PuzzleConsole.WriteLine($"7. Santa's Calling It a Day (Exit)");
                PuzzleConsole.WriteLine();
                PuzzleConsole.Flush();

                int option = PromptInt("Select an option: ", 1);

                this.Reset();
                this.PrintSubTitle($"{this.Year} Day {this.Day} - {puzzle?.DayTitle ?? string.Empty}");
                PuzzleConsole.WriteLine();
                PuzzleConsole.Flush();

                switch ((TheToyMakersWorkshop)option)
                {
                    case TheToyMakersWorkshop.RunSilverSolution:
                        await PuzzleRunner.RunSilverAsync(this.Year, this.Day, this.Executions);
                        this.WaitForUser();
                        break;
                    case TheToyMakersWorkshop.RunGoldSolution:
                        await PuzzleRunner.RunGoldAsync(this.Year, this.Day, this.Executions);
                        this.WaitForUser();
                        break;
                    case TheToyMakersWorkshop.RunBothParts:
                        await PuzzleRunner.RunAsync(this.Year, this.Day, this.Executions, false);
                        this.WaitForUser();
                        break;
                    case TheToyMakersWorkshop.SetExecutionCount:
                        this.Executions = PromptInt("Executions", 1);
                        break;
                    case TheToyMakersWorkshop.ChooseAnotherPuzzle:
                        return await new TheToyMakersWorkshopSelectorMenu().Execute();
                    case TheToyMakersWorkshop.ReturnToMainMenu:
                        return await new NorthPoleOperationsMenu().Execute();
                    case TheToyMakersWorkshop.Exit:
                        return await new ExitMenu().Execute();
                }
            }
        }
    }
}
