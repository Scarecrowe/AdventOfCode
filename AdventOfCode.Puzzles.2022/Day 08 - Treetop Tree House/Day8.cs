namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_08___Treetop_Tree_House;

    public class Day8 : Puzzle, IPuzzle
    {
        public Day8()
            : base(2022, 8, "Treetop Tree House")
        {
        }

        public Day8(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new TreetopTreeHouse(this.Input).VisibleTrees()}";

        public string Gold() => $"{new TreetopTreeHouse(this.Input).BestTreeHouseScore()}";
    }
}
