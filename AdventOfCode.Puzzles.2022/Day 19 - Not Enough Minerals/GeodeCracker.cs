namespace AdventOfCode.Puzzles._2022.Day_19___Not_Enough_Minerals
{
    using AdventOfCode.Core.Extensions;

    public class GeodeCracker
    {
        public GeodeCracker(string[] input)
        {
            this.States = new();
            this.BluePrints = input.Select(x => new BluePrint(x)).ToList();
        }

        public List<BluePrint> BluePrints { get; private set; }

        public Dictionary<string, int> States { get; private set; }

        public int Run24() => this.BluePrints.Select((x, i) => this.RunBluePrint(x, 24) * (i + 1)).Sum();

        public long Run32() => this.BluePrints.Take(3).Select((x) => this.RunBluePrint(x, 32)).Product();

        public int RunBluePrint(BluePrint bluePrint, int totalTime)
        {
            int result = 0;

            Queue<GeodeCrackerState> queue = new();
            this.States.Clear();

            queue.Enqueue(new(bluePrint, totalTime));

            while (queue.Count > 0)
            {
                GeodeCrackerState state = queue.Dequeue();

                if (state.Minutes <= 0)
                {
                    if (state.Minerals[MineralType.Geode] > result)
                    {
                        result = state.Minerals[MineralType.Geode];
                    }

                    continue;
                }

                state.Cycle(queue, this.States, totalTime);
            }

            return result;
        }
    }
}
