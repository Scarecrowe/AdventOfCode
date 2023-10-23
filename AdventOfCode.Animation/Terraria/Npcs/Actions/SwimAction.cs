namespace AdventOfCode.Animation.Terraria.Npcs.Actions
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class SwimAction : Action, IAction
    {
        public SwimAction(
            Npc npc,
            ActionTileset tileset,
            double speed)
            : base(npc, speed, tileset)
        {
            this.Npc.SetRandomDirection(new() { Cardinal.North, Cardinal.NorthWest, Cardinal.NorthEast });
            this.ActionType = ActionType.Swim;
        }

        public void Update(
            HashSet<Vector<long>> collisions,
            VectorArray<long, EntityType> map,
            int frame)
        {
            if (RandomGenerator.NextDouble() < 0.01)
            {
                this.Npc.SetRandomDirection(collisions, map, this.IsCollision);
            }

            if (this.Npc.Direction == Cardinal.None)
            {
                return;
            }

            Vector<double> point = this.Npc.Point.Clone();

            this.Npc.Point.Transform(this.Npc.Direction);

            Vector<long> tilePoint = this.Npc.TilePoint();

            if (tilePoint.Y < 0
                || tilePoint.X < 0)
            {
                return;
            }

            if (this.Npc.IsCollision(collisions, map) != CollisionType.None)
            {
                this.Npc.Point.X = point.X;
                this.Npc.Point.Y = point.Y;
                this.Npc.SetRandomDirection(collisions, map, this.IsCollision);
            }
        }

        public CollisionType IsCollision(
           HashSet<Vector<long>> collisions,
           VectorArray<long, EntityType> map)
        {
            Vector<long> point = this.Npc.TilePoint();
            if (!IsValidPoint(point, map))
            {
                return CollisionType.Invalid;
            }

            if (this.Npc.ActionType == ActionType.Swim)
            {
                if (map[point] == EntityType.Settled
                    || (map[point] == EntityType.Water
                    && map[point + Vector<long>.West] == EntityType.Water
                    && map[point + Vector<long>.East] == EntityType.Water))
                {
                    return CollisionType.None;
                }

                return CollisionType.Invalid;
            }

            return CollisionType.None;
        }

        public IAction Clone()
        {
            SwimAction result = new(
                this.Npc,
                this.Tileset.Clone(),
                this.Speed);

            result.Npc.SetDirection(this.Npc.Direction);

            return result;
        }
    }
}
