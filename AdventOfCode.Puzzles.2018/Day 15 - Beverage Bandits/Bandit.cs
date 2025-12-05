namespace AdventOfCode.Puzzles._2018.Day_15___Beverage_Bandits
{
    using AdventOfCode.Core;

    public class Bandit
    {
        public Bandit(char type, Vector<int> point, int healthPoints, int attackPoints)
        {
            this.Letter = type;
            this.Type = this.Letter == 'E' ? BanditType.Elf : BanditType.Goblin;
            this.Point = point;
            this.HealthPoints = healthPoints;
            this.AttackPoints = attackPoints;
        }

        public BanditType Type { get; }

        public Vector<int> Point { get; }

        public char Letter { get; }

        public int HealthPoints { get; private set; }

        public int AttackPoints { get; private set; }

        public BanditType DefenderType() => this.Type == BanditType.Elf ? BanditType.Goblin : BanditType.Elf;

        public new string ToString() => $"{this.Type}: ({this.Point.X},{this.Point.Y}) HP: {this.HealthPoints}";

        public void Move(Vector<int> point)
        {
            this.Point.X = point.X;
            this.Point.Y = point.Y;
        }

        public void Attack(Bandit bandit) => bandit.Defend(this);

        public void Defend(Bandit bandit) => this.HealthPoints -= bandit.AttackPoints;
    }
}
