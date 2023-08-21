namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_04___Secure_Container;

    public class Day4 : Puzzle, IPuzzle
    {
        public Day4(string file)
        {
            this.DayTitle = "Secure Container";
            this.GetPuzzleData(file);
        }

        public Day4(string[] input) => this.Input = input;

        public string Silver() => $"{SecureContainer.Simple(this.Input)}";

        public string Gold() => $"{SecureContainer.Advanced(this.Input)}";
    }
}
