namespace AdventOfCode.Puzzles._2015.Day_22___Wizard_Simulator_20XX
{
    public class Spells : Dictionary<SpellType, Spell>
    {
        public Spells()
        {
            this.Add(SpellType.MagicMissile, new Spell(SpellType.MagicMissile, "Magic Missile", 53, 4, 0));
            this.Add(SpellType.Drain, new Spell(SpellType.Drain, "Drain", 73, 2, 2));
            this.Add(SpellType.Shield, new Spell(SpellType.Shield, "Shield", 113, 0, 0, 0, 7, EntityType.Player, 6));
            this.Add(SpellType.Poison, new Spell(SpellType.Poison, "Poison", 173, 0, 3, 0, 0, EntityType.Enemy, 6));
            this.Add(SpellType.Recharge, new Spell(SpellType.Recharge, "Recharge", 229, 101, 0, 0, 0, EntityType.Player, 5));
        }
    }
}
