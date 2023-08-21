namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_16___Dragon_Checksum;

    public class Day16 : Puzzle, IPuzzle
    {
        public Day16(string file)
        {
            this.DayTitle = "Dragon Checksum";
            this.GetPuzzleData(file);
        }

        public Day16(string[] input) => this.Input = input;

        public string Silver() => $"{new DragonChecksum(this.Input[0]).Fill(272)}";

        public string Gold() => $"{new DragonChecksum(this.Input[0]).Fill(35651584)}";
    }
}
