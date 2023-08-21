namespace AdventOfCode.Runner
{
    public interface ICommandArguments
    {
        int Year { get; }

        int Day { get; }

        int BatchCount { get; }

        bool Valid { get; }
    }
}
