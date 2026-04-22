namespace AdventOfCode.Runner
{
    using AdventOfCode.Runner.Menus;
    using AdventOfCode.Runner.North_Pole_Operations;

    public class PuzzleMenu
    {
        public PuzzleMenu()
        {
            this.Menu = new NorthPoleOperationsMenu();
        }

        private IMenu Menu { get; set; }

        public async void Execute()
        {
            while(true)
            {
                IMenu menu = await this.Menu.Execute();

                if (menu is ExitMenu)
                {
                    await menu.Execute();
                    break;
                }

                this.Menu = menu;
            }
        }
    }
}
