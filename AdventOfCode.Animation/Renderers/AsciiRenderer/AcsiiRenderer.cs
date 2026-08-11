namespace AdventOfCode.Animation.Renderers.AsciiRenderer
{
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using AdventOfCode.Animation.FFmpeg;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Exceptions;

    public class AsciiRenderer : IAsciiRenderer
    {
        public AsciiRenderer(
            IAsciiRendererConfiguration configuration,
            SolutionType solutionType,
            int year,
            int day)
        {
            this.SolutionType = solutionType;
            this.Configuration = configuration;
            this.Year = year;
            this.Day = day;
            this.DayTitle = configuration.Title;
            this.Frames = new Frames();
            this.HorizontalPadding = 3;
            this.VerticalPadding = 6;
        }

        public IAsciiRendererConfiguration Configuration { get; set; }

        public IFrames Frames { get; }

        public SolutionType SolutionType { get; }

        public int Year { get; }

        public int Day { get; }

        public string DayTitle { get; }

        private FFmpegBuilder? Ffmpeg { get; set; }

        private (int Width, int Height)? FontSize { get; set; }

        private int HorizontalPadding { get; }

        private int VerticalPadding { get; }

        private (int Width, int Height)? ViewPortSize { get; set; }

        private Bitmap? ViewPort { get; set; }

        private Graphics? Graphics { get; set; }

        private Stopwatch? TimeoutStopwatch { get; set; }

        private TimeSpan? Timeout { get; set; }

        public void SetTimeout(TimeSpan timeout)
        {
            this.Timeout = timeout;
            this.TimeoutStopwatch = Stopwatch.StartNew();
        }

        public void Render()
        {
            if (this.Ffmpeg != null)
            {
                return;
            }

            this.Ffmpeg = this.Configuration.OutputFormat == RenderFileFormat.Mp4
                ? new AsciiVideoRenderer(this.Year, this.Day, this.Configuration.Fps, this.SolutionType).BuildMp4().Run()
                : new AsciiVideoRenderer(this.Year, this.Day, this.Configuration.Fps, this.SolutionType).BuildGif().Run();
        }

        public void Finish()
        {
            if (this.ViewPort != null)
            {
                for (int i = 1; i <= this.Configuration.Fps * 2; i++)
                {
                    this.Ffmpeg?.WriteBitmap(this.ViewPort, ImageFormat.Jpeg);
                }
            }

            this.Ffmpeg?.Quit();
            this.Graphics?.Dispose();
            this.ViewPort?.Dispose();
        }

        public void RenderFrame(IFrame frame)
        {
            this.Render();
            this.SetViewPortSize(frame.Data[0].Length, frame.Data.Length);

            if (this.ViewPortSize == null
                || this.FontSize == null
                || this.ViewPort == null)
            {
                return;
            }

            int width = this.ViewPortSize.Value.Width;
            int height = this.ViewPortSize.Value.Height;

            this.FillBackground(width, height);

            for (int i = 0; i < frame.Data.Length; i++)
            {
                int x = this.HorizontalPadding;
                int y = (i * this.FontSize.Value.Height) + this.VerticalPadding;

                for (int j = 0; j < frame.Data[i].Length; j++)
                {
                    char chr = frame.Data[i][j];

                    ICharacterConfiguration? configuration = this.Configuration.Characters?.FirstOrDefault(x => x.Character == chr) ?? null;

                    if (configuration == null)
                    {
                        this.Graphics?.DrawString(
                            chr.ToString(),
                            this.Configuration.Font ?? DefaultTheme.Font,
                            new SolidBrush(this.Configuration.FontColor ?? DefaultTheme.FontColor),
                            x,
                            y);
                    }
                    else
                    {
                        if (configuration.Visible
                            && configuration.Font != null
                            && configuration.Color != null)
                        {
                            string display = chr.ToString();

                            if (!string.IsNullOrEmpty(configuration.Replace.ToString()))
                            {
                                display = configuration.Replace.ToString() ?? chr.ToString();
                            }

                            this.Graphics?.DrawString(display, configuration.Font, new SolidBrush(configuration.Color.Value), x, y);
                        }
                    }

                    x += this.FontSize.Value.Width;
                }
            }

            this.Ffmpeg?.WriteBitmap(ResizeIfTooLarge(this.ViewPort), ImageFormat.Jpeg);
        }

        private static Bitmap ResizeIfTooLarge(Bitmap source)
        {
            const int maxWidth = 1920;
            const int maxHeight = 1080;

            if (source.Width <= maxWidth
                && source.Height <= maxHeight)
            {
                return source;
            }

            double scale = Math.Min(
                (double)maxWidth / source.Width,
                (double)maxHeight / source.Height);

            int newWidth = (int)Math.Floor(source.Width * scale);
            int newHeight = (int)Math.Floor(source.Height * scale);

            if (newWidth % 2 != 0)
            {
                newWidth--;
            }

            if (newHeight % 2 != 0)
            {
                newHeight--;
            }

            newWidth = Math.Max(2, newWidth);
            newHeight = Math.Max(2, newHeight);

            Bitmap resized = new(newWidth, newHeight);

            using Graphics graphics = Graphics.FromImage(resized);

            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            graphics.DrawImage(source, 0, 0, newWidth, newHeight);

            return resized;
        }

        private static (int Width, int Height) GetFontRenderSize(string value, Font font)
        {
            using Bitmap bmp = new(1, 1);
            using Graphics graphics = Graphics.FromImage(bmp);

            int height = (int)Math.Ceiling(font.GetHeight(graphics));

            RectangleF rect = new(0, 0, 1000, 1000);
            using StringFormat format = new();
            format.SetMeasurableCharacterRanges(new[] { new CharacterRange(0, 1) });

            Region[] regions = graphics.MeasureCharacterRanges(value, font, rect, format);
            RectangleF bounds = regions[0].GetBounds(graphics);

            int width = (int)Math.Ceiling(bounds.Width);

            return (width, height);
        }

        private void SetFontSize()
        {
            if (this.FontSize != null)
            {
                return;
            }

            this.FontSize = GetFontRenderSize("0", this.Configuration.Font ?? DefaultTheme.Font);
        }

        private void SetViewPortSize(int width, int height)
        {
            this.SetFontSize();

            if (this.FontSize == null)
            {
                return;
            }

            width = (width * this.FontSize.Value.Width) + (this.HorizontalPadding * 3);
            height = (this.FontSize.Value.Height * height) + (this.VerticalPadding * 2);

            if (width % 2 != 0)
            {
                width--;
            }

            if (height % 2 != 0)
            {
                height--;
            }

            this.ViewPortSize = (width, height);

            this.ViewPort = new(width, height);
            this.Graphics = Graphics.FromImage(this.ViewPort);
        }

        private void FillBackground(int width, int height)
        {
            this.Graphics?.FillRectangle(new SolidBrush(this.Configuration.BackgroundColor ?? DefaultTheme.BackgroundColor), new Rectangle(0, 0, width, height));
        }
    }
}
