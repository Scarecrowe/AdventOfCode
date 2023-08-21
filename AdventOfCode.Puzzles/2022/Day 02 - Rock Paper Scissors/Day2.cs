namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_02___Rock_Paper_Scissors;

    public class Day2 : Puzzle, IPuzzle
    {
        public Day2(string file)
        {
            this.DayTitle = "Rock Paper Scissors";
            this.GetPuzzleData(file);
        }

        public Day2(string[] input) => this.Input = input;

        public string Silver() => $"{new RockPaperScissors<PlayerBShape>(this.Input).Play()}";

        public string Gold() => $"{new RockPaperScissors<RoundResult>(this.Input).Play()}";
    }
}
