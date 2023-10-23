namespace AdventOfCode.Animation.Terraria.Renderers
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using AdventOfCode.Animation.FFmpeg;

    public class SplashRenderer
    {
        public SplashRenderer(Screen screen)
        {
            this.Screen = screen;
            this.FadeRenderer = new(this.Screen.Width, this.Screen.Height, FadeType.In);
            this.Splash = Terraria.GetImage("Backgrounds\\Splash_9_0.png");
            this.FadeType = FadeType.In;
            this.Frame = new(this.Screen.Width, this.Screen.Height, PixelFormat.Format32bppArgb);
        }

        public int Frames { get; private set; }

        private Screen Screen { get; }

        private FadeRenderer FadeRenderer { get; }

        private Image Splash { get; }

        private Bitmap Frame { get; }

        private FadeType FadeType { get; set; }

        public void Render(FFmpegBuilder ffmpeg)
        {
            using Graphics graphics = Graphics.FromImage(this.Frame);

            this.RenderFadeIn(graphics, ffmpeg);
            this.RenderTitle(graphics, ffmpeg);
            this.RenderFadeOut(graphics, ffmpeg);
        }

        private void RenderFadeIn(Graphics graphics, FFmpegBuilder ffmpeg)
        {
            while(true)
            {
                this.Frames++;

                graphics.Clear(Color.Transparent);

                graphics.DrawImage(this.Splash, 0, 0);

                this.FadeRenderer.Render(graphics);

                ffmpeg.WriteBitmap(this.Frame, ImageFormat.Jpeg);

                if (this.FadeRenderer.Opacity == 0)
                {
                    break;
                }
            }
        }

        private void RenderFadeOut(Graphics graphics, FFmpegBuilder ffmpeg)
        {
            this.FadeRenderer.SetFadeOut();

            while (true)
            {
                this.Frames++;

                graphics.Clear(Color.Transparent);

                graphics.DrawImage(this.Splash, 0, 0);

                this.FadeRenderer.Render(graphics);

                ffmpeg.WriteBitmap(this.Frame, ImageFormat.Jpeg);

                if (this.FadeRenderer.Opacity == 255)
                {
                    break;
                }
            }
        }

        private void RenderTitle(Graphics graphics, FFmpegBuilder ffmpeg)
        {
            List<string> title = new()
            {
                "Advent Of Code 2018 Day 17",
                "Reservoir Research"
            };

            int index = 0;
            int opacity = 0;
            int center = this.Screen.Width / 2;
            using Font font = new("Source Code Pro", 80);

            while (true)
            {
                this.Frames++;

                graphics.Clear(Color.Transparent);

                graphics.DrawImage(this.Splash, 0, 0);

                SizeF size = graphics.MeasureString(title[index], font);

                using (SolidBrush brush = new(Color.FromArgb(opacity, Color.White)))
                {
                    graphics.DrawString(title[index], font, brush, center - (size.Width / 2), 400);
                }

                ffmpeg.WriteBitmap(this.Frame, ImageFormat.Jpeg);

                if (this.FadeType == FadeType.In)
                {
                    if (opacity >= 255)
                    {
                        this.FadeType = FadeType.Out;
                    }
                    else
                    {
                        opacity += 3;
                    }
                }
                else
                {
                    if (opacity <= 0)
                    {
                        this.FadeType = FadeType.In;
                        index++;

                        if (index == title.Count)
                        {
                            break;
                        }
                    }
                    else
                    {
                        opacity -= 3;
                    }
                }
            }
        }
    }
}
