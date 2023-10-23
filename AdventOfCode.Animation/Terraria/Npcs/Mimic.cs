namespace AdventOfCode.Animation.Terraria.Npcs
{
    using AdventOfCode.Animation.Terraria.Npcs.Actions;
    using AdventOfCode.Animation.Terraria.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class Mimic : Npc, INpc
    {
        public const double JumpSpeed = 1.5;
        public const double JumpHeight = 4.0;
        public const int JumpAnimateFps = 20;
        public const double JumpFrequency = 0.02;

        public const int IdleAnimateFps = 1;
        public const double IdleFrequency = 0.02;

        public Mimic(
            NpcType type,
            Frequency spawnFrequency,
            ActionTileset tileset,
            int idleIndex)
            : base(type, spawnFrequency)
        {
            this.WithJump(
                tileset.Clone().SetFps(JumpAnimateFps),
                JumpSpeed,
                JumpHeight,
                new(0.0, JumpFrequency));

            this.WithIdle(
                tileset.Clone()
                .SetFps(IdleAnimateFps)
                .SetIndexStart(idleIndex)
                .SetIndexEnd(idleIndex),
                new(0.0, IdleFrequency));

            this.Actions[ActionType.Jump].Activate();
            this.ActionType = ActionType.Jump;
        }

        private int LastFrame { get; set; }

        private int TotalFrames { get; set; }

        public new void Update(
            HashSet<Vector<long>> collisions,
            VectorArray<long, EntityType> map,
            int frame)
        {
            this.TotalFrames++;
            Vector<long> tilePoint = this.TilePoint();

            if (!Action.IsValidPoint(tilePoint, map))
            {
                return;
            }

            if (map[tilePoint] == EntityType.Water)
            {
                this.ActionType = ActionType.Idle;
                this.Actions[this.ActionType].SetCanDeactivate(false);
                this.Actions[ActionType.Idle].Deactivate();
            }

            base.Update(collisions, map, frame);

            if (this.ActionType == ActionType.Jump)
            {
                if (this.Actions[ActionType.Jump].IsActive)
                {
                    this.LastFrame = this.TotalFrames;
                }
                else
                {
                    this.Actions[ActionType.Jump].Tileset.Pause();

                    if (this.TotalFrames > this.LastFrame + WaterFallRenderer.Fps)
                    {
                        this.Actions[ActionType.Jump].Tileset.Unpause();
                        this.Actions[ActionType.Jump].Activate();
                    }
                }
            }
        }

        public INpc Clone()
        {
            Mimic result = new(
                this.Type,
                this.SpawnFrequency,
                this.Actions[ActionType.Jump].Tileset.Clone(),
                this.Actions[ActionType.Idle].Tileset.IndexStart);

            return result;
        }
    }
}
