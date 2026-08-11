namespace AdventOfCode.Core.ConsoleMenu
{
    using System.Threading.Tasks;
    using AdventOfCode.Core;

    public class ExitMenu : ConsoleMenu, IConsoleMenu
    {
        public ExitMenu()
            : base("North Pole Operations")
        {
        }

        public Task<IConsoleMenu> Execute()
        {
            this.Reset();
            PuzzleConsole.WriteLine("Clocking off for Christmas.");
            PuzzleConsole.WriteLine();
            PuzzleConsole.Flush();
            Environment.Exit(0);

            return Task.FromResult((IConsoleMenu)this);
        }
    }
}
