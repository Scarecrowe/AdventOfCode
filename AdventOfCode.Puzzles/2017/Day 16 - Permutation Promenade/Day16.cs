namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_16___Permutation_Promenade;

    public class Day16 : Puzzle, IPuzzle
    {
        public Day16(string file)
        {
            this.DayTitle = "Permutation Promenade";
            this.GetPuzzleData(file);
        }

        public Day16(string[] input) => this.Input = input;

        public string Silver() => $"{new PermutationPromenade(this.Input).Sort(PermutationPromenade.InitialValue())}";

        [Slow]
        public string Gold() => $"{new PermutationPromenade(this.Input).Sort(PermutationPromenade.InitialValue(), 1000000000)}";
    }
}
