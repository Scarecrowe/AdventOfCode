namespace AdventOfCode.Animation.Terraria.Renderers
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using AdventOfCode.Animation.Extensions;
    using AdventOfCode.Animation.FFmpeg;
    using AdventOfCode.Animation.Terraria.Clouds;
    using AdventOfCode.Animation.Terraria.Liquids;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class IntroRenderer
    {
        public const int CloudCount = 100;

        public IntroRenderer(Screen screen)
        {
            this.Screen = screen;
            this.Liquid = new(
                LiquidType.Water,
                new(Terraria.GetImage("Liquids\\Water_0.png"), 16, 16),
                new(Terraria.GetImage("Liquids\\Liquid_0.png"), 16, 16, new(2, 0)),
                LiquidRenderer.Opacity);
            this.Sun = Terraria.GetImage("Random\\Sun.png");
            this.Cloud = Terraria.GetImage("backgrounds\\Cloud.png");
            this.Clouds = LoadClouds();
            this.FlowIndex = 2;
            this.FadeRenderer = new(this.Screen.Width, this.Screen.Height, FadeType.In);
        }

        public int Frames { get; private set; }

        private Screen Screen { get; }

        private Liquid Liquid { get; }

        private Image Sun { get; }

        private Image Cloud { get; }

        private List<Cloud> Clouds { get; }

        private int FlowTopIndex { get; set; }

        private int FlowIndex { get; set; }

        private int FlowSkip { get; set; }

        private int FlowLength { get; set; }

        private FadeRenderer FadeRenderer { get; }

        public void Render(
            Graphics graphics,
            Bitmap frame,
            Bitmap scene,
            FFmpegBuilder ffmpeg,
            ReservoirResearch puzzle)
        {
            Vector<long> stream = new(WaterFallRenderer.ReservoirSourceX - puzzle.ClayMin.X, (WaterFallRenderer.IntroLength + 260) / 16);
            int center = ((int)(stream.X * 16) - (int)this.Screen.Point.X) - (this.Cloud.Width / 2);

            while (true)
            {
                int waterTop = 224;
                this.Frames++;

                this.RenderScene(graphics, scene);
                this.RenderSun(graphics);
                this.RenderClouds(graphics);
                this.RenderStaticCloudLiquid(graphics, center, waterTop);

                waterTop = (waterTop - 4) + 80;

                this.RenderStaticCloud(graphics, center);
                this.RenderLiquid(graphics, stream, center, waterTop);
                this.FadeRenderer.Render(graphics);

                ffmpeg.WriteBitmap(frame, ImageFormat.Jpeg);

                if (waterTop + (this.FlowLength * 16) >= WaterFallRenderer.IntroLength + 64)
                {
                    break;
                }

                if (this.Frames % 4 == 0)
                {
                    this.FlowLength++;
                }

                if (this.FlowLength > 40)
                {
                    this.Screen.SetLiquidMax(new(stream.X, stream.Y));
                    this.Screen.Scroll();
                }
            }
        }

        private static List<Cloud> LoadClouds()
        {
            List<Cloud> result = new();

            for (int i = 0; i < CloudCount; i++)
            {
                result.Add(new($"Clouds\\Cloud_{RandomGenerator.Next(1, 36)}.png", new(1, -1)));
            }

            return result;
        }

        private void RenderSun(Graphics graphics)
        {
            graphics.DrawImage(this.Sun, this.Screen.Width - 200, 200 - this.Screen.Point.Y);
        }

        private void RenderStaticCloud(Graphics graphics, int center)
        {
            graphics.DrawImage(this.Cloud, new Point(center - 48, 64 - (int)this.Screen.Point.Y));
        }

        private void RenderClouds(Graphics graphics)
        {
            foreach (Cloud cloud in this.Clouds)
            {
                cloud.Move();
                graphics.DrawImage(cloud.Image, cloud.Point.X, cloud.Point.Y - this.Screen.Point.Y);
            }
        }

        private void RenderLiquid(Graphics graphics, Vector<long> stream, int center, int waterTop)
        {
            for (int y = 0; y < this.FlowLength; y++)
            {
                graphics.DrawImageWithOpacity(
                    LiquidRenderer.Opacity,
                    this.Liquid.Flow(this.FlowIndex),
                    new(center + (14 * 16), (waterTop + (y * 16)) - (int)this.Screen.Point.Y, 16, 16));

                if (this.FlowSkip >= 50)
                {
                    this.FlowIndex += 5;

                    if (this.FlowIndex > 77)
                    {
                        this.FlowIndex = 2;
                    }

                    this.FlowSkip = 0;
                }

                this.FlowSkip++;
            }
        }

        private void RenderStaticCloudLiquid(Graphics graphics, int center, int waterTop)
        {
            for (int y = 0; y < 6; y++)
            {
                for (int x = center; x <= center + 336; x += 16)
                {
                    if (y == 0)
                    {
                        graphics.DrawImageWithOpacity((float)1.0f, this.Liquid.Top(this.FlowTopIndex), new Rectangle(x - 2, (waterTop - 4) - (int)this.Screen.Point.Y, 16, 16));
                    }
                    else
                    {
                        graphics.DrawImageWithOpacity((float)1.0f, this.Liquid.Settled, new Rectangle(x - 2, ((waterTop - 4) + (y * 16)) - (int)this.Screen.Point.Y, 16, 16));
                    }

                    this.FlowTopIndex++;

                    if (this.FlowTopIndex > 8)
                    {
                        this.FlowTopIndex = 0;
                    }
                }
            }
        }

        private void RenderScene(Graphics graphics, Bitmap scene)
            => graphics.DrawImage(
                scene,
                new Rectangle(0, 0, this.Screen.Width, this.Screen.Height),
                new Rectangle((int)this.Screen.Point.X, (int)this.Screen.Point.Y, this.Screen.Width, this.Screen.Height),
                GraphicsUnit.Pixel);
    }
}
