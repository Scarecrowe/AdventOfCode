namespace AdventOfCode.Puzzles._2018.Day_24___Immune_System_Simulator_20XX
{
    using AdventOfCode.Core.Extensions;

    public class Group
    {
        public Group(ArmyType type, int number, string line)
        {
            this.Number = number;
            this.Type = type;
            this.Weak = new List<AttackType>();
            this.Immune = new List<AttackType>();

            string[] tokens = line.Split(" with an attack that does ", StringSplitOptions.RemoveEmptyEntries);
            string[] attacking = tokens[1].Split(" damage at ", StringSplitOptions.RemoveEmptyEntries);
            string[] attack = attacking[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            tokens = tokens[0].Split(" hit points", StringSplitOptions.RemoveEmptyEntries);
            string[] units = tokens[0].Split(" units each with ", StringSplitOptions.RemoveEmptyEntries);

            this.AttackPoints = attack[0].ToInt();
            this.AttackType = this.StringToAttackType(attack[1]);
            this.Initiative = attacking[1].Split(" ")[1].ToInt();
            this.Units = units[0].ToInt();
            this.HitPoints = units[1].ToInt();

            if (tokens.Length > 1)
            {
                tokens = tokens[1].Split(";");

                foreach (string token in tokens)
                {
                    string value = token.Replace("(").Replace(")").Trim();

                    if (value.StartsWith("weak to"))
                    {
                        foreach (string weak in value.Replace("weak to ").Split(", "))
                        {
                            this.Weak.Add(this.StringToAttackType(weak));
                        }
                    }
                    else
                    {
                        foreach (string immune in value.Replace("immune to ", string.Empty).Split(", "))
                        {
                            this.Immune.Add(this.StringToAttackType(immune));
                        }
                    }
                }
            }
        }

        public Group(Group group, int boost)
        {
            this.Type = group.Type;
            this.AttackType = group.AttackType;
            this.AttackPoints = group.Type == ArmyType.ImmuneSystem ? group.AttackPoints + boost : group.AttackPoints;
            this.Units = group.Units;
            this.HitPoints = group.HitPoints;
            this.Initiative = group.Initiative;
            this.Weak = group.Weak;
            this.Immune = group.Immune;
            this.Number = group.Number;
        }

        public ArmyType Type { get; }

        public AttackType AttackType { get; }

        public int AttackPoints { get; }

        public int Units { get; private set; }

        public int HitPoints { get; }

        public int Initiative { get; }

        public List<AttackType> Weak { get; }

        public List<AttackType> Immune { get; }

        public Group? Target { get; set; }

        public int Number { get; }

        public int EffectivePower() => this.AttackPoints * this.Units;

        public Group Clone(int boost) => new(this, boost);

        public Group SetTarget(Group target)
        {
            this.Target = target;

            return this;
        }

        public Group Attack()
        {
            this.Target?.Defend(this);

            return this;
        }

        public Group Defend(Group attacker)
        {
            this.Units -= (int)Math.Floor((decimal)attacker.Damage(this) / this.HitPoints);

            return this;
        }

        public int Damage(Group target)
        {
            if (target.Immune.Contains(this.AttackType))
            {
                return 0;
            }
            else if (target.Weak.Contains(this.AttackType))
            {
                return this.EffectivePower() * 2;
            }

            return this.EffectivePower();
        }

        private AttackType StringToAttackType(string value)
            => (AttackType)Enum.Parse(typeof(AttackType), $"{value[0].ToString().ToUpper()}{value.Substring(1)}");
    }
}
