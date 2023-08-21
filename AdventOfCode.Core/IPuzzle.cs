namespace AdventOfCode.Core
{
    public interface IPuzzle
    {
        string FilePath { get; }

        string DayTitle { get; }

        string[] Input { get; }

        string Silver();

        string Gold();
    }
}
