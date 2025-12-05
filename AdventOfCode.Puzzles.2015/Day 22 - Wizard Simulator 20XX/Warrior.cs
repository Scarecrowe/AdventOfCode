namespace AdventOfCode.Puzzles._2015.Day_22___Wizard_Simulator_20XX
{
    public class Warrior : Entity, IEntity
    {
        public Warrior(int hitPoints, int damage, EntityClass @class = EntityClass.Warrior)
            : base(hitPoints, damage, @class) => this.Type = EntityType.Enemy;

        public Warrior(IEntity entity)
            : base(entity)
        {
        }
    }
}
