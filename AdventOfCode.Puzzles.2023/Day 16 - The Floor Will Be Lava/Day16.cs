namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_16___The_Floor_Will_Be_Lava;

    public class Day16 : Puzzle, IPuzzle
    {
        public Day16()
        {
            this.DayTitle = "The Floor Will Be Lava";
            this.GetPuzzleData(16, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new TheFloorWillBeLava(this.Input).Shine()}";

        public string Gold() => $"{new TheFloorWillBeLava(this.Input)}";
    }
}
