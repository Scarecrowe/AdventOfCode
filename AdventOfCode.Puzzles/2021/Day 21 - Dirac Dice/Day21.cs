namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_21___Dirac_Dice;

    public class Day21 : Puzzle, IPuzzle
    {
        public Day21(string file)
        {
            this.DayTitle = "Dirac Dice";
            this.GetPuzzleData(file);
        }

        public Day21(string[] input) => this.Input = input;

        public string Silver() => $"{new DiracDice(this.Input).Play()}";

        public string Gold() => $"{new DiracDice(this.Input).PlayQuantum()}";
    }
}
