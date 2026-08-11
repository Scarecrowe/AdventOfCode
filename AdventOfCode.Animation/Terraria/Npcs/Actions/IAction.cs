namespace AdventOfCode.Animation.Terraria.Npcs.Actions
{
    using AdventOfCode.Core;

    public interface IAction
    {
        Npc Npc { get; }

        Frequency? Frequency { get; }

        double Speed { get; }

        ActionTileset Tileset { get; }

        ActionType ActionType { get; }

        bool IsActive { get; }

        bool CanDeactivate { get; }

        void Activate();

        void Deactivate();

        void SetFrequency(Frequency frequency);

        void SetCanDeactivate(bool value);

        void Update(
            HashSet<Vector<long>> collisions,
            VectorArray<long, EntityType> map,
            int frame);

        CollisionType IsCollision(
            HashSet<Vector<long>> collisions,
            VectorArray<long, EntityType> map);

        IAction Clone();
    }
}
