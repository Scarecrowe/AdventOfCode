namespace AdventOfCode.Puzzles._2016.Day_01___No_Time_for_a_Taxicab
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class NoTimeForATaxicab
    {
        public NoTimeForATaxicab(string[] input)
        {
            this.Facing = TaxicabDirection.North;
            this.Directions = Parse(input);
            this.Position = new(0, 0);
            this.Stops = new() { { new(0, 0), 1 } };
        }

        public TaxicabDirection Facing { get; private set; }

        public List<(TaxicabTurn turn, int value)> Directions { get; private set; }

        public Dictionary<Vector<int>, int> Stops { get; private set; }

        public Vector<int> Position { get; }

        public long Travel(bool visited = false)
        {
            foreach ((TaxicabTurn turn, int value) in this.Directions)
            {
                this.Facing = this.GetFacing(turn);
                long? result = this.Move(visited, value);

                if (result != null)
                {
                    return result.Value;
                }
            }

            return new Vector<int>(0, 0).Distance(this.Position);
        }

        private static List<(TaxicabTurn turn, int value)> Parse(string[] input)
        {
            return input[0]
                .Split(", ")
                .Select(direction => (direction[0] == 'R' ? TaxicabTurn.Right : TaxicabTurn.Left, direction[1..].ToInt()))
                .ToList();
        }

        private int? MoveInDirection(bool visited, int value, Func<int, int> incrementor)
        {
            if (!visited)
            {
                incrementor(value);
                return null;
            }

            for (int i = 1; i <= value; i++)
            {
                incrementor(1);

                if (this.Stops.ContainsKey(this.Position))
                {
                    return new Vector<int>(0, 0).Distance(this.Position);
                }

                this.Stops.Add(this.Position, 1);
            }

            return null;
        }

        private int? Move(bool visited, int value)
        {
            return this.Facing switch
            {
                TaxicabDirection.North => this.MoveInDirection(visited, value, (value) => this.Position.Y -= value),
                TaxicabDirection.East => this.MoveInDirection(visited, value, (value) => this.Position.X += value),
                TaxicabDirection.South => this.MoveInDirection(visited, value, (value) => this.Position.Y += value),
                TaxicabDirection.West => this.MoveInDirection(visited, value, (value) => this.Position.X -= value),
                _ => throw new InvalidOperationException(),
            };
        }

        private TaxicabDirection GetFacing(TaxicabTurn turn)
        {
            int facing = (int)this.Facing + (int)turn;
            facing = facing < 0 ? 3 : facing;
            facing = facing > 3 ? 0 : facing;

            return (TaxicabDirection)facing;
        }
    }
}
