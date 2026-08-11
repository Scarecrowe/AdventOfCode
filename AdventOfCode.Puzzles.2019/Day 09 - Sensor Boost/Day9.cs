namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_09___Sensor_Boost;

    public class Day9 : Puzzle, IPuzzle
    {
        public Day9()
            : base(2019, 9, "Sensor Boost")
        {
        }

        public Day9(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new SensorBoost(this.Input[0]).RunTestMode()}";

        public string Gold() => $"{new SensorBoost(this.Input[0]).RunBoost()}";
    }
}
