namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2024.Day_05___Print_Queue;

    public class Day5 : Puzzle, IPuzzle
    {
        public Day5()
            : base(2024, 5, "Print Queue", StringSplitOptions.None)
        {
        }

        public Day5(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new PrintQueue(this.Input).Silver()}";

        public string Gold() => $"{new PrintQueue(this.Input).Gold()}";       
    }
}
