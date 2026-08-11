namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class OxygenSystemTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 8, 12);

        public static readonly Color Empty = Color.FromArgb(60, 90, 110);
        public static readonly Color Wall = Color.FromArgb(0, 180, 255);
        public static readonly Color Oxygen = Color.FromArgb(80, 255, 180);
        public static readonly Color Droid = Color.FromArgb(255, 220, 80);
        public static readonly Color Start = Color.FromArgb(255, 120, 80);

        public static readonly Color Text = Color.FromArgb(220, 235, 255);
        public static readonly Color Numbers = Color.FromArgb(120, 255, 220);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");

        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['#'] = '█',
            ['.'] = '·',
            ['~'] = '▓',
            ['@'] = '◆',
            ['+'] = '◉',

            ['0'] = '0',
            ['1'] = '1',
            ['2'] = '2',
            ['3'] = '3',
            ['4'] = '4',
            ['5'] = '5',
            ['6'] = '6',
            ['7'] = '7',
            ['8'] = '8',
            ['9'] = '9',

            ['/'] = '│',
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
                ' ' => Background,

                '#' => Wall,
                '.' => Empty,
                '~' => Oxygen,
                '@' => Droid,
                '+' => Start,

                >= '0' and <= '9' => Numbers,

                '/' or '-' => Wall,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(OxygenSystemTheme.Background)
                .SetFont(OxygenSystemTheme.Font)
                .SetFps(8);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in OxygenSystemTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        OxygenSystemTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            foreach (char c in "DROIDEXPLORATIONOXYGENFILLMINUTE DISTANCE")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            OxygenSystemTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}