namespace AdventOfCode.Animation.Renderers.AsciiRenderer
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.Configuration;

    public class AsciiRendererConfiguration : IAsciiRendererConfiguration
    {
        public AsciiRendererConfiguration(string title)
        {
            this.Title = title;
            this.Characters = new CharactersConfiguration();
            this.BackgroundColor = Color.Black;
            this.FontColor = Color.White;
            this.Font = new Font("Lucida Console", 12);
            this.OutputFormat = RenderFileFormat.Mp4;
            this.Fps = 6;
        }

        public string Title { get; private set; }

        public ICharactersConfiguration? Characters { get; private set; }

        public Color? BackgroundColor { get; private set; }

        public Color? FontColor { get; private set; }

        public Font? Font { get; private set; }

        public int Fps { get; private set; }

        public RenderFileFormat OutputFormat { get; private set; }

        public IAsciiRendererConfiguration SetTitle(string title)
        {
            this.Title = title;

            return this;
        }

        public IAsciiRendererConfiguration SetCharacters(ICharactersConfiguration characters)
        {
            this.Characters = characters;

            return this;
        }

        public IAsciiRendererConfiguration SetBackgroundColor(Color color)
        {
            this.BackgroundColor = color;

            return this;
        }

        public IAsciiRendererConfiguration SetFontColor(Color color)
        {
            this.FontColor = color;

            return this;
        }

        public IAsciiRendererConfiguration SetFont(Font font)
        {
            this.Font = font;

            return this;
        }

        public IAsciiRendererConfiguration SetFps(int fps)
        {
            this.Fps = fps;

            return this;
        }

        public IAsciiRendererConfiguration SetOutputFormat(RenderFileFormat format)
        {
            this.OutputFormat = format;

            return this;
        }
    }
}
