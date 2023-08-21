namespace AdventOfCode.Puzzles._2021.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2021.Day_01___Sonar_Sweep;

    public class Day1 : Puzzle, IPuzzle
    {
        public Day1(string file)
        {
            this.DayTitle = "Sonar Sweep";
            this.GetPuzzleData(file);
        }

        public Day1(string[] input) => this.Input = input;

        public string Silver() => $"{SonarSweep.LargerThan(this.Input)}";

        public string Gold() => $"{SonarSweep.LargerThan(SonarSweep.GroupAndSum(this.Input))}";
    }
}
