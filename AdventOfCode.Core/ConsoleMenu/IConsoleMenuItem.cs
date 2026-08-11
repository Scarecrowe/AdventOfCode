namespace AdventOfCode.Core.ConsoleMenu
{
    public interface IConsoleMenuItem
    {
        object? Key { get; }

        int Index { get; }

        string Name { get; }

        string Description { get; }

        T? GetKey<T>();
    }
}
