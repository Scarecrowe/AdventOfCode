namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_07___Internet_Protocol_Version_7;

    public class Day7 : Puzzle, IPuzzle
    {
        public Day7()
            : base(2016, 7, "Internet Protocol Version 7")
        {
        }

        public Day7(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new InternetProtocolVersion7(this.Input).SupportsTls()}";

        public string Gold() => $"{new InternetProtocolVersion7(this.Input).SupportsSsl()}";
    }
}
