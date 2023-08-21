namespace AdventOfCode.Puzzles._2017.Day_22___Sporifica_Virus
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class SporificaVirus
    {
        private static readonly Dictionary<Cardinal, Dictionary<EntityType, Cardinal>> Turns = new()
        {
            {
                Cardinal.North,
                new()
                {
                    { EntityType.Infected, Cardinal.East },
                    { EntityType.Flagged, Cardinal.South },
                    { EntityType.Weakened, Cardinal.West },
                    { EntityType.Clean, Cardinal.West },
                }
            },
            {
                Cardinal.South,
                new()
                {
                    { EntityType.Infected, Cardinal.West },
                    { EntityType.Flagged, Cardinal.North },
                    { EntityType.Weakened, Cardinal.East },
                    { EntityType.Clean, Cardinal.East },
                }
            },
            {
                Cardinal.East,
                new()
                {
                    { EntityType.Infected, Cardinal.South },
                    { EntityType.Flagged, Cardinal.West },
                    { EntityType.Weakened, Cardinal.North },
                    { EntityType.Clean, Cardinal.North },
                }
            },
            {
                Cardinal.West,
                new()
                {
                    { EntityType.Infected, Cardinal.North },
                    { EntityType.Flagged, Cardinal.East },
                    { EntityType.Weakened, Cardinal.South },
                    { EntityType.Clean, Cardinal.South },
                }
            }
        };

        public SporificaVirus(string[] input)
        {
            this.Map = new(input, (c) => c == '#' ? EntityType.Infected : EntityType.Clean);
            this.Direction = Cardinal.North;
            this.Position = new(this.Map.Width / 2, this.Map.Height / 2);
        }

        public int InfectedCount { get; private set; }

        private VectorDictionary<int, EntityType> Map { get; }

        private Cardinal Direction { get; set; }

        private Vector<int> Position { get; set; }

        public SporificaVirus Run(int bursts, bool mutated = false)
        {
            for (int i = 1; i <= bursts; i++)
            {
                if (mutated)
                {
                    this.BurstMutated();
                }
                else
                {
                    this.Burst();
                }
            }

            return this;
        }

        private void Move()
        {
            this.Position += CardinalHelper.CardinalTransform<int>()[this.Direction];

            this.Map.TryAddValue(this.Position, EntityType.Clean);
        }

        private void TurnMutated()
        {
            if (this.Map[this.Position] == EntityType.Weakened)
            {
                return;
            }

            this.Direction = Turns[this.Direction][this.Map[this.Position]];
        }

        private void Burst()
        {
            this.Direction = Turns[this.Direction][this.Map[this.Position]];

            this.Map[this.Position] = this.Map[this.Position] == EntityType.Infected ? EntityType.Clean : EntityType.Infected;

            if (this.Map[this.Position] == EntityType.Infected)
            {
                this.InfectedCount++;
            }

            this.Move();
        }

        private void BurstMutated()
        {
            this.TurnMutated();

            switch (this.Map[this.Position])
            {
                case EntityType.Clean:
                    this.Map[this.Position] = EntityType.Weakened;
                    break;
                case EntityType.Flagged:
                    this.Map[this.Position] = EntityType.Clean;
                    break;
                case EntityType.Infected:
                    this.Map[this.Position] = EntityType.Flagged;
                    break;
                case EntityType.Weakened:
                    this.Map[this.Position] = EntityType.Infected;
                    break;
            }

            if (this.Map[this.Position] == EntityType.Infected)
            {
                this.InfectedCount++;
            }

            this.Move();
        }
    }
}
