namespace AdventOfCode.Animation.Terraria.Basins
{
    using System.Drawing;

    public interface IBlockTileset
    {
        public Image TopLeft();

        public Image TopRight();

        public Image BottomLeft();

        public Image BottomRight();

        public Image Left(int index);

        public Image Right(int index);

        public Image Bottom(int index);

        public Image Middle(int index);

        public Image Top(int index);
    }
}
