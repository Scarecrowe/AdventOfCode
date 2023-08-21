namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_13___Knights_of_the_Dinner_Table;

    public class Day13 : Puzzle, IPuzzle
    {
        public Day13(string file)
        {
            this.DayTitle = "Knights of the Dinner Table";
            this.GetPuzzleData(file, StringSplitOptions.None);
        }

        public Day13(string[] input) => this.Input = input;

        public string Silver() => $"{new KnightsOfTheDinnerTable(this.Input).OptimalSeating()}";

        public string Gold() => $"{new KnightsOfTheDinnerTable(this.Input).AddPerson("Scarecrowe").OptimalSeating()}";
    }
}
