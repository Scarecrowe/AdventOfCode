namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_01___The_Tyranny_of_the_Rocket_Equation;

    public class Day1 : Puzzle, IPuzzle
    {
        public Day1()
            : base(2019, 1, "The Tyranny of the Rocket Equation")
        {
        }

        public Day1(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{RocketEquation.FuelRequirements(this.Input)}";

        public string Gold() => $"{RocketEquation.FullFuelRequirements(this.Input)}";
    }
}
