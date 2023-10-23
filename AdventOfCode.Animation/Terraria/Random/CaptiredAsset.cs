namespace AdventOfCode.Animation.Terraria.Random
{
    using System.Drawing;
    using AdventOfCode.Animation.Extensions;
    using AdventOfCode.Animation.Terraria.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class CapturedAsset : RandomAsset, IRandomAsset
    {
        public CapturedAsset(RandomAssetType randomAssetType, string path, Frequency frequency, int tileWidth, int tileHeight)
            : base(new(Terraria.GetImage(path), tileWidth, tileHeight), frequency, 0, 0)
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
            if (map[new(point.X + 1, point.Y - 1)] == EntityType.Clay
                || map[new(point.X + 1, point.Y - 2)] == EntityType.Clay
                || map[new(point.X + 1, point.Y - 3)] == EntityType.Clay)
            {
                return false;
            }

            Vector<long> renderPoint = this.RenderPoint(point);

            Image image = (Image)this.Tileset.GetImage(this.GetRandomIndex(), 0).Clone();

            if (RandomGenerator.TrueOrFalse())
            {
                image = image.FlipHorizontally();
            }

            graphics.DrawImage(image, renderPoint.X, renderPoint.Y);

            rendered.Add(new(point.X, point.Y - 1));
            rendered.Add(new(point.X + 1, point.Y - 1));

            return true;
        }
    }
}
