namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_15___Science_for_Hungry_People;

    public class Day15 : Puzzle, IPuzzle
    {
        public Day15(string file)
        {
            this.DayTitle = "Science for Hungry People";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day15(string[] input) => this.Input = input;

        public string Silver() => $"{new ScienceForHungryPeople(this.Input).HighestRankingMixture(100)}";

        public string Gold() => $"{new ScienceForHungryPeople(this.Input).HighestRankingMixture(100, 500)}";
    }
}
