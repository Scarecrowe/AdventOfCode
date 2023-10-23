namespace AdventOfCode.Animation.Terraria.Npcs.Actions
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class WalkAction : Action, IAction
    {
        public WalkAction(
            Npc npc,
            ActionTileset tileset,
            double speed)
            : base(npc, speed, tileset)
        {
            this.Npc.SetRandomDirection(new() { Cardinal.West, Cardinal.East });
            this.ActionType = ActionType.Walk;
        }

        public void Update(
            HashSet<Vector<long>> collisions,
            VectorArray<long, EntityType> map,
            int frame)
        {
            Vector<double> point = this.Npc.Point.Clone();

            if (this.Npc.Direction == Cardinal.None)
            {
                return;
            }

            this.Npc.Point.Transform(Vector<double>.GetPointByCardinal(this.Npc.Direction) * this.Speed);

            if (this.Npc.IsCollision(collisions, map) != CollisionType.None)
            {
                this.Npc.Point.X = point.X;
                this.Npc.Point.Y = point.Y;
                this.Npc.SetDirection(this.Npc.Direction == Cardinal.West ? Cardinal.East : Cardinal.West);
            }
        }

        public IAction Clone()
        {
            WalkAction result = new(
                this.Npc,
                this.Tileset.Clone(),
                this.Speed);

            return result;
        }

        public CollisionType IsCollision(HashSet<Vector<long>> collisions, VectorArray<long, EntityType> map)
        {
            return CollisionType.None;
        }
    }
}
