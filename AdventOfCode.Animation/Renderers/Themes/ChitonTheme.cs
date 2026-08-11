namespace AdventOfCode.Animation.Renderers.Themes
{
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class ChitonTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 8, 12);
        public static readonly Color Risk = Color.FromArgb(70, 95, 110);
        public static readonly Color Settled = Color.FromArgb(35, 55, 70);
        public static readonly Color Frontier = Color.FromArgb(255, 190, 80);
        public static readonly Color Path = Color.FromArgb(80, 255, 170);
        public static readonly Color Player = Color.FromArgb(255, 245, 120);
        public static readonly Color Target = Color.FromArgb(255, 90, 120);
        public static readonly Color Text = Color.FromArgb(220, 240, 255);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 8);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            ['.'] = '·',
            ['*'] = '◆',
            ['~'] = '▓',
            ['@'] = '●',
            ['X'] = '✦',
            [' '] = ' '
        };

        public static Color GetColor(char c)
        {
            return c switch
            {
                '.' => Settled,
                '*' => Frontier,
                '~' => Path,
                '@' => Player,
                'X' => Target,
                >= '0' and <= '9' => Risk,
                _ => Text
            };
        }

        public static char GetCharacter(char c)
        {
            return CharacterMap.TryGetValue(c, out char replacement)
                ? replacement
                : c;
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(Background)
                .SetFont(Font)
                .SetFps(30);

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

            foreach (char c in "CHITONSEARCHINGLOWESTRISKEXITREACHED ")
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
