namespace AdventOfCode.Animation.Terraria.Npcs.Actions
{
    using System.Collections.Generic;
    using AdventOfCode.Animation.Terraria.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class JumpAction : Action, IAction
    {
        public JumpAction(
            Npc npc,
            ActionTileset tileset,
            double speed,
            double height)
            : base(npc, speed, tileset)
        {
            this.Height = height;
            this.ActionType = ActionType.Jump;
            this.Transform = new();
            this.Tileset.SetLoop(false);
            this.Npc.SetRandomDirection(new() { Cardinal.East, Cardinal.West });
        }

        public double Height { get; }

        public bool IsJumping { get; private set; }

        private Vector<double> Transform { get; set; }

        public IAction Clone()
            => new JumpAction(this.Npc, this.Tileset.Clone(), this.Speed, this.Height);

        public CollisionType IsCollision(
            HashSet<Vector<long>> collisions,
            VectorArray<long, EntityType> map)
        {
            return CollisionType.None;
        }

        public void Update(
            HashSet<Vector<long>> collisions,
            VectorArray<long, EntityType> map,
            int frame)
        {
            if (!this.IsJumping)
            {
                this.Transform = Vector<double>.GetPointByCardinal(this.Npc.Direction == Cardinal.East ? Cardinal.NorthWest : Cardinal.NorthEast) * this.Speed;
                this.Transform.Y = this.Height;
                this.Tileset.Reset();
                this.IsJumping = true;
                this.CanDeactivate = false;
            }

            this.Transform.Y -= WaterFallRenderer.Gravity * WaterFallRenderer.TimeDelta;

            Vector<double> point = this.Npc.Point.Clone();

            this.Npc.SetPoint(this.Npc.Point - this.Transform);

            CollisionType collisionType = this.Npc.IsCollision(collisions, map);

            if (collisionType == CollisionType.None)
            {
                return;
            }

            this.Npc.SetPoint(point);

            if (collisionType == CollisionType.Bottom)
            {
                this.Npc.Point.Y = Math.Ceiling(this.Npc.Point.Y);
                this.IsJumping = false;
                this.CanDeactivate = true;
                this.Deactivate();
                this.Tileset.Reset();
                return;
            }

            if (collisionType == CollisionType.Left
                || collisionType == CollisionType.Right)
            {
                this.Npc.SetDirection(this.Npc.Direction == Cardinal.West ? Cardinal.East : Cardinal.West);
                this.Transform.X *= -1;
            }
        }
    }
}
