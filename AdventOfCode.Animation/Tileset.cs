namespace AdventOfCode.Animation
{
    using System.Drawing;
    using AdventOfCode.Core;

    public class Tileset
    {
        public Tileset()
        {
            this.TileWidth = 1;
            this.TileHeight = 1;
            this.Offset = new();
            this.Tiles = new();
        }

        public Tileset(
            Image image,
            int tileWidth,
            int tileHeight)
        {
            this.TileWidth = tileWidth;
            this.TileHeight = tileHeight;
            this.Offset = new();
            this.Tiles = new();
            this.ParseTiles(image);
        }

        public Tileset(
            Image image,
            int tileWidth,
            int tileHeight,
            Vector<int> offset)
        {
            this.TileWidth = tileWidth;
            this.TileHeight = tileHeight;
            this.Offset = offset;
            this.Tiles = new();
            this.ParseTiles(image);
        }

        protected Tileset(
            int tileWidth,
            int tileHeight,
            Vector<int> offset,
            VectorDictionary<int, Image> tiles)
        {
            this.TileWidth = tileWidth;
            this.TileHeight = tileHeight;
            this.Offset = offset;
            this.Tiles = tiles;
        }

        public int TileWidth { get; protected set; }

        public int TileHeight { get; protected set; }

        public Vector<int> Offset { get; protected set; }

        public int Count => this.Tiles.Count;

        protected VectorDictionary<int, Image> Tiles { get; set; }

        public Image this[Vector<int> point]
        {
            get
            {
                return this.Tiles[point];
            }

            set
            {
                this.Tiles[point] = value;
            }
        }

        public Image GetImage(int x, int y) => this.Tiles[new(x, y)];

        public Image GetImage(int index) => this.GetImage(0, index);

        public Tileset Clone() => new(this.TileWidth, this.TileHeight, this.Offset, this.Tiles);

        private void ParseTiles(Image image)
        {
            Bitmap tile = new(this.TileWidth, this.TileHeight);

            using Graphics graphics = Graphics.FromImage(tile);

            for (int y = 0; y < image.Height / this.TileHeight; y++)
            {
                for (int x = 0; x < image.Width / this.TileWidth; x++)
                {
                    Vector<int> point = new(x, y);

                    this.RenderTile(graphics, image, point);

                    this.Tiles.Add(point, (Bitmap)tile.Clone());
                }
            }
        }

        private void RenderTile(Graphics graphics, Image image, Vector<int> point)
        {
            int x = point.X + (point.X * (this.TileWidth + this.Offset.X - 1));
            int y = point.Y + (point.Y * (this.TileHeight + this.Offset.Y - 1));

            graphics.Clear(Color.Transparent);
            graphics.DrawImage(image, new Rectangle(0, 0, this.TileWidth, this.TileHeight), new Rectangle(x, y, this.TileWidth, this.TileHeight), GraphicsUnit.Pixel);
        }
    }
}
