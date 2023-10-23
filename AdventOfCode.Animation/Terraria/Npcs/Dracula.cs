namespace AdventOfCode.Animation.Terraria.Npcs
{
    using AdventOfCode.Animation.Terraria.Npcs.Actions;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class Dracula : Npc, INpc
    {
        public const double WalkSpeed = 1.0;
        public const int WalkAnimateFps = 12;
        public const double WalkFrequency = 0.02;

        public const double FlySpeed = 1.0;
        public const int FlyAnimateFps = 15;
        public const double FlyFrequency = 0.001;

        public const int IdleAnimateFps = 1;
        public const double IdleFrequency = 0.02;

        public Dracula(
            NpcType type,
            Frequency spawnFrequency,
            ActionTileset walkTileset,
            ActionTileset flyTileset)
            : base(type, spawnFrequency)
        {
            this.WithWalk(
                walkTileset.SetFps(WalkAnimateFps),
                WalkSpeed,
                new(0.0, WalkFrequency));

            this.WithFly(
                flyTileset.SetFps(FlyAnimateFps),
                FlySpeed,
                new(0.0, FlyFrequency));

            this.WithIdle(
             walkTileset.Clone()
             .SetFps(IdleAnimateFps)
             .SetIndexStart(2)
             .SetIndexEnd(2),
             new(0.0, IdleFrequency));

            this.SetRandomDirection(new() { Cardinal.West, Cardinal.East });
            this.Actions[ActionType.Walk].Activate();
            this.ActionType = ActionType.Walk;
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

            if (this.ActionType == ActionType.Fly
                && this.Actions.ContainsKey(ActionType.Walk))
            {
                this.Actions.Remove(ActionType.Walk);
                this.Actions.Remove(ActionType.Idle);
            }

            if (this.ActionType != ActionType.Fly)
            {
                List<Vector<long>> tilePoints = this.TilePoints();

                if (IsInLiquid(tilePoints, map)
                    && CanFly(tilePoints, map, collisions, 10))
                {
                    this.Actions.Remove(ActionType.Walk);
                    this.Actions.Remove(ActionType.Idle);
                    this.ActionType = ActionType.Fly;
                    this.Actions[ActionType.Fly].Activate();
                }
            }

            base.Update(collisions, map, frame);
        }

        public INpc Clone()
        {
            Dracula result = new(
                this.Type,
                this.SpawnFrequency,
                this.Actions[ActionType.Walk].Tileset.Clone(),
                this.Actions[ActionType.Fly].Tileset.Clone());

            return result;
        }

        private static bool IsInLiquid(
            List<Vector<long>> tilePoints,
            VectorArray<long, EntityType> map)
        {
            foreach(Vector<long> point in tilePoints)
            {
                EntityType value = map[point];

                if (value == EntityType.Water
                    || value == EntityType.Settled)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool CanFly(
            List<Vector<long>> tilePoints,
            VectorArray<long, EntityType> map,
            HashSet<Vector<long>> collisions,
            int height)
        {
            foreach(Vector<long> point in tilePoints)
            {
                for(int i = 1; i <= height; i++)
                {
                    if(collisions.Contains(point + (Vector<long>.North * i)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
