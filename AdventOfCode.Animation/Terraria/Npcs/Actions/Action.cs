namespace AdventOfCode.Animation.Terraria.Npcs.Actions
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public abstract class Action
    {
        public Action(
            Npc npc,
            double speed)
        {
            this.Npc = npc;
            this.Speed = speed;
            this.IsActive = false;
            this.Tileset = new();
            this.CanDeactivate = true;
        }

        public Action(
            Npc npc,
            double speed,
            ActionTileset tileset)
        {
            this.Npc = npc;
            this.Speed = speed;
            this.Tileset = tileset;
            this.IsActive = false;
            this.CanDeactivate = true;
        }

        public Npc Npc { get; }

        public Frequency? Frequency { get; protected set; }

        public double Speed { get; }

        public ActionTileset Tileset { get; protected set; }

        public ActionType ActionType { get; protected set; }

        public bool IsActive { get; protected set; }

        public bool CanDeactivate { get; protected set; }

        public static bool IsValidPoint(
            Vector<long> point,
            VectorArray<long, EntityType> map)
        {
            if (point.Y >= map.Height
                || point.X >= map.Width
                || point.X < 0
                || point.Y < 0)
            {
                return false;
            }

            return true;
        }

        public void Activate() => this.IsActive = true;

        public void Deactivate() => this.IsActive = false;

        public void SetFrequency(Frequency frequency)
        {
            this.Frequency = frequency;
        }

        public void SetCanDeactivate(bool value)
        {
            this.CanDeactivate = value;
        }
    }
}
