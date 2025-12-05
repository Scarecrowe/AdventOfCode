namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_19___Medicine_for_Rudolph;

    public class Day19 : Puzzle, IPuzzle
    {
        public Day19()
        {
            this.DayTitle = "Medicine for Rudolph";
            this.GetPuzzleData(19, this.DayTitle, StringSplitOptions.None);
        }

        public Day19(string[] input) => this.Input = input;

        public string Silver() => $"{new MedicineForRudolph(this.Input).CreateMolecules().Count}";

        public string Gold() => $"{new MedicineForRudolph(this.Input).FabricateMolecule()}";
    }
}
