namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_01___Chronal_Calibration;

    public class Day1 : Puzzle, IPuzzle
    {
        public Day1()
        {
            this.DayTitle = "Inverse Captcha";
            this.GetPuzzleData(1, this.DayTitle);
        }

        public Day1(string[] input) => this.Input = input;

        public string Silver() => $"{new ChronalCalibration(this.Input).Run().Frequency}";

        public string Gold() => $"{new ChronalCalibration(this.Input).Run(true).Frequency}";
    }
}
