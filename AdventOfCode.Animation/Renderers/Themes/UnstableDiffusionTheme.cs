namespace AdventOfCode.Animation.Renderers.Themes
{
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class UnstableDiffusionTheme
    {
        public static readonly Color Background = Color.FromArgb(8, 8, 10);
        public static readonly Color Ground = Color.FromArgb(35, 35, 38);
        public static readonly Color Elf = Color.FromArgb(180, 255, 160);
        public static readonly Color Text = Color.FromArgb(230, 235, 220);
        public static readonly Color Numbers = Color.FromArgb(255, 220, 120);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['#'] = '●',
            ['/'] = '│'
        };

        public static Color GetColor(char c)
        {
            return c switch
            {
                '.' => Ground,
                '#' => Elf,
                >= '0' and <= '9' => Numbers,
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
                characters.Add(new CharacterConfiguration(c, configuration.Font, GetColor(c), c.ToString()));
            }

            foreach (char c in "UNSTABLEDIFFUSIONROUNDSTABLEAFTEREMPTY ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(new CharacterConfiguration(c, configuration.Font, GetColor(c), c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
