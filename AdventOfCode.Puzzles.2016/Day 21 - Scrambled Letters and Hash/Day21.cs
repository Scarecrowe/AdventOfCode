namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_21___Scrambled_Letters_and_Hash;

    public class Day21 : Puzzle, IPuzzle
    {
        public Day21()
            : base(2016, 21, "Scrambled Letters and Hash")
        {
        }

        public Day21(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new ScrambledLettersAndHash(this.Input).Scramble("abcdefgh")}";

        public string Gold() => $"{new ScrambledLettersAndHash(this.Input).Unscramble("fbgdceah")}";
    }
}
