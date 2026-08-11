namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class SeaCucumberTheme
    {
        public static readonly Color Background = Color.FromArgb(2, 10, 18);
        public static readonly Color Empty = Color.FromArgb(10, 35, 50);
        public static readonly Color East = Color.FromArgb(255, 190, 90);
        public static readonly Color South = Color.FromArgb(90, 220, 180);
        public static readonly Color Text = Color.FromArgb(210, 240, 255);
        public static readonly Color Numbers = Color.FromArgb(120, 255, 220);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 10);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['>'] = '►',
            ['v'] = '▼',
            ['/'] = '│'
        };

        public static Color GetColor(char c)
        {
            return c switch
            {
                '.' => Empty,
                '>' => East,
                'v' => South,
                >= '0' and <= '9' => Numbers,
                '/' => South,
                ' ' => Background,
                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(Background)
                .SetFont(Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in CharacterMap)
            {
                characters.Add(new CharacterConfiguration(
                    pair.Key,
                    configuration.Font,
                    GetColor(pair.Key),
                    pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(new CharacterConfiguration(
                    c,
                    configuration.Font,
                    GetColor(c),
                    c.ToString()));
            }

            foreach (char c in "SEACUCUMBERMIGRATIONINITIALSCANLOCKEDLANDEDNOMOVEMENTSTEP ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(new CharacterConfiguration(
                        c,
                        configuration.Font,
                        GetColor(c),
                        c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}