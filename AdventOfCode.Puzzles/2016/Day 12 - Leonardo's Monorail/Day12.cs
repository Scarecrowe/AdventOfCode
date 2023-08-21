namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_12___Leonardo_s_Monorail;

    public class Day12 : Puzzle, IPuzzle
    {
        public Day12(string file)
        {
            this.DayTitle = "Leonardo's Monorail";
            this.GetPuzzleData(file);
        }

        public Day12(string[] input) => this.Input = input;

        public string Silver() => $"{new LeonardosMonorail(this.Input).Process().Registers[0]}";

        public string Gold() => $"{new LeonardosMonorail(this.Input).Process(1).Registers[0]}";
    }
}
