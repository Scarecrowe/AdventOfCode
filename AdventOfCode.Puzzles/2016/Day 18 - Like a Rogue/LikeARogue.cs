namespace AdventOfCode.Puzzles._2016.Day_18___Like_a_Rogue
{
    using AdventOfCode.Core;

    public class LikeARogue
    {
        public LikeARogue(string input)
        {
            this.Input = input;
            this.Map = new VectorArray<int, EntityType>(input, ".^");
        }

        public string Input { get; }

        public VectorArray<int, EntityType> Map { get; private set; }

        public long SafeTileCount { get; private set; }

        public LikeARogue BuildMap(int rows)
        {
            for (int y = 0; y < rows - 1; y++)
            {
                VectorArray<int, EntityType> state = new(this.Map.Width, 1);

                for (int x = 0; x < this.Map.Width; x++)
                {
                    if (this.Map[0, x] == EntityType.Safe)
                    {
                        this.SafeTileCount++;
                    }

                    bool left = x != 0 && (this.Map[0, x - 1] == EntityType.Safe ? false : true);
                    bool center = this.Map[0, x] == EntityType.Safe ? false : true;
                    bool right = x < this.Map.Width - 1 && (this.Map[0, x + 1] == EntityType.Safe ? false : true);
                    state[0, x] = IsTrap(left, center, right) ? EntityType.Trap : EntityType.Safe;
                }

                this.Map = state;
            }

            for (int x = 0; x < this.Map.Width; x++)
            {
                if (this.Map[0, x] == EntityType.Safe)
                {
                    this.SafeTileCount++;
                }
            }

            return this;
        }

        private static bool IsTrap(bool left, bool center, bool right) =>
            (left && center && !right)
            || (right && center && !left)
            || (left && !center && !right)
            || (right && !center & !left);
    }
}
