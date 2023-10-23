namespace AdventOfCode.Animation.Terraria.Npcs
{
    using AdventOfCode.Animation.Terraria.Npcs.Actions;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class Bird : Npc, INpc
    {
        public const double FlySpeed = 2.0;
        public const int FlyAnimateFps = 15;
        public const double FlyFrequency = 0.001;

        public const int IdleAnimateFps = 1;

        public Bird(
            NpcType type,
            Frequency spawnFrequency,
            ActionTileset moveTileset)
            : base(type, spawnFrequency)
        {
            this.WithFly(moveTileset.SetFps(FlyAnimateFps), FlySpeed, new(0.0, FlyFrequency));
            this.WithIdle(moveTileset.Clone().SetFps(IdleAnimateFps).SetIndexStart(4).SetIndexEnd(4));
            this.ActionType = ActionType.Idle;
            this.Actions[this.ActionType].Activate();
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

            if (this.ActionType != ActionType.Fly)
            {
                if (map[tilePoint] == EntityType.Water
                    || map[tilePoint + Vector<long>.South] == EntityType.Water)
                {
                    this.Actions.Remove(ActionType.Idle);
                    this.Actions[ActionType.Fly].Activate();
                    this.ActionType = ActionType.Fly;
                }
            }

            base.Update(collisions, map, frame);
        }

        public INpc Clone()
        {
            Bird result = new(
                this.Type,
                this.SpawnFrequency,
                this.Actions[ActionType.Fly].Tileset.Clone());

            return result;
        }
    }
}
