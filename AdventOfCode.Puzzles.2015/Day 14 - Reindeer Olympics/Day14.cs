namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_14___Reindeer_Olympics;

    public class Day14 : Puzzle, IPuzzle
    {
        public Day14()
            : base(2015, 14, "Reindeer Olympics")
        {
        }

        public Day14(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new ReindeerOlympics(this.Input, 2503).RaceByDistance()}";

        public string Gold() => $"{new ReindeerOlympics(this.Input, 2503).RaceByScore()}";
    }
}
