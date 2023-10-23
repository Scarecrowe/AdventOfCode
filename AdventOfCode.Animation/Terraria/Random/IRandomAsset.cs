namespace AdventOfCode.Animation.Terraria.Random
{
    using System.Drawing;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public interface IRandomAsset
    {
        RandomAssetType RandomAssetType { get; }

        Tileset Tileset { get; }

        Frequency Frequency { get; }

        int? StartIndex { get; }

        int? EndIndex { get; }

        bool Render(
            Graphics graphics,
            VectorArray<long, EntityType> map,
            HashSet<Vector<int>> rendered,
            Vector<long> point);
    }
}
