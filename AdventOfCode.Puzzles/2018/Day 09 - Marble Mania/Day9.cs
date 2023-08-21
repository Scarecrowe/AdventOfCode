namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_09___Marble_Mania;

    public class Day9 : Puzzle, IPuzzle
    {
        public Day9(string file)
        {
            this.DayTitle = "Marble Mania";
            this.GetPuzzleData(file);
        }

        public Day9(string[] input) => this.Input = input;

        public string Silver() => $"{new MarbleMania(this.Input[0]).Play()}";

        public string Gold() => $"{new MarbleMania(this.Input[0]).Play(100)}";
    }
}
