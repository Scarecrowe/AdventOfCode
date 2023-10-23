namespace AdventOfCode.Animation.Terraria.Npcs
{
    using AdventOfCode.Animation.Terraria.Npcs.Actions;

    public class Bunny : Npc, INpc
    {
        public const double WalkSpeed = 3.0;
        public const int WalkAnimateFps = 15;
        public const double WalkFrequency = 0.02;

        public const int IdleAnimateFps = 15;
        public const double IdleFrequency = 0.02;

        public Bunny(
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
                .SetIndexStart(0)
                .SetIndexEnd(0),
                new(0.0, IdleFrequency));

            this.Actions[ActionType.Walk].Activate();
            this.ActionType = ActionType.Walk;
        }

        public INpc Clone()
        {
            Bunny result = new(
                this.Type,
                this.SpawnFrequency,
                this.Actions[ActionType.Walk].Tileset.Clone());

            return result;
        }
    }
}
