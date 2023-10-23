namespace AdventOfCode.Animation.Terraria.Random
{
    using System.Drawing;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class CharacterStatueAsset : RandomAsset
    {
        public CharacterStatueAsset(string path)
            : base(new(Terraria.GetImage(path), 16, 16, new(2, 2)), new Frequency(0.0, 1.0))
        {
        }

        public RandomAssetType RandomAssetType => RandomAssetType.CharacterStatue;

        public void Render(Graphics graphics, Vector<long> point, char chr)
        {
            int ascii = (int)chr;
            int index = 0;

            if (ascii >= 48 && ascii <= 57)
            {
                index = ascii - 48;
            }
            else if(ascii >= 65 && ascii <= 90)
            {
                index = ascii - 65;
            }

            long x = point.X * 16;
            long y = (point.Y - 1) * 16;

            graphics.DrawImage(this.Tileset[new(index * 2, 0)], x, y - 16);
            graphics.DrawImage(this.Tileset[new((index * 2) + 1, 0)], x + 16, y - 16);
            graphics.DrawImage(this.Tileset[new(index * 2, 1)], x, y);
            graphics.DrawImage(this.Tileset[new((index * 2) + 1, 1)], x + 16, y);
            graphics.DrawImage(this.Tileset[new(index * 2, 2)], x, y + 16);
            graphics.DrawImage(this.Tileset[new((index * 2) + 1, 2)], x + 16, y + 16);
        }
    }
}
