namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_09___Smoke_Basin;

    public class Day9 : Puzzle, IPuzzle
    {
        public Day9(string file)
        {
            this.DayTitle = "Smoke Basin";
            this.GetPuzzleData(file);

            // this.Input = new string[5];
            // this.Input[0] = "2199943210";
            // this.Input[1] = "3987894921";
            // this.Input[2] = "9856789892";
            // this.Input[3] = "8767896789";
            // this.Input[4] = "9899965678";
        }

        public Day9(string[] input) => this.Input = input;

        public string Silver() => $"{new SmokeBasin(this.Input).SumOfRiskLevels()}";

        public string Gold() => $"{new SmokeBasin(this.Input).SumOfBasin()}";
    }
}
