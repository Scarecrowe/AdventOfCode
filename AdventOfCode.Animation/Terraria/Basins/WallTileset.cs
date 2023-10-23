namespace AdventOfCode.Animation.Terraria.Basins
{
    using System.Drawing;

    public class WallTileset
    {
        public WallTileset(Tileset tileset)
        {
            this.Tileset = tileset;
        }

        public Tileset Tileset { get; }

        public Image Top(int index) => this.Tileset.GetImage(index, 0);

        public Image Middle(int index) => this.Tileset.GetImage(index, 1);
    }
}
