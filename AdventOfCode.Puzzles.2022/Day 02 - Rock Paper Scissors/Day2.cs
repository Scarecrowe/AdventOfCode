namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_02___Rock_Paper_Scissors;

    public class Day2 : Puzzle, IPuzzle
    {
        public Day2()
            : base(2022, 2, "Rock Paper Scissors")
        {
        }

        public Day2(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new RockPaperScissors<PlayerBShape>(this.Input).Play()}";

        public string Gold() => $"{new RockPaperScissors<RoundResult>(this.Input).Play()}";
    }
}
