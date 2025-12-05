namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_08___Treetop_Tree_House;

    public class Day8 : Puzzle, IPuzzle
    {
        public Day8()
        {
            this.DayTitle = "Treetop Tree House";
            this.GetPuzzleData(8, this.DayTitle);
        }

        public Day8(string[] input) => this.Input = input;

        public string Silver() => $"{new TreetopTreeHouse(this.Input).VisibleTrees()}";

        public string Gold() => $"{new TreetopTreeHouse(this.Input).BestTreeHouseScore()}";
    }
}
