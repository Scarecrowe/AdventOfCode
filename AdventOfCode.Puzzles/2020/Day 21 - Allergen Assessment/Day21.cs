namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_21___Allergen_Assessment;

    public class Day21 : Puzzle, IPuzzle
    {
        public Day21(string file)
        {
            this.DayTitle = "Allergen Assessment";
            this.GetPuzzleData(file);
        }

        public Day21(string[] input) => this.Input = input;

        public string Silver() => $"{AllergenAssessment.NonAllergens(this.Input)}";

        public string Gold() => $"{AllergenAssessment.Dangerous(this.Input)}";
    }
}
