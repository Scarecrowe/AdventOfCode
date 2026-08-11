namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_09___Mirage_Maintenance;

    public class Day9 : Puzzle, IPuzzle
    {
        public Day9()
            : base(2023, 9, "Mirage Maintenance")
        {
        }

        public Day9(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new MirageMaintenance(this.Input).End()}";

        public string Gold() => $"{new MirageMaintenance(this.Input).Start()}";
    }
}
