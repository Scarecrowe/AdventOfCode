namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class TransparentOrigamiTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 8, 14);
        public static readonly Color Paper = Color.FromArgb(16, 28, 40);
        public static readonly Color Dot = Color.FromArgb(120, 255, 220);
        public static readonly Color FoldLine = Color.FromArgb(255, 190, 90);
        public static readonly Color Text = Color.FromArgb(225, 240, 255);
        public static readonly Color Numbers = Color.FromArgb(150, 220, 255);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['#'] = '█',
            ['|'] = '│',
            ['-'] = '─',
            ['+'] = '┼',
            ['/'] = '╱',
            ['\\'] = '╲'
        };

        public static char GetCharacter(char c)
        {
            return CharacterMap.TryGetValue(c, out char replacement)
                ? replacement
                : c;
        }

        public static Color GetColor(char c)
        {
            return c switch
            {
                ' ' => Background,
                '.' => Paper,
                '#' => Dot,
                '|' or '-' or '+' or '/' or '\\' => FoldLine,

                >= '0' and <= '9' => Numbers,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(TransparentOrigamiTheme.Background)
                .SetFont(TransparentOrigamiTheme.Font)
                .SetFps(8);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in TransparentOrigamiTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        TransparentOrigamiTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        TransparentOrigamiTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "TRANSPARENTORIGAMIFOLDDOTSCODEFIRSTCOMPLETEPZFJHR ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            TransparentOrigamiTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}