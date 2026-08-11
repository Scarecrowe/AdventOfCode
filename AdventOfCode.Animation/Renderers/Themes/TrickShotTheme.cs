namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class TrickShotTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 8, 18);
        public static readonly Color Empty = Color.FromArgb(18, 28, 42);
        public static readonly Color Axis = Color.FromArgb(70, 90, 115);
        public static readonly Color Target = Color.FromArgb(255, 90, 90);
        public static readonly Color Trail = Color.FromArgb(90, 210, 255);
        public static readonly Color Probe = Color.FromArgb(255, 230, 90);
        public static readonly Color Start = Color.FromArgb(120, 255, 170);
        public static readonly Color Text = Color.FromArgb(220, 235, 255);
        public static readonly Color Numbers = Color.FromArgb(120, 255, 220);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 10);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['-'] = '─',
            ['T'] = '█',
            ['#'] = '▓',
            ['@'] = '◆',
            ['S'] = '▲',
            [','] = ',',
            ['/'] = '/'
        };

        public static Color GetColor(char c)
        {
            return c switch
            {
                '.' => Empty,
                '-' => Axis,
                'T' => Target,
                '#' => Trail,
                '@' => Probe,
                'S' => Start,
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
                characters.Add(new CharacterConfiguration(
                    c,
                    configuration.Font,
                    GetColor(c),
                    c.ToString()));
            }

            foreach (char c in "TRICKSHOTVELOCITYPEAKTARGETHITVALIDESFOUNDALLCURRENTTOTAL ")
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