namespace AdventOfCode.Puzzles._2015.Day_22___Wizard_Simulator_20XX
{
    public class Effect
    {
        public Effect(Spell spell)
        {
            this.Spell = spell;
            this.Turns = spell.Turns;
        }

        public Effect(Spell spell, int turns)
        {
            this.Spell = spell;
            this.Turns = turns;
        }

        public int Turns { get; protected set; }

        public Spell Spell { get; }

        public void ReduceTurn() => this.Turns--;
    }
}
