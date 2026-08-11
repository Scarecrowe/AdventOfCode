namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class CosmicExpansionTheme
    {
        public static readonly Color Background = Color.FromArgb(2, 4, 12);
        public static readonly Color Space = Color.FromArgb(18, 24, 42);
        public static readonly Color Expansion = Color.FromArgb(55, 75, 120);
        public static readonly Color Trail = Color.FromArgb(90, 120, 180);
        public static readonly Color CurrentPath = Color.FromArgb(255, 210, 90);
        public static readonly Color Galaxy = Color.FromArgb(180, 230, 255);
        public static readonly Color Start = Color.FromArgb(120, 255, 180);
        public static readonly Color End = Color.FromArgb(255, 120, 180);
        public static readonly Color Text = Color.FromArgb(220, 235, 255);
        public static readonly Color Numbers = Color.FromArgb(160, 255, 220);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['+'] = '░',
            ['~'] = '▒',
            ['*'] = '█',
            ['@'] = '◆',
            ['$'] = '◇'
        };

        public static Color GetColor(char c)
        {
            return c switch
            {
                ' ' => Background,
                '.' => Space,
                '+' => Expansion,
                '~' => Trail,
                '*' => CurrentPath,
                '@' => Start,
                '$' => End,

                >= 'A' and <= 'Z' => Galaxy,
                >= '0' and <= '9' => Numbers,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(CosmicExpansionTheme.Background)
                .SetFont(CosmicExpansionTheme.Font)
                .SetFps(8);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in CosmicExpansionTheme.CharacterMap)
            {
                characters.Add(new CharacterConfiguration(
                    pair.Key,
                    configuration.Font,
                    CosmicExpansionTheme.GetColor(pair.Key),
                    pair.Value.ToString()));
            }

            for (char c = 'A'; c <= 'Z'; c++)
            {
                characters.Add(new CharacterConfiguration(
                    c,
                    configuration.Font,
                    CosmicExpansionTheme.GetColor(c),
                    "✦"));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(new CharacterConfiguration(
                    c,
                    configuration.Font,
                    CosmicExpansionTheme.GetColor(c),
                    c.ToString()));
            }

            foreach (char c in "COSMICEXPANSIONDEEPxPAIR/DISTANCETOTAL ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(new CharacterConfiguration(
                        c,
                        configuration.Font,
                        CosmicExpansionTheme.GetColor(c),
                        c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}