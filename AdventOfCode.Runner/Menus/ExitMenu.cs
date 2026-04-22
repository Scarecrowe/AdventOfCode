namespace AdventOfCode.Runner.Menus
{
    using System.Threading.Tasks;
    using AdventOfCode.Core;

    public class ExitMenu : Menu, IMenu
    {
        public ExitMenu()
            : base("North Pole Operations")
        {
        }

        public Task<IMenu> Execute()
        {
            this.Reset();
            PuzzleConsole.WriteLine("Clocking off for Christmas.");
            PuzzleConsole.WriteLine();
            PuzzleConsole.Flush();
            Environment.Exit(0);

            return Task.FromResult((IMenu)this);
        }
    }
}
