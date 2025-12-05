namespace AdventOfCode.Puzzles._2015.Day_22___Wizard_Simulator_20XX
{
    public class Wizard : Entity, IEntity
    {
        public Wizard(int hitPoints, int manaPoints)
            : base(hitPoints, manaPoints) => this.Type = EntityType.Player;

        public Wizard(IEntity entity)
            : base(entity)
        {
        }
    }
}
