namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_02___Cube_Conundrum;

    public class Day2 : Puzzle, IPuzzle
    {
        public Day2(string file)
        {
            this.DayTitle = "Cube Conundrum";
            this.GetPuzzleData(file);
        }

        public string Silver() => $"{new CubeConundrum(this.Input).SumOfIds(12, 13, 14)}";

        public string Gold() => $"{new CubeConundrum(this.Input).SumOfPower()}";
    }
}
