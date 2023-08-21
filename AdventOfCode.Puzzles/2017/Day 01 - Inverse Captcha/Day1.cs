namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_01___Inverse_Captcha;

    public class Day1 : Puzzle, IPuzzle
    {
        public Day1(string file)
        {
            this.DayTitle = "Chronal Calibration";
            this.GetPuzzleData(file);
        }

        public Day1(string[] input) => this.Input = input;

        public string Silver() => $"{InverseCaptcha.Simple(this.Input[0])}";

        public string Gold() => $"{InverseCaptcha.Advanced(this.Input[0])}";
    }
}
