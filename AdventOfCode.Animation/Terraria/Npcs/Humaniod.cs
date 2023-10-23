namespace AdventOfCode.Animation.Terraria.Npcs
{
    using AdventOfCode.Animation.Terraria.Npcs.Actions;

    public class Humaniod : Npc, INpc
    {
        public const double WalkSpeed = 1.0;
        public const int WalkAnimateFps = 12;
        public const double WalkFrequency = 0.02;

        public const int IdleAnimateFps = 1;
        public const double IdleFrequency = 0.02;

        public Humaniod(
            NpcType type,
            Frequency spawnFrequency,
            ActionTileset tileset)
            : base(type, spawnFrequency)
        {
            this.WithWalk(
                tileset.SetFps(WalkAnimateFps),
                WalkSpeed,
                new(0.0, WalkFrequency));

            this.WithIdle(
                tileset.Clone()
                .SetFps(IdleAnimateFps)
                .SetIndexStart(2)
                .SetIndexEnd(2),
                new(0.0, IdleFrequency));

            this.Actions[ActionType.Walk].Activate();
            this.ActionType = ActionType.Walk;
        }

        public INpc Clone()
        {
            Humaniod result = new(
                this.Type,
                this.SpawnFrequency,
                this.Actions[ActionType.Walk].Tileset.Clone());

            return result;
        }
    }
}
