namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_21___Springdroid_Adventure;

    public class Day21 : Puzzle, IPuzzle
    {
        public Day21(string file)
        {
            this.DayTitle = "Springdroid Adventure";
            this.GetPuzzleData(file);
        }

        public Day21(string[] input) => this.Input = input;

        public string Silver() => $"{new SpringdroidAdventure(this.Input[0]).Run()}";

        public string Gold() => $"{new SpringdroidAdventure(this.Input[0]).RunWihtInreasedSensor()}";
    }
}
