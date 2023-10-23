namespace AdventOfCode.Animation.Terraria.Npcs.Actions
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class FlyAction : Action, IAction
    {
        public const double RandomDirectionChangeFrequency = 0.02;

        public FlyAction(
            Npc npc,
            ActionTileset tileset,
            double speed)
            : base(npc, speed, tileset)
        {
            this.Npc.SetRandomDirection(new() { Cardinal.North, Cardinal.NorthWest, Cardinal.NorthEast });
            this.ActionType = ActionType.Fly;
        }

        public void Update(
            HashSet<Vector<long>> collisions,
            VectorArray<long, EntityType> map,
            int frame)
        {
            Vector<long> topLeft = this.Npc.TilePoint();
            Vector<long> bottomLeft = new(topLeft.X, ((int)this.Npc.Point.Y + this.Tileset.TileHeight) / 16);

            if (map[bottomLeft] == EntityType.Settled
                || (map[bottomLeft] == EntityType.Water && map[topLeft] == EntityType.Air))
            {
                this.Npc.Point.Y -= this.Speed * 2;

                return;
            }

            if (RandomGenerator.NextDouble() < RandomDirectionChangeFrequency)
            {
                this.Npc.SetRandomDirection(collisions, map, this.Npc.IsCollision);
            }

            if (this.Npc.Direction == Cardinal.None)
            {
                return;
            }

            Vector<double> point = this.Npc.Point.Clone();

            this.Npc.Point.Transform(Vector<double>.GetPointByCardinal(this.Npc.Direction) * this.Speed);

            if (this.Npc.IsCollision(collisions, map) != CollisionType.None)
            {
                this.Npc.Point.X = point.X;
                this.Npc.Point.Y = point.Y;

                Vector<long> tilePoint = this.Npc.TilePoint();

                if (map[tilePoint.Y - 1, tilePoint.X] == EntityType.Water)
                {
                    List<Cardinal> directions = new()
                    {
                        Cardinal.North,
                        Cardinal.NorthWest,
                        Cardinal.NorthEast
                    };

                    this.Npc.SetDirection(directions[RandomGenerator.Next(0, 2)]);
                }
                else
                {
                    this.Npc.SetRandomDirection(collisions, map, this.Npc.IsCollision);
                }
            }
        }

        public CollisionType IsCollision(
            HashSet<Vector<long>> collisions,
            VectorArray<long, EntityType> map)
        {
            Vector<long> tilePoint = this.Npc.TilePoint();

            if (!IsValidPoint(tilePoint, map))
            {
                return CollisionType.Invalid;
            }

            return map[tilePoint] == EntityType.Settled
                ? CollisionType.Invalid : CollisionType.None;
        }

        public IAction Clone()
        {
            FlyAction result = new(
                this.Npc,
                this.Tileset.Clone(),
                this.Speed);

            result.Npc.SetDirection(this.Npc.Direction);

            return result;
        }
    }
}
