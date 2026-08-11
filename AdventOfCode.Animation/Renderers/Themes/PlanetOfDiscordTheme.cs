namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class PlanetOfDiscordTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 6, 10);

        public static readonly Color Empty = Color.FromArgb(30, 40, 55);
        public static readonly Color Bug = Color.FromArgb(120, 255, 140);
        public static readonly Color Recursive = Color.FromArgb(255, 210, 90);

        public static readonly Color Text = Color.FromArgb(220, 235, 255);
        public static readonly Color Numbers = Color.FromArgb(120, 220, 255);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['#'] = '●',
            ['.'] = '·',
            ['?'] = '◆',
            ['-'] = '─',
            ['/'] = '│',
            ['+'] = '+',
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

                '#' => Bug,
                '.' => Empty,
                '?' => Recursive,

                >= '0' and <= '9' => Numbers,

                '-' or '/' => Recursive,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(PlanetOfDiscordTheme.Background)
                .SetFont(PlanetOfDiscordTheme.Font)
                .SetFps(8);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in PlanetOfDiscordTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        PlanetOfDiscordTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        PlanetOfDiscordTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "PLANETOFDISCORDRECURSIVEMINUTEBIODIVERSITYFIRSTREPEATEDLAYOUTAFTERBUGSLEVELVISIBLELEVELSACTIVETOTALTO+- ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            PlanetOfDiscordTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}