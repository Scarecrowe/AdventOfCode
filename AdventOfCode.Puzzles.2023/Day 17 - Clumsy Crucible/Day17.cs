namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_17___Clumsy_Crucible;

    public class Day17 : Puzzle, IPuzzle
    {
        public Day17()
        {
            this.DayTitle = "Clumsy Crucible";
            this.GetPuzzleData(17, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new ClumsyCrucible(this.Input).CrucibleHeatLoss()}";

        public string Gold() => $"{new ClumsyCrucible(this.Input).UltraCrucibleHeatLoss()}";
    }
}
