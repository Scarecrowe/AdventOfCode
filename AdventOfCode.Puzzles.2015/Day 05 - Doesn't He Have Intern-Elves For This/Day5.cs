namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_05___Doesn_t_He_Have_Intern_Elves_For_This;

    public class Day5 : Puzzle, IPuzzle
    {
        public Day5()
            : base(2015, 5, "Doesn't He Have Intern-Elves For This", StringSplitOptions.None)
        {
        }

        public Day5(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new DoesntHeHaveInternElvesForThis(this.Input, ChristmasListModel.Normal).Nice}";

        public string Gold() => $"{new DoesntHeHaveInternElvesForThis(this.Input, ChristmasListModel.Advanced).Nice}";
    }
}
