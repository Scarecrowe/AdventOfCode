namespace AdventOfCode.Animation.Terraria.Basins
{
    using System.Drawing;

    public class BlockTilesetA : Tileset, IBlockTileset
    {
        public BlockTilesetA(Image image)
            : base(image, 16, 16, new(2, 2))
        {
        }

        public Image Bottom(int index) => this.GetImage(index, 2);

        public Image BottomLeft() => this.GetImage(3, 19);

        public Image BottomRight() => this.GetImage(2, 19);

        public Image Left(int index) => this.GetImage(0, index);

        public Image Right(int index) => this.GetImage(4, index);

        public Image Middle(int index) => this.GetImage(index, 1);

        public Image Top(int index) => this.GetImage(index, 0);

        public Image TopLeft() => this.GetImage(3, 18);

        public Image TopRight() => this.GetImage(2, 18);
    }
}
