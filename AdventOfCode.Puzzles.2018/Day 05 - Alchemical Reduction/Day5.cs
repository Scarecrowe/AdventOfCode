namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_05___Alchemical_Reduction;

    public class Day5 : Puzzle, IPuzzle
    {
        public Day5()
            : base(2018, 5, "Alchemical Reduction")
        {
        }

        public Day5(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new AlchemicalReduction(this.Input[0]).React().Length}";

        [Slow]
        public string Gold() => $"{new AlchemicalReduction(this.Input[0]).ShortestPolymer()}";
    }
}
