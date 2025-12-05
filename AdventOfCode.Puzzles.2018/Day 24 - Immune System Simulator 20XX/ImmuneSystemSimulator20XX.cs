namespace AdventOfCode.Puzzles._2018.Day_24___Immune_System_Simulator_20XX
{
    public class ImmuneSystemSimulator20XX
    {
        public ImmuneSystemSimulator20XX(string[] input)
        {
            this.Targets = new();
            this.Groups = new();
            int i;

            for (i = 1; i < input.Length; i++)
            {
                if (string.IsNullOrEmpty(input[i]))
                {
                    break;
                }

                this.Groups.Add(new Group(ArmyType.ImmuneSystem, i, input[i]));
            }

            int number = 1;
            for (i += 2; i < input.Length; i++)
            {
                this.Groups.Add(new Group(ArmyType.Infection, number, input[i]));
                number++;
            }
        }

        public List<Group> Groups { get; private set; }

        public List<(Group army, int damage)> Targets { get; private set; }

        public int Battle()
        {
            while (this.Groups.Any(c => c.Type == ArmyType.ImmuneSystem)
                && this.Groups.Any(c => c.Type == ArmyType.Infection))
            {
                this.TargetSelection()
                    .Attack()
                    .ClearTargets();
            }

            return this.Groups.Sum(c => c.Units);
        }

        public int BattleWithBoost()
        {
            int boost = 1;
            List<Group> state = new();
            this.Groups.ForEach(c => state.Add(c.Clone(0)));

            while (true)
            {
                this.BattleWithExit();

                if (!this.Groups.Any(c => c.Type == ArmyType.Infection))
                {
                    int result = this.Groups.Sum(c => c.Units);
                    int i = 1;

                    while (true)
                    {
                        this.Groups.Clear();
                        state.ForEach(c => this.Groups.Add(c.Clone(boost - i)));
                        this.BattleWithExit();

                        if (this.Groups.Any(c => c.Type == ArmyType.Infection))
                        {
                            break;
                        }

                        result = this.Groups.Sum(c => c.Units);
                        i++;
                    }

                    return result;
                }

                boost += 10;
                this.Groups.Clear();
                state.ForEach(c => this.Groups.Add(c.Clone(boost)));
            }
        }

        private ImmuneSystemSimulator20XX TargetSelection()
        {
            foreach (IGrouping<ArmyType, Group> group in this.Groups
                .OrderByDescending(c => c.EffectivePower())
                .ThenByDescending(c => c.Initiative)
                .GroupBy(x => x.Type))
            {
                foreach (Group attacker in group)
                {
                    this.Targets.Clear();

                    foreach (Group defender in this.Groups.Where(x => x.Type != attacker.Type))
                    {
                        if (this.Groups.Any(c => c.Target == defender))
                        {
                            continue;
                        }

                        int damage = attacker.Damage(defender);

                        if (damage > 0)
                        {
                            this.Targets.Add((defender, damage));
                        }
                    }

                    if (this.Targets.Any())
                    {
                        attacker.SetTarget(this.Targets
                        .OrderByDescending(c => c.damage)
                        .ThenByDescending(c => c.army.EffectivePower())
                        .ThenByDescending(c => c.army.Initiative)
                        .First().army);
                    }
                }
            }

            return this;
        }

        private ImmuneSystemSimulator20XX Attack()
        {
            this.Groups = this.Groups.OrderByDescending(c => c.Initiative).ToList();

            for (int i = 0; i < this.Groups.Count; i++)
            {
                Group attacker = this.Groups[i];

                if (attacker.Target == null)
                {
                    continue;
                }

                attacker.Attack();

                if (attacker.Target.Units <= 0)
                {
                    int index = this.Groups.IndexOf(attacker.Target);
                    this.Groups.RemoveAt(index);

                    if (index < i)
                    {
                        i--;
                    }
                }
            }

            return this;
        }

        private ImmuneSystemSimulator20XX ClearTargets()
        {
            this.Groups.ForEach(c => c.Target = null);

            return this;
        }

        private void BattleWithExit()
            {
                while (this.Groups.Any(c => c.Type == ArmyType.ImmuneSystem)
                       && this.Groups.Any(c => c.Type == ArmyType.Infection))
                {
                    int units = this.Groups.Sum(c => c.Units);

                    this.TargetSelection()
                        .Attack()
                        .ClearTargets();

                    if (units == this.Groups.Sum(c => c.Units))
                    {
                        break;
                    }
                }
            }
        }
}
