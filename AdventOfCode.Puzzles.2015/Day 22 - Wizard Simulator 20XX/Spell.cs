namespace AdventOfCode.Puzzles._2015.Day_22___Wizard_Simulator_20XX
{
    public class Spell
    {
        public Spell(
            SpellType type,
            string name,
            int manaCost,
            int damage,
            int recovery)
        {
            this.Type = type;
            this.Name = name;
            this.ManaCost = manaCost;
            this.Damage = damage;
            this.Recovery = recovery;
        }

        public Spell(
            SpellType type,
            string name,
            int manaCost,
            int manaRecovery,
            int damage,
            int recovery,
            int armor,
            EntityType effects,
            int effectTurns)
        {
            this.Type = type;
            this.Name = name;
            this.ManaCost = manaCost;
            this.ManaRecovery = manaRecovery;
            this.Damage = damage;
            this.Recovery = recovery;
            this.Armor = armor;
            this.IsEffect = true;
            this.Effects = effects;
            this.Turns = effectTurns;
        }

        public SpellType Type { get; }

        public string Name { get; private set; }

        public int ManaCost { get; private set; }

        public int ManaRecovery { get; private set; }

        public int Damage { get; private set; }

        public int Recovery { get; private set; }

        public int Armor { get; private set; }

        public bool IsEffect { get; private set; }

        public EntityType Effects { get; }

        public int Turns { get; private set; }

        public new string ToString() => $"{this.Type}";
    }
}
