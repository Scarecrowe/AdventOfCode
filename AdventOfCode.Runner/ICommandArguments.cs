namespace AdventOfCode.Runner
{
    public interface ICommandArguments
    {
        int Year { get; }

        int Day { get; }

        int Iterations { get; }

        bool Valid { get; }
    }
}
