namespace AdventOfCode.Core
{
    public interface IPuzzle
    {
        string FilePath { get; }

        string DayTitle { get; }

        int Year { get; }

        int Day { get; }

        string[] Input { get; }

        string Silver();

        string Gold();
    }
}
