namespace AdventOfCode.Puzzles._2015.Day_22___Wizard_Simulator_20XX
{
    using AdventOfCode.Core.Extensions;

    public abstract class Entity : IEntity
    {
        public Entity(int hitPoints, int damage, EntityClass entityClass)
        {
            this.HitPoints = hitPoints;
            this.Damage = damage;
            this.Defence = 0;
            this.Class = entityClass;
            this.Effects = new();
        }

        public Entity(int hitPoints, int manaPoints)
        {
            this.HitPoints = hitPoints;
            this.ManaPoints = manaPoints;
            this.Defence = 0;
            this.Class = EntityClass.Wizard;
            this.Effects = new();
        }

        public Entity(IEntity entity)
        {
            this.HitPoints = entity.HitPoints;
            this.Damage = entity.Damage;
            this.Defence = entity.Defence;
            this.Type = entity.Type;
            this.Class = entity.Class;
            this.ManaPoints = entity.ManaPoints;
            this.ManaSpent = entity.ManaSpent;
            this.Effects = entity.Effects.Select(effect => new Effect(effect.Spell, effect.Turns)).ToList();
        }

        public string Name => this.Type == EntityType.Player ? "Player" : "Boss";

        public EntityType Type { get; protected set; }

        public EntityClass Class { get; protected set; }

        public int HitPoints { get; protected set; }

        public int ManaPoints { get; protected set; }

        public int ManaSpent { get; protected set; }

        public int Damage { get; protected set; }

        public int Defence { get; protected set; }

        public bool Shield { get; protected set; }

        public List<Effect> Effects { get; protected set; }

        public bool AddEffect(Spell spell)
        {
            if (this.Effects.Any(x => x.Spell.Type == spell.Type))
            {
                return false;
            }

            this.Effects.Add(new(spell));

            if (spell.Type == SpellType.Shield)
            {
                this.Defence = 7;
            }

            return true;
        }

        public void ApplyEffects()
        {
            this.Defence = 0;

            foreach (Effect effect in this.Effects.ToArray())
            {
                if (effect.Turns > 0)
                {
                    effect.ReduceTurn();

                    switch (effect.Spell.Type)
                    {
                        case SpellType.Shield:
                            this.Defence += effect.Spell.Armor;
                            break;
                        case SpellType.Poison:
                            this.DecreaseHitPoints(effect.Spell.Damage);
                            break;
                        case SpellType.Recharge:
                            this.IncreaseManaPoints(effect.Spell.ManaRecovery);
                            break;
                    }

                    if (effect.Turns == 0)
                    {
                        this.Effects.Remove(effect);
                    }
                }
                else
                {
                    this.Effects.Remove(effect);
                }
            }
        }

        public int TotalAttack() => this.Damage;

        public int TotalDefence() => this.Defence;

        public void IncreaseHitPoints(int hitPoints) => this.HitPoints += hitPoints;

        public void DecreaseHitPoints(int hitPoints) => this.HitPoints -= hitPoints;

        public void Attack(IEntity defender)
        {
            int hitPoints = this.TotalAttack() - defender.TotalDefence();
            hitPoints = hitPoints < 1 ? 1 : hitPoints;

            defender.DecreaseHitPoints(hitPoints);
        }

        public void IncreaseManaPoints(int manaPoints) => this.ManaPoints += manaPoints;

        public void DecreaseManaPoints(int manaPoints)
        {
            this.ManaPoints -= manaPoints;
            this.ManaSpent += manaPoints;
        }

        public bool Cast(Spell spell, IEntity defender)
        {
            if (spell.IsEffect)
            {
                switch (spell.Type)
                {
                    case SpellType.Poison:
                        if (!defender.AddEffect(spell))
                        {
                            return false;
                        }

                        break;
                    case SpellType.Shield:
                    case SpellType.Recharge:
                        if (!this.AddEffect(spell))
                        {
                            return false;
                        }

                        break;
                }
            }
            else
            {
                defender.DecreaseHitPoints(spell.Damage);
                this.IncreaseHitPoints(spell.Recovery);
            }

            this.DecreaseManaPoints(spell.ManaCost);

            return true;
        }

        public bool IsAlive() => this.HitPoints > 0;
    }
}
