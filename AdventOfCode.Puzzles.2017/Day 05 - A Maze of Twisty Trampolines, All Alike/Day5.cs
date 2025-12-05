namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_05___A_Maze_of_Twisty_Trampolines__All_Alike;

    public class Day5 : Puzzle, IPuzzle
    {
        public Day5()
        {
            this.DayTitle = "A Maze of Twisty Trampolines, All Alike";
            this.GetPuzzleData(5, this.DayTitle);
        }

        public Day5(string[] input) => this.Input = input;

        public string Silver() => $"{TwistyTrampolines.Simple(this.Input)}";

        public string Gold() => $"{TwistyTrampolines.Advanced(this.Input)}";
    }
}
