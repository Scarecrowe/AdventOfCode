namespace AdventOfCode.Animation.Terraria.Random
{
    using System.Drawing;
    using AdventOfCode.Animation.Terraria.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class GraveAsset : RandomAsset, IRandomAsset
    {
        public GraveAsset(string path, Frequency frequency)
            : base(new(Terraria.GetImage(path), 16, 16, new(2, 2)), frequency)
        {
        }

        public RandomAssetType RandomAssetType => RandomAssetType.Grave;

        public bool Render(
            Graphics graphics,
            VectorArray<long, EntityType> map,
            HashSet<Vector<int>> rendered,
            Vector<long> point)
        {
            if (!IsValid(map, point))
            {
                return false;
            }

            int index = RandomGenerator.Next(0, 11);

            long x = point.X * 16;
            long y = (point.Y - 1) * 16;

            graphics.DrawImage(this.Tileset[new(index * 2, 0)], x, y - 16);
            graphics.DrawImage(this.Tileset[new((index * 2) + 1, 0)], x + 16, y - 16);
            graphics.DrawImage(this.Tileset[new(index * 2, 1)], x, y);
            graphics.DrawImage(this.Tileset[new((index * 2) + 1, 1)], x + 16, y);

            rendered.Add(new Vector<int>(point.X, point.Y - 1));
            rendered.Add(new Vector<int>(point.X + 1, point.Y - 1));

            return true;
        }
    }
}
