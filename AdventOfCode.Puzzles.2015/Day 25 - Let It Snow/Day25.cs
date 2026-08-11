namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_25___Let_It_Snow;

    public class Day25 : Puzzle, IPuzzle
    {
        public Day25()
            : base(2015, 25, "Let It Snow", StringSplitOptions.None)
        {
        }

        public Day25(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new LetItSnow(this.Input).Generate()}";

        public string Gold() => $"You have enough stars to [Turn It Off and On]";
    }
}
