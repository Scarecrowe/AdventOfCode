namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class CryostasisTheme
    {
        public static readonly Color Background = Color.FromArgb(2, 8, 14);
        public static readonly Color Text = Color.FromArgb(210, 240, 245);
        public static readonly Color Command = Color.FromArgb(120, 255, 180);
        public static readonly Color Title = Color.FromArgb(255, 220, 100);
        public static readonly Color Border = Color.FromArgb(80, 180, 220);
        public static readonly Color Number = Color.FromArgb(255, 140, 90);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static Color GetColor(char c)
        {
            return c switch
            {
                '>' => Command,
                '-' => Border,
                >= '0' and <= '9' => Number,
                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(Background)
                .SetFont(Font)
                .SetFps(1);

            ICharactersConfiguration characters = new CharactersConfiguration();

            for (char c = (char)32; c <= (char)126; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        GetColor(c),
                        c.ToString()));
            }

            characters.Add(
                new CharacterConfiguration(
                    '…',
                    configuration.Font,
                    Text,
                    "…"));

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}