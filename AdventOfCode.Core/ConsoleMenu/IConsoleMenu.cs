namespace AdventOfCode.Core.ConsoleMenu
{
    using System.Threading.Tasks;

    public interface IConsoleMenu
    {
        string Title { get; }

        bool MainMenu { get; }

        IConsoleMenuItems Items { get; }

        List<string> History { get; }

        Task<IConsoleMenu> Execute();

        void GotoMainMenu();
    }
}
