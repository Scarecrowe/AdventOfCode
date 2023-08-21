namespace AdventOfCode.Puzzles._2015.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2015.Day_22___Wizard_Simulator_20XX;

    public class Day22 : Puzzle, IPuzzle
    {
        public Day22(string file)
        {
            this.DayTitle = "Wizard Simulator 20XX";
            this.GetPuzzleData(file);
        }

        public Day22(string[] input) => this.Input = input;

        public string Silver() => $"{new WizardSimulator20XX(new Wizard(50, 500), WizardSimulator20XX.LoadEnemy(this.Input)).MinMana(BattleMode.Easy)}";

        public string Gold() => $"{new WizardSimulator20XX(new Wizard(50, 500), WizardSimulator20XX.LoadEnemy(this.Input)).MinMana(BattleMode.Hard)}";
    }
}
