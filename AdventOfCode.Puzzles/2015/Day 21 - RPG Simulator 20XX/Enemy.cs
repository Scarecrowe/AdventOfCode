namespace AdventOfCode.Puzzles._2015.Day_21___RPG_Simulator_20XX
{
    public class Enemy : Entity, IEntity
    {
        public Enemy(int hitPoints, int strength, int defence)
            : base(hitPoints, strength, defence)
        {
            this.Type = EntityType.Enemy;
        }

        public Enemy(IEntity entity)
            : base(entity)
        {
        }
    }
}
