namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class CarePackageTheme
    {
        public static readonly Color Background = Color.FromArgb(5, 5, 18);

        public static readonly Color Empty = Color.FromArgb(5, 5, 18);
        public static readonly Color Wall = Color.FromArgb(0, 220, 255);
        public static readonly Color Block = Color.FromArgb(255, 80, 180);
        public static readonly Color Paddle = Color.FromArgb(0, 255, 120);
        public static readonly Color Ball = Color.FromArgb(255, 230, 80);
        public static readonly Color Text = Color.FromArgb(220, 220, 255);
        public static readonly Color Divider = Color.FromArgb(80, 120, 180);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");

        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['#'] = '█',
            ['▒'] = '▓',
            ['='] = '▄',
            ['o'] = '●',
            ['-'] = '─'
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
                ' ' => Empty,
                '#' => Wall,
                '▒' => Block,
                '=' => Paddle,
                'o' => Ball,
                '-' => Divider,

                'S' or 'C' or 'O' or 'R' or 'E' or 'B' or 'L' or 'K' or ':' => Text,
                >= '0' and <= '9' => Text,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(CarePackageTheme.Background)
                .SetFont(CarePackageTheme.Font)
                .SetFps(6);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in CarePackageTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        CarePackageTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        CarePackageTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "SCOREBLOCKS:")
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        CarePackageTheme.GetColor(c),
                        c.ToString()));
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
