namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_05___Sunny_with_a_Chance_of_Asteroids;

    public class Day5 : Puzzle, IPuzzle
    {
        public Day5()
            : base(2019, 5, "Sunny with a Chance of Asteroids")
        {
        }

        public Day5(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new SunnyWithAChanceOfAsteroids(this.Input[0]).AirConditionerDiagnostics()}";

        public string Gold() => $"{new SunnyWithAChanceOfAsteroids(this.Input[0]).ThermalRadiatorDiagnostics()}";
    }
}
