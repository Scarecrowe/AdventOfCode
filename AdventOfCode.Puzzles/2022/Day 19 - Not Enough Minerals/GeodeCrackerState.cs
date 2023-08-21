namespace AdventOfCode.Puzzles._2022.Day_19___Not_Enough_Minerals
{
    using System.Text;

    public class GeodeCrackerState
    {
        public GeodeCrackerState(BluePrint bluePrint, int duration)
        {
            this.Robots = new()
            {
                { MineralType.Ore, 1 },
                { MineralType.Clay, 0 },
                { MineralType.Obsidian, 0 },
                { MineralType.Geode, 0 }
            };

            this.Minerals = new()
            {
                { MineralType.Ore, 0 },
                { MineralType.Clay, 0 },
                { MineralType.Obsidian, 0 },
                { MineralType.Geode, 0 }
            };

            this.BluePrint = bluePrint;
            this.Minutes = duration;
        }

        public GeodeCrackerState(GeodeCrackerState state)
        {
            this.Robots = new();
            this.Minerals = new();
            this.BluePrint = state.BluePrint;

            foreach (var robot in state.Robots)
            {
                this.Robots.Add(robot.Key, robot.Value);
            }

            foreach (var mineral in state.Minerals)
            {
                this.Minerals.Add(mineral.Key, mineral.Value);
            }

            this.Minutes = state.Minutes;
        }

        public Dictionary<MineralType, int> Robots { get; private set; }

        public Dictionary<MineralType, int> Minerals { get; private set; }

        public BluePrint BluePrint { get; private set; }

        public int Minutes { get; private set; }

        public GeodeCrackerState Clone() => new(this);

        public bool ShouldBuildRobotFor(MineralType type)
        {
            foreach (var quantities in this.BluePrint.BuildQuantitiesFor(type))
            {
                if (this.Robots[quantities.Key] <= quantities.Value)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanBuildRobotFor(MineralType type)
        {
            foreach (var mineralRequired in this.BluePrint.BuildQuantitiesFor(type))
            {
                if (this.Minerals[mineralRequired.Key] < mineralRequired.Value)
                {
                    return false;
                }
            }

            return true;
        }

        public GeodeCrackerState BuildRobotFor(MineralType type)
        {
            foreach (var mineral in this.BluePrint.BuildQuantitiesFor(type))
            {
                this.Minerals[mineral.Key] -= mineral.Value;
            }

            this.Robots[type]++;

            return this;
        }

        public GeodeCrackerState CollectMinerals()
        {
            foreach (var robot in this.Robots)
            {
                this.Minerals[robot.Key] += robot.Value;
            }

            return this;
        }

        public bool TryEnqueue(GeodeCrackerState state, Queue<GeodeCrackerState> queue, Dictionary<string, int> states)
        {
            string key = state.ToKey();
            if (states.ContainsKey(key))
            {
                if (state.TotalMinerals() > states[key] - 1)
                {
                    states[key] = state.TotalMinerals();
                    queue.Enqueue(state);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                states.Add(key, state.TotalMinerals());
                queue.Enqueue(state);
                return true;
            }
        }

        public void Cycle(Queue<GeodeCrackerState> queue, Dictionary<string, int> states, int totalTime)
        {
            this.Minutes--;

            var clone = this.Clone().CollectMinerals();

            this.TryEnqueue(clone, queue, states);

            if (this.CanBuildRobotFor(MineralType.Geode))
            {
                this.TryEnqueue(this.Clone().CollectMinerals().BuildRobotFor(MineralType.Geode), queue, states);
                return;
            }

            if (this.CanBuildRobotFor(MineralType.Obsidian))
            {
                this.TryEnqueue(this.Clone().CollectMinerals().BuildRobotFor(MineralType.Obsidian), queue, states);
            }

            if (((totalTime == 24 && this.Minutes > 5) || (totalTime == 32 && this.Minutes > 10)) && this.ShouldBuildRobotFor(MineralType.Clay) && this.CanBuildRobotFor(MineralType.Clay))
            {
                this.TryEnqueue(this.Clone().CollectMinerals().BuildRobotFor(MineralType.Clay), queue, states);
            }

            if (((totalTime == 24 && this.Minutes > 5) || (totalTime == 32 && this.Minutes > 10)) && this.CanBuildRobotFor(MineralType.Ore))
            {
                this.TryEnqueue(this.Clone().CollectMinerals().BuildRobotFor(MineralType.Ore), queue, states);
            }

            return;
        }

        public string ToKey()
        {
            StringBuilder result = new();

            foreach (var robot in this.Robots)
            {
                result.Append($"{robot.Key},{robot.Value}");
            }

            return result.ToString();
        }

        public int TotalMinerals()
        {
            int result = 0;

            foreach (var mineral in this.Minerals)
            {
                result += mineral.Value;
            }

            return result;
        }
    }
}
