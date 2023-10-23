namespace AdventOfCode.Animation.Terraria.Random
{
    using System.Drawing;
    using AdventOfCode.Animation.Terraria.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class PumpkinAsset : RandomAsset, IRandomAsset
    {
        public PumpkinAsset(string path, Frequency frequency)
            : base(new(Terraria.GetImage(path), 16, 16, new(2, 2)), frequency)
        {
        }

        public RandomAssetType RandomAssetType => RandomAssetType.Pumpkin;

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

            long x = point.X * 16;
            long y = (point.Y - 1) * 16;

            int index = RandomGenerator.Next(0, 8);

            graphics.DrawImage(this.Tileset[new(2, index * 2)], x, y - 16);
            graphics.DrawImage(this.Tileset[new(3, index * 2)], x + 16, y - 16);
            graphics.DrawImage(this.Tileset[new(2, (index * 2) + 1)], x, y);
            graphics.DrawImage(this.Tileset[new(3, (index * 2) + 1)], x + 16, y);

            rendered.Add(new Vector<int>(point.X, point.Y - 1));
            rendered.Add(new Vector<int>(point.X + 1, point.Y - 1));

            return true;
        }
    }
}
