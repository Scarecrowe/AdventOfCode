namespace AdventOfCode.Puzzles._2015.Day_21___RPG_Simulator_20XX
{
    public interface IEntity
    {
        EntityType Type { get; }

        int HitPoints { get; }

        int Strength { get; }

        int Defence { get; }

        int TotalAttack();

        int TotalDefence();

        void Attack(IEntity defender);

        void TakeDamage(int damage);

        void EquipItem(Item item);

        void EquipItems(List<Item> items);
    }
}
