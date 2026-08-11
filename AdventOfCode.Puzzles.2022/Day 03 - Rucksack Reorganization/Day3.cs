namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_03___Rucksack_Reorganization;

    public class Day3 : Puzzle, IPuzzle
    {
        public Day3()
            : base(2022, 3, "Rucksack Reorganization")
        {
        }

        public Day3(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{RucksackReorganization.Sum(this.Input)}";

        public string Gold() => $"{RucksackReorganization.GroupSum(this.Input)}";
    }
}
