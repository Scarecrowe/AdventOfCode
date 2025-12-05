namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_24___Never_Tell_Me_The_Odds;

    public class Day24 : Puzzle, IPuzzle
    {
        public Day24()
        {
            this.DayTitle = "Never Tell Me The Odds";
            this.GetPuzzleData(24, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new NeverTellMeTheOdds(this.Input).TestAreaIntersections()}";

        public string Gold() => $"{new NeverTellMeTheOdds(this.Input, false).SingleThrow()}";
    }
}
