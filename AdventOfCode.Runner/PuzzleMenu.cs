namespace AdventOfCode.Runner
{
    using AdventOfCode.Core.ConsoleMenu;
    using AdventOfCode.Runner.Menus;
    using AdventOfCode.Runner.North_Pole_Operations;

    public class PuzzleMenu
    {
        public PuzzleMenu()
        {
        }

        public async void Execute()
        {
            while(true)
            {
                IConsoleMenu menu = new NorthPoleOperationsMenu();

                await menu.Execute();
            }
        }
    }
}
