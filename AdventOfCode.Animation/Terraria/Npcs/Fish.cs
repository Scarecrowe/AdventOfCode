namespace AdventOfCode.Animation.Terraria.Npcs
{
    using AdventOfCode.Animation.Terraria.Npcs.Actions;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class Fish : Npc, INpc
    {
        public const double WalkSpeed = 1.0;
        public const int WalkAnimateFps = 15;
        public const double WalkFrequency = 0.02;

        public const int IdleAnimateFps = 1;
        public const double IdleFrequency = 0.02;

        public const double SwimSpeed = 2.0;
        public const int SwimAnimateFps = 15;

        public Fish(
            NpcType type,
            Frequency spawnFrequency,
            ActionTileset walkTileset,
            ActionTileset swimTileset)
            : base(type, spawnFrequency)
        {
            this.WithWalk(
                walkTileset.SetFps(WalkAnimateFps),
                WalkSpeed,
                new(0.0, WalkFrequency));

            this.WithIdle(
                walkTileset.Clone()
                .SetFps(IdleAnimateFps)
                .SetIndexStart(0)
                .SetIndexEnd(0),
                new(0.0, IdleFrequency));

            this.WithSwim(
                swimTileset.SetFps(SwimAnimateFps),
                SwimSpeed);

            this.SetRandomDirection(new() { Cardinal.West, Cardinal.East });
            this.ActionType = ActionType.Walk;
            this.Actions[ActionType.Walk].Activate();
        }

        public new void Update(
            HashSet<Vector<long>> collisions,
            VectorArray<long, EntityType> map,
            int frame)
        {
            Vector<long> tilePoint = this.TilePoint();

            if (!Action.IsValidPoint(tilePoint, map))
            {
                return;
            }

            if (this.ActionType == ActionType.Walk)
            {
                if (map[tilePoint] == EntityType.Settled
                    && map[tilePoint + (Vector<long>.North * 2)] == EntityType.Settled)
                {
                    this.Actions.Remove(ActionType.Walk);
                    this.Actions.Remove(ActionType.Idle);

                    this.ActionType = ActionType.Swim;
                    this.Actions[ActionType.Swim].Activate();
                    this.SetRandomDirection(collisions, map, this.IsCollision);
                    return;
                }
            }

            base.Update(collisions, map, frame);
        }

        public INpc Clone()
        {
            Fish result = new(
                this.Type,
                this.SpawnFrequency,
                this.Actions[ActionType.Walk].Tileset.Clone(),
                this.Actions[ActionType.Swim].Tileset.Clone());

            return result;
        }
    }
}
