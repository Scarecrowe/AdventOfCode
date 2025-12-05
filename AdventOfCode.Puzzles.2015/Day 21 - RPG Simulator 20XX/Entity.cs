namespace AdventOfCode.Puzzles._2015.Day_21___RPG_Simulator_20XX
{
    public abstract class Entity : IEntity
    {
        public Entity(int hitPoints, int strength, int defence)
        {
            this.HitPoints = hitPoints;
            this.Strength = strength;
            this.Defence = defence;
            this.Rings = new();
        }

        public Entity(IEntity entity)
        {
            this.HitPoints = entity.HitPoints;
            this.Strength = entity.Strength;
            this.Defence = entity.Defence;
            this.Rings = new();
            this.Type = entity.Type;
        }

        public EntityType Type { get; protected set; }

        public int HitPoints { get; protected set; }

        public int Strength { get; protected set; }

        public int Defence { get; protected set; }

        public Item? Weapon { get; protected set; }

        public Item? Armor { get; protected set; }

        public List<Item> Rings { get; protected set; }

        public int TotalAttack() => this.Strength
                + (this.Weapon == null ? 0 : this.Weapon.Power)
                + this.Rings.Sum(x => x.Power);

        public int TotalDefence() => this.Defence
                + (this.Armor == null ? 0 : this.Armor.Defence)
                + this.Rings.Sum(x => x.Defence);

        public void TakeDamage(int damage) => this.HitPoints -= damage;

        public void EquipItem(Item item)
        {
            switch (item.Type)
            {
                case ItemType.Weapon:
                    this.Weapon = item;
                    break;
                case ItemType.Armor:
                    this.Armor = item;
                    break;
                case ItemType.Ring:
                    this.Rings.Add(item);
                    break;
            }
        }

        public void EquipItems(List<Item> items) => items.ForEach(item => this.EquipItem(item));

        public void Attack(IEntity defender)
        {
            int hitPoints = this.TotalAttack() - defender.TotalDefence();
            hitPoints = hitPoints < 1 ? 1 : hitPoints;
            defender.TakeDamage(hitPoints);
        }
    }
}
