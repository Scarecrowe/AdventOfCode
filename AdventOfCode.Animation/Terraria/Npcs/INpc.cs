namespace AdventOfCode.Animation.Terraria.Npcs
{
    using System.Drawing;
    using AdventOfCode.Animation.Terraria.Npcs.Actions;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public interface INpc
    {
        Frequency SpawnFrequency { get; }

        int TileWidth { get; }

        int TileHeight { get; }

        Vector<double> Point { get; }

        Cardinal Direction { get; }

        ActionType ActionType { get; }

        NpcType Type { get; }

        void SetPoint(Vector<double> point);

        void Update(
            HashSet<Vector<long>> collisions,
            VectorArray<long, EntityType> map,
            int frame);

        CollisionType IsCollision(
            HashSet<Vector<long>> collisions,
            VectorArray<long, EntityType> map);

        Vector<long> TilePoint();

        List<Vector<long>> TilePoints();

        List<IAction> GetInActiveActions(double frequency);

        INpc Clone();

        void Render(
            Graphics graphics,
            Vector<long> point,
            int frame);

        void SetDirection(Cardinal direciton);

        void SetRandomDirection(
            HashSet<Vector<long>> collisions,
            VectorArray<long, EntityType> map,
            Func<HashSet<Vector<long>>, VectorArray<long, EntityType>, CollisionType> isCollision);
    }
}
