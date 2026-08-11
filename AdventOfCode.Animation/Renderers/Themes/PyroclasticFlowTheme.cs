namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class PyroclasticFlowTheme
    {
        public static readonly Color Background = Color.FromArgb(5, 4, 8);
        public static readonly Color Wall = Color.FromArgb(95, 85, 105);
        public static readonly Color Rock = Color.FromArgb(220, 95, 55);
        public static readonly Color Falling = Color.FromArgb(255, 210, 80);
        public static readonly Color Air = Color.FromArgb(25, 20, 30);
        public static readonly Color Text = Color.FromArgb(245, 225, 210);
        public static readonly Color Numbers = Color.FromArgb(255, 165, 90);
        public static readonly Color Jet = Color.FromArgb(120, 220, 255);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['#'] = '█',
            ['@'] = '◆',
            ['|'] = '│',
            ['-'] = '─',
            ['+'] = '┴',
            ['<'] = '←',
            ['>'] = '→',
            ['/'] = '/'
        };

        public static Color GetColor(char c)
        {
            return c switch
            {
                '.' => Air,
                '#' => Rock,
                '@' => Falling,
                '|' or '-' or '+' => Wall,
                '<' or '>' => Jet,
                >= '0' and <= '9' => Numbers,
                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(Background)
                .SetFont(Font)
                .SetFps(16);

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

            foreach (char c in "PYROCLASTICFLOWROCKHEIGHTJETFORMATIONSPAWNFALLRESTBLOCKED ")
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