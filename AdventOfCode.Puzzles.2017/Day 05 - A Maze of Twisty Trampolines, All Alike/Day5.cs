namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_05___A_Maze_of_Twisty_Trampolines__All_Alike;

    public class Day5 : Puzzle, IPuzzle
    {
        public Day5()
            : base(2017, 5, "A Maze of Twisty Trampolines, All Alike")
        {
        }

        public Day5(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{TwistyTrampolines.Simple(this.Input)}";

        public string Gold() => $"{TwistyTrampolines.Advanced(this.Input)}";
    }
}
