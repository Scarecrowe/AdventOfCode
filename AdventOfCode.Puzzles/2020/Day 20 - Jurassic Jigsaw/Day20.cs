namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_20___Jurassic_Jigsaw;

    public class Day20 : Puzzle, IPuzzle
    {
        public Day20(string file)
        {
            this.DayTitle = "Jurassic Jigsaw";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day20(string[] input) => this.Input = input;

        public string Silver() => $"{new JurrassicJigsaw(this.Input).Corners()}";

        public string Gold() => $"{new JurrassicJigsaw(this.Input).NotSeaMonster()}";
    }
}
