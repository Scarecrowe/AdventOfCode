namespace AdventOfCode.Puzzles._2015.Day_22___Wizard_Simulator_20XX
{
    public interface IEntity
    {
        string Name { get; }

        EntityType Type { get; }

        EntityClass Class { get; }

        int HitPoints { get; }

        int ManaPoints { get; }

        int Damage { get; }

        int Defence { get; }

        int ManaSpent { get; }

        List<Effect> Effects { get; }

        bool AddEffect(Spell spell);

        void ApplyEffects();

        void Attack(IEntity defender);

        bool Cast(Spell spell, IEntity defender);

        int TotalAttack();

        int TotalDefence();

        void IncreaseHitPoints(int hitPoints);

        void DecreaseHitPoints(int hitPoints);

        void IncreaseManaPoints(int manaPoints);

        void DecreaseManaPoints(int manaPoints);

        bool IsAlive();
    }
}
