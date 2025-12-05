namespace AdventOfCode.Puzzles._2023.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2023.Day_14___Parabolic_Reflector_Dish;

    public class Day14 : Puzzle, IPuzzle
    {
        public Day14()
        {
            this.DayTitle = "Parabolic Reflector Dish";
            this.GetPuzzleData(14, this.DayTitle, StringSplitOptions.None);
        }

        public string Silver() => $"{new ParabolicReflectorDish(this.Input)}";

        public string Gold() => $"{new ParabolicReflectorDish(this.Input)}";
    }
}
