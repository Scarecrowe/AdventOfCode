namespace AdventOfCode.Animation.Terraria
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class Screen
    {
        public Screen(
            int width,
            int height,
            Vector<long> max,
            int tileWidth,
            int tileHeight,
            long clayMinX)
        {
            this.Width = width;
            this.Height = height;
            this.TileWidth = tileWidth;
            this.TileHeight = tileHeight;
            this.Point = new(0, 0);
            this.ScrollMax = new(0, 0);
            this.LiquidMax = new(0, 0);
            this.Max = max;
            this.ReservoirX = 500;

            if ((this.ReservoirX - clayMinX) > (this.Width / 16))
            {
                this.Point.X = ((this.ReservoirX - clayMinX) * 16) - (this.Width / 2);
                this.LiquidMax.X = this.ReservoirX - clayMinX;
            }
        }

        public int ReservoirX { get; }

        public int Width { get; }

        public int Height { get; }

        public int TileWidth { get; }

        public int TileHeight { get; }

        public Vector<long> Max { get; }

        public Vector<long> Point { get; private set; }

        public Vector<long> ScrollMax { get; private set; }

        public Vector<long> LiquidMax { get; private set; }

        public void Scroll()
        {
            int distance = this.LiquidMax.Y > ((this.Point.Y + 900) / 16) ? 5 : 1;

            if ((this.Point.Y / 16) < (this.LiquidMax.Y - 15))
            {
                this.Point.Y += distance;
            }

            long center = (this.Point.X + (this.Width / 2)) / 16;

            if (this.LiquidMax.X > center)
            {
                this.Point.X += distance;
            }
            else if (this.LiquidMax.X < center)
            {
                this.Point.X -= distance;
            }

            if (this.Point.X < 0)
            {
                this.Point.X = 0;
            }

            if (this.Point.X > (this.Max.X * 16) - (this.Width + 48))
            {
                this.Point.X = (this.Max.X * 16) - (this.Width + 48);
            }
        }

        public Edges Edges()
        {
            long minX = this.Point.X / this.TileWidth;
            long minY = this.Point.Y / this.TileHeight;
            long maxX = (this.Point.X + this.Width) / this.TileWidth;
            long maxY = (this.Point.Y + this.Height) / this.TileHeight;

            if (maxX >= this.Max.X)
            {
                maxX = this.Max.X - 1;
            }

            if (maxY >= this.Max.Y)
            {
                maxY = this.Max.Y - 1;
            }

            return new Edges(new(minX, minY), new(maxX, minY), new(minX, maxY), new(maxX, maxY));
        }

        public Vector<long> RenderPoint(long x, long y)
        {
            long renderX = ((x - (this.Point.X / 16)) * 16) + (((this.Point.X / 16) * 16) - this.Point.X);
            long renderY = ((y - (this.Point.Y / 16)) * 16) + (((this.Point.Y / 16) * 16) - this.Point.Y);

            return new(renderX, renderY);
        }

        public void SetLiquidMax(Vector<long> stream)
        {
            if (stream.Y > this.LiquidMax.Y)
            {
                this.LiquidMax.Y = stream.Y;
                this.LiquidMax.X = stream.X;
            }

            if (stream.X > this.LiquidMax.X)
            {
                this.LiquidMax.X = stream.X;
            }
        }

        public int CountOfWater(VectorArray<long, EntityType> map)
        {
            int result = 0;
            Edges edges = this.Edges();

            for (long y = edges.TopLeft.Y; y <= edges.BottomLeft.Y; y++)
            {
                for (long x = edges.TopLeft.X; x <= edges.TopRight.X; x++)
                {
                    EntityType entityType = map[y, x];

                    if (entityType == EntityType.Water
                        || entityType == EntityType.Settled)
                    {
                        result++;
                    }
                }
            }

            return result;
        }
    }
}
