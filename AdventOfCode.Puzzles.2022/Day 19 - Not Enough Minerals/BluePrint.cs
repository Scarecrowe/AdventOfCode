namespace AdventOfCode.Puzzles._2022.Day_19___Not_Enough_Minerals
{
    using AdventOfCode.Core.Extensions;

    public class BluePrint
    {
        public BluePrint(string input)
        {
            this.Ore = new();
            this.Clay = new();
            this.Obsidian = new();
            this.Geode = new();

            string[] robots = input.Split(": ", StringSplitOptions.RemoveEmptyEntries)[1].Split(".", StringSplitOptions.RemoveEmptyEntries);

            foreach (string robot in robots)
            {
                string current = robot.Replace("Each ").Trim();

                if (current.StartsWith("ore"))
                {
                    this.Ore = new() { { MineralType.Ore, current.Replace("ore robot costs ").Replace(" ore").ToInt() } };
                }
                else if (current.StartsWith("clay"))
                {
                    this.Clay = new() { { MineralType.Ore, current.Replace("clay robot costs ").Replace(" ore").ToInt() } };
                }
                else if (current.StartsWith("obsidian"))
                {
                    int[] values = current.Replace("obsidian robot costs ").Replace("ore and ").Replace(" clay").SplitSpace().ToInt();
                    this.Obsidian = new() { { MineralType.Ore, values[0] }, { MineralType.Clay, values[1] } };
                }
                else
                {
                    int[] values = current.Replace("geode robot costs ").Replace("ore and ").Replace(" obsidian").SplitSpace().ToInt();
                    this.Geode = new() { { MineralType.Ore, values[0] }, { MineralType.Obsidian, values[1] } };
                }
            }
        }

        public Dictionary<MineralType, int> Ore { get; private set; }

        public Dictionary<MineralType, int> Clay { get; private set; }

        public Dictionary<MineralType, int> Obsidian { get; private set; }

        public Dictionary<MineralType, int> Geode { get; private set; }

        public Dictionary<MineralType, int> BuildQuantitiesFor(MineralType type)
        {
            return type switch
            {
                MineralType.Ore => this.Ore,
                MineralType.Clay => this.Clay,
                MineralType.Obsidian => this.Obsidian,
                MineralType.Geode => this.Geode,
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
