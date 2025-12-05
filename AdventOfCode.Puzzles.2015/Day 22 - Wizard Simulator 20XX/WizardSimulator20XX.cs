namespace AdventOfCode.Puzzles._2015.Day_22___Wizard_Simulator_20XX
{
    using AdventOfCode.Core.Extensions;

    public class WizardSimulator20XX
    {
        public WizardSimulator20XX(IEntity player, IEntity enemy)
        {
            this.Spells = new();
            this.InitalEnemy = enemy;
            this.InitalPlayer = player;
        }

        private Spells Spells { get; set; }

        private IEntity InitalEnemy { get; set; }

        private IEntity InitalPlayer { get; set; }

        public static Warrior LoadEnemy(string[] input)
        {
            int hitPoints = 0;
            int damage = 0;

            for (int i = 0; i < input.Length; i++)
            {
                string[] tokens = input[i].Split(": ");

                switch (tokens[0])
                {
                    case "Hit Points":
                        hitPoints = tokens[1].ToInt();
                        break;
                    case "Damage":
                        damage = tokens[1].ToInt();
                        break;
                }
            }

            return new Warrior(hitPoints, damage);
        }

        public bool Battle((List<SpellType> Spells, IEntity Wizard, IEntity Boss) state, BattleMode mode)
        {
            if (mode == BattleMode.Hard)
            {
                state.Wizard.DecreaseHitPoints(1);

                if (!state.Wizard.IsAlive())
                {
                    return false;
                }
            }

            Spell spell = this.Spells[state.Spells[^1]];

            if (spell.ManaCost > state.Wizard.ManaPoints)
            {
                return false;
            }

            state.Boss.ApplyEffects();
            state.Wizard.ApplyEffects();

            if (!state.Boss.IsAlive())
            {
                return false;
            }

            if (!state.Wizard.Cast(spell, state.Boss))
            {
                return false;
            }

            state.Boss.ApplyEffects();
            state.Wizard.ApplyEffects();

            if (!state.Boss.IsAlive())
            {
                return false;
            }

            state.Boss.Attack(state.Wizard);

            return true;
        }

        public int MinMana(BattleMode mode)
        {
            int result = int.MaxValue;

            PriorityQueue<(List<SpellType> Spells, IEntity Wizard, IEntity Boss), int> queue = new();

            foreach (KeyValuePair<SpellType, Spell> spell in this.Spells)
            {
                queue.Enqueue((new List<SpellType> { spell.Key }, new Wizard(this.InitalPlayer), new Warrior(this.InitalEnemy)), 50);
            }

            while (queue.Count > 0)
            {
                (List<SpellType> Spells, IEntity Wizard, IEntity Boss) state = queue.Dequeue();

                if (state.Wizard.ManaSpent > result)
                {
                    continue;
                }

                if (state.Wizard.ManaPoints < this.Spells[state.Spells.Last()].ManaCost)
                {
                    continue;
                }

                var active = this.Battle(state, mode);

                if (state.Boss.HitPoints <= 0)
                {
                    if (state.Wizard.ManaSpent < result)
                    {
                        result = state.Wizard.ManaSpent;
                    }

                    continue;
                }

                if (state.Wizard.HitPoints > 0 && active)
                {
                    foreach (KeyValuePair<SpellType, Spell> spell in this.Spells)
                    {
                        if ((spell.Key == SpellType.Shield
                            || spell.Key == SpellType.Poison
                            || spell.Key == SpellType.Recharge)
                            && state.Spells.Last() == spell.Key)
                        {
                            continue;
                        }

                        queue.Enqueue((new List<SpellType>(state.Spells) { spell.Key }, new Wizard(state.Wizard), new Warrior(state.Boss)), state.Wizard.HitPoints);
                    }
                }
            }

            return result;
        }
    }
}
