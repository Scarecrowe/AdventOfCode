namespace AdventOfCode.Animation.Renderers.AsciiRenderer
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core.ConsoleMenu.Configuration;

    public interface IAsciiRendererConfiguration : IConfiguration
    {
        string Title { get; }

        Color? BackgroundColor { get; }

        Color? FontColor { get; }

        Font? Font { get; }

        ICharactersConfiguration? Characters { get; }

        RenderFileFormat OutputFormat { get; }

        int Fps { get; }

        IAsciiRendererConfiguration SetCharacters(ICharactersConfiguration config);

        IAsciiRendererConfiguration SetBackgroundColor(Color color);

        IAsciiRendererConfiguration SetFontColor(Color color);

        IAsciiRendererConfiguration SetFont(Font font);

        IAsciiRendererConfiguration SetOutputFormat(RenderFileFormat format);

        IAsciiRendererConfiguration SetFps(int fps);
    }
}
