namespace AdventOfCode.Puzzles._2015.Day_21___RPG_Simulator_20XX
{
    public class Warrior : Entity, IEntity
    {
        public Warrior(int hitPoints, int strength, int defence)
            : base(hitPoints, strength, defence)
        {
            this.Type = EntityType.Player;
        }

        public Warrior(IEntity entity)
            : base(entity)
        {
        }
    }
}
