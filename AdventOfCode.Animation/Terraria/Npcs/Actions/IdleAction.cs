namespace AdventOfCode.Animation.Terraria.Npcs.Actions
{
    using System.Collections.Generic;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class IdleAction : Action, IAction
    {
        public const double DirectionChangeFrequency = 0.02;

        public IdleAction(
            Npc npc,
            ActionTileset tileset)
            : base(npc, 0.0, tileset)
        {
            this.ActionType = ActionType.Idle;
        }

        public IAction Clone()
            => new IdleAction(
                this.Npc,
                this.Tileset.Clone());

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
            if (RandomGenerator.NextDouble() < DirectionChangeFrequency)
            {
                this.Npc.SetDirection(this.Npc.Direction == Cardinal.West ? Cardinal.East : Cardinal.West);
            }
        }
    }
}
