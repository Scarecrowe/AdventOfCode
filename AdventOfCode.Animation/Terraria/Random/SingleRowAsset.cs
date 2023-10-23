namespace AdventOfCode.Animation.Terraria.Random
{
    using System.Drawing;
    using AdventOfCode.Animation.Terraria.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class SingleRowAsset : RandomAsset, IRandomAsset
    {
        public SingleRowAsset(RandomAssetType randomAssetType, string path, Frequency frequency, int startIndex, int endIndex)
            : base(new(Terraria.GetImage(path), 16, 20, new(2, 2)), frequency, startIndex, endIndex)
        {
            this.RandomAssetType = randomAssetType;
        }

        public RandomAssetType RandomAssetType { get; }

        public bool Render(
            Graphics graphics,
            VectorArray<long, EntityType> map,
            HashSet<Vector<int>> rendered,
            Vector<long> point)
        {
            Vector<long> renderPoint = this.RenderPoint(point);

            graphics.DrawImage(this.Tileset.GetImage(this.GetRandomIndex(), 0), renderPoint.X, renderPoint.Y);

            rendered.Add(new(point.X, point.Y - 1));

            return true;
        }
    }
}
