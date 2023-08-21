namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_24___Planet_of_Discord;

    public class Day24 : Puzzle, IPuzzle
    {
        public Day24(string file)
        {
            this.DayTitle = "Planet of Discord";
            this.GetPuzzleData(file);
        }

        public Day24(string[] input) => this.Input = input;

        public string Silver() => $"{new PlanetOfDiscord(this.Input).BiodiversityRating()}";

        public string Gold() => $"{new PlanetOfDiscord(this.Input).BugCount()}";
    }
}
