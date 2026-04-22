namespace AdventOfCode.Runner.The_Toymakers_Workshop
{
    using System;
    using System.Threading.Tasks;
    using AdventOfCode.Core;
    using AdventOfCode.Runner.Menus;
    using AdventOfCode.Runner.North_Pole_Operations;

    public class TheToyMakersWorkshopSelectorMenu : Menu, IMenu
    {
        public TheToyMakersWorkshopSelectorMenu()
           : base("The Toymaker's Workshop")
        {
        }

        public async Task<IMenu> Execute()
        {
            while (true)
            {
                PuzzleYearDayMenu selector = new(this.Title);

                while (selector.Year == -1 || selector.Day == -1)
                {
                    this.Reset();
                    this.WriteLine("Assembling your solution...");
                    PuzzleConsole.WriteLine();
                    PuzzleConsole.Flush();

                    await selector.Execute();

                    if (selector.Day >= 1 && selector.Day <= selector.Days)
                    {
                        IMenu menu = await new TheToyMakersWorkshopMenu(selector.Year, selector.Day).Execute();

                        if (menu is NorthPoleOperationsMenu)
                        {
                            return menu;
                        }

                        selector.ResetYear();
                        continue;
                    }

                    switch ((TheToyMakersWorkshopSelector)selector.Day - selector.Days)
                    {
                        case TheToyMakersWorkshopSelector.ChooseAnotherPuzzle:
                            selector.ResetYear();
                            break;
                        case TheToyMakersWorkshopSelector.ReturnToMainMenu:
                            return await new NorthPoleOperationsMenu().Execute();
                        case TheToyMakersWorkshopSelector.Exit:
                            return await new ExitMenu().Execute();
                    }
                }

                break;
            }

            return new NorthPoleOperationsMenu();
        }
    }
}
