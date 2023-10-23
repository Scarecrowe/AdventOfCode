namespace AdventOfCode.Animation.Terraria.Renderers
{
    using System.Drawing;

    public class FadeRenderer
    {
        public FadeRenderer(
            int width,
            int height,
            FadeType type)
        {
            this.Width = width;
            this.Height = height;
            this.Opacity = 0;

            if (type == FadeType.In)
            {
                this.SetFadeIn();
                return;
            }

            this.SetFadeOut();
        }

        public int Width { get; }

        public int Height { get; }

        public int Opacity { get; private set; }

        public FadeType Type { get; private set; }

        public bool Finished { get; private set; }

        public void SetFadeIn()
        {
            this.Opacity = 255;
            this.Type = FadeType.In;
            this.Finished = false;
        }

        public void SetFadeOut()
        {
            this.Opacity = 0;
            this.Type = FadeType.Out;
            this.Finished = false;
        }

        public void Render(Graphics graphics)
        {
            int increment = 255 / (WaterFallRenderer.Fps * 3);

            if (this.Type == FadeType.Out)
            {
                this.Opacity += increment;
            }
            else
            {
                this.Opacity -= increment;
            }

            if (this.Opacity > 255)
            {
                this.Opacity = 255;
                this.Finished = true;
            }

            if (this.Opacity < 0)
            {
                this.Opacity = 0;
                this.Finished = true;
            }

            graphics.FillRectangle(new SolidBrush(Color.FromArgb(this.Opacity, 0, 0, 0)), new(0, 0, this.Width, this.Height));
        }
    }
}
