namespace AdventOfCode.Animation.Terraria.Liquids
{
    using System.Drawing;

    public class Liquid
    {
        public Liquid(
            LiquidType liquidType,
            Tileset flowTileset,
            Tileset topTileset,
            float opacity)
        {
            this.LiquidType = liquidType;
            this.FlowTileset = flowTileset;
            this.TopTileset = topTileset;
            this.Opacity = opacity;
        }

        public LiquidType LiquidType { get; }

        public Tileset FlowTileset { get; }

        public Tileset TopTileset { get; }

        public float Opacity { get; }

        public Image TopRight => this.FlowTileset.GetImage(2, 0);

        public Image TopLeft => this.FlowTileset.GetImage(0, 0);

        public Image Settled => this.FlowTileset.GetImage(1, 4);

        public Image Top(int index)
        {
            if (this.LiquidType == LiquidType.Lava)
            {
                return this.TopTileset.GetImage(0, 0);
            }
            else
            {
                return this.TopTileset.GetImage(index, 0);
            }
        }

        public Image Flow(int index) => this.FlowTileset.GetImage(1, index);
    }
}
