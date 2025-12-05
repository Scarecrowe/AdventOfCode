namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_22___Slam_Shuffle;

    public class Day22 : Puzzle, IPuzzle
    {
        public Day22()
        {
            this.DayTitle = "Slam Shuffle";
            this.GetPuzzleData(22, this.DayTitle);
        }

        public Day22(string[] input) => this.Input = input;

        public string Silver() => $"{new SlamShuffle(this.Input).Shuffle()}";

        public string Gold() => $"{new SlamShuffle(this.Input).ShuffleLargerDeck()}";
    }
}
