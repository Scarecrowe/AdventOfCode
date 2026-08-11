namespace AdventOfCode.Core.ConsoleMenu
{
    public interface IConsoleMenuItems : IList<IConsoleMenuItem>
    {
        void Add(string name, string description);

        void Add(object key, string name, string description);
    }
}
