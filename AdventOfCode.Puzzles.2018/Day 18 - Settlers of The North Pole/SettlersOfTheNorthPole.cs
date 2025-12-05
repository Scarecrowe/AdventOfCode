namespace AdventOfCode.Puzzles._2018.Day_18___Settlers_of_The_North_Pole
{
    using AdventOfCode.Core;

    public class SettlersOfTheNorthPole
    {
        public SettlersOfTheNorthPole(string[] input) => this.Map = new(input, ".|#");

        private VectorArray<int, EntityType> Map { get; }

        public long Cycle(int minutes)
        {
            List<long> states = new();

            for (int i = 1; i <= minutes; i++)
            {
                VectorArray<int, EntityType> state = this.Map.Clone();

                foreach(VectorCell<int, EntityType> cell in state.AxisEnumerator())
                {
                    List<VectorCell<int, EntityType>> adjacent = state.AdjacentInterCardinal(cell.Point).ToList();

                    switch (cell.Value)
                    {
                        case EntityType.Open:
                            if (adjacent.Count(c => c.Value == EntityType.Trees) >= 3)
                            {
                                this.Map[cell.Point] = EntityType.Trees;
                            }

                            break;
                        case EntityType.Trees:
                            if (adjacent.Count(c => c.Value == EntityType.LumberyYard) >= 3)
                            {
                                this.Map[cell.Point] = EntityType.LumberyYard;
                            }

                            break;
                        case EntityType.LumberyYard:
                            this.Map[cell.Point] = (adjacent.Any(c => c.Value == EntityType.LumberyYard)
                                && adjacent.Any(c => c.Value == EntityType.Trees))
                                ? EntityType.LumberyYard : EntityType.Open;
                            break;
                    }
                }

                states.Add(this.Score());

                for (int j = 1; j < states.Count - 1; j++)
                {
                    if (states[j] == states[^1] && states[j - 1] == states[^2])
                    {
                        int value = states.Count - j - 1;
                        return states[(i - value + ((minutes - i) % value)) - 1];
                    }
                }
            }

            return states[minutes - 1];
        }

        private long Score() => this.Map.Count(EntityType.Trees) * this.Map.Count(EntityType.LumberyYard);
    }
}
