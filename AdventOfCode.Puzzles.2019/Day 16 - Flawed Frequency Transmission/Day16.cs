namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_16___Flawed_Frequency_Transmission;

    public class Day16 : Puzzle, IPuzzle
    {
        public Day16()
            : base(2019, 16, "Flawed Frequency Transmission")
        {
        }

        public Day16(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{FlawedFrequencyTransmission.Single(this.Input)}";

        public string Gold() => $"{FlawedFrequencyTransmission.Multiple(this.Input)}";
    }
}
