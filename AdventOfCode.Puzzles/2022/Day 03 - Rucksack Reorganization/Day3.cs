namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_03___Rucksack_Reorganization;

    public class Day3 : Puzzle, IPuzzle
    {
        public Day3(string file)
        {
            this.DayTitle = "Rucksack Reorganization";
            this.GetPuzzleData(file);
        }

        public Day3(string[] input) => this.Input = input;

        public string Silver() => $"{RucksackReorganization.Sum(this.Input)}";

        public string Gold() => $"{RucksackReorganization.GroupSum(this.Input)}";
    }
}
