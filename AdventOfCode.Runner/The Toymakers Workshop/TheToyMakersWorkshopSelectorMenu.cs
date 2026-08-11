namespace AdventOfCode.Runner.The_Toymakers_Workshop
{
    using System.Threading.Tasks;
    using AdventOfCode.Core;
    using AdventOfCode.Core.ConsoleMenu;
    using AdventOfCode.Runner.North_Pole_Operations;

    public class TheToyMakersWorkshopSelectorMenu : ConsoleMenu, IConsoleMenu
    {
        public TheToyMakersWorkshopSelectorMenu()
           : base("The Toymaker's Workshop")
        {
        }

        public async Task<IConsoleMenu> Execute()
        {
            YearMenu yearSelector = new(this.Title, "Assembling your solution...", "Return to North Pole Operations");

            while (true)
            {
                this.Reset();

                await yearSelector.Execute();

                if (yearSelector.GoBack)
                {
                    this.GotoMainMenu();
                    return this;
                }

                DayMenu daySelector = new(this.Title, "Assembling your solution...", "Select a different year", yearSelector.Year);

                await daySelector.Execute();

                if (daySelector.GoBack)
                {
                    yearSelector.ResetYear();
                    continue;
                }

                if (daySelector.MainMenu)
                {
                    this.GotoMainMenu();
                    return this;
                }

                IConsoleMenu menu = await new TheToyMakersWorkshopMenu(Puzzle.GetPuzzle(yearSelector.Year, daySelector.Day)!).Execute();

                if (menu.MainMenu)
                {
                    return this;
                }

                yearSelector.ResetYear();

                break;
            }

            return new NorthPoleOperationsMenu();
        }
    }
}
