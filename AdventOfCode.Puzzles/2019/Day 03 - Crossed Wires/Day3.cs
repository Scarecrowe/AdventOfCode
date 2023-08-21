namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_03___Crossed_Wires;

    public class Day3 : Puzzle, IPuzzle
    {
        public Day3(string file)
        {
            this.DayTitle = "Crossed Wires";
            this.GetPuzzleData(file);
        }

        public Day3(string[] input) => this.Input = input;

        public string Silver() => $"{CrossedWires.Nearest(this.Input)}";

        public string Gold() => $"{CrossedWires.StepsToIntersection(this.Input)}";
    }
}
