namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_18___Like_a_GIF_For_Your_Yard;

    public class Day18 : Puzzle, IPuzzle
    {
        public Day18(string file)
        {
            this.DayTitle = "Like a GIF For Your Yard";
            this.GetPuzzleData(file);
        }

        public Day18(string[] input) => this.Input = input;

        public string Silver() => $"{new LikeAGIFForYourYard(this.Input).Animate(100).CountLit()}";

        public string Gold() => $"{new LikeAGIFForYourYard(this.Input).AnimateGameOfLife(100).CountLit()}";
    }
}
