namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_22___Crab_Combat;

    public class Day22 : Puzzle, IPuzzle
    {
        public Day22(string file)
        {
            this.DayTitle = "Crab Combat";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day22(string[] input) => this.Input = input;

        public string Silver() => $"{new CrabCombat(this.Input).Play()}";

        public string Gold()
        {
            CrabCombat combat = new(this.Input);

            return $"{combat.PlayRecursive(combat.Players, 1).Score()}";
        }
    }
}
