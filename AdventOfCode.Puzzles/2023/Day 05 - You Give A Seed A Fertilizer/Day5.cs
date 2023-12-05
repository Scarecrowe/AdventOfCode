namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_05___You_Give_A_Seed_A_Fertilizer;

    public class Day5 : Puzzle, IPuzzle
    {
        public Day5(string file)
        {
            this.DayTitle = "If You Give A Seed A Fertilizer";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public string Silver() => $"{new YouGiveASeedAFertilizer(this.Input.ToList()).LowestSeed()}";

        public string Gold() => $"{new YouGiveASeedAFertilizer(this.Input.ToList()).LowestRange()}";
    }
}
