namespace AdventOfCode.Animation.Renderers.Themes
{
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class BlizzardBasinTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 8, 18);
        public static readonly Color Wall = Color.FromArgb(70, 90, 120);
        public static readonly Color Ground = Color.FromArgb(20, 35, 55);
        public static readonly Color Blizzard = Color.FromArgb(120, 220, 255);
        public static readonly Color Trail = Color.FromArgb(80, 255, 180);
        public static readonly Color Player = Color.FromArgb(255, 230, 90);
        public static readonly Color Text = Color.FromArgb(225, 240, 255);
        public static readonly Color Numbers = Color.FromArgb(255, 150, 220);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['#'] = '█',
            ['.'] = '·',
            ['~'] = '▓',
            ['@'] = '◆',
            ['>'] = '▶',
            ['<'] = '◀',
            ['^'] = '▲',
            ['v'] = '▼',
            ['*'] = '✹',
            ['/'] = '│'
        };

        public static Color GetColor(char c)
        {
            return c switch
            {
                ' ' => Background,
                '#' => Wall,
                '.' => Ground,
                '~' => Trail,
                '@' => Player,
                '>' or '<' or '^' or 'v' or '*' => Blizzard,
                >= '0' and <= '9' => Numbers,
                '/' => Blizzard,
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

            foreach (char c in "BLIZZARDBASINROUNDTRIPMINUTE ")
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
