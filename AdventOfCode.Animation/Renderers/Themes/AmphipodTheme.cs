namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class AmphipodTheme
    {
        public static readonly Color Background = Color.FromArgb(8, 6, 12);
        public static readonly Color Wall = Color.FromArgb(90, 80, 110);
        public static readonly Color Empty = Color.FromArgb(35, 30, 45);
        public static readonly Color Amber = Color.FromArgb(255, 210, 90);
        public static readonly Color Bronze = Color.FromArgb(220, 145, 70);
        public static readonly Color Copper = Color.FromArgb(110, 220, 210);
        public static readonly Color Desert = Color.FromArgb(210, 120, 255);
        public static readonly Color Moved = Color.FromArgb(255, 255, 255);
        public static readonly Color Text = Color.FromArgb(230, 225, 245);
        public static readonly Color Numbers = Color.FromArgb(150, 255, 220);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 24);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['#'] = '█',
            ['.'] = '·',
            ['A'] = 'A',
            ['B'] = 'B',
            ['C'] = 'C',
            ['D'] = 'D',
            ['a'] = 'A',
            ['b'] = 'B',
            ['c'] = 'C',
            ['d'] = 'D',
            ['-'] = '─',
            ['/'] = '│'
        };

        public static Color GetColor(char c)
        {
            return c switch
            {
                ' ' => Background,
                '#' => Wall,
                '.' => Empty,

                'A' => Amber,
                'B' => Bronze,
                'C' => Copper,
                'D' => Desert,

                'a' or 'b' or 'c' or 'd' => Moved,

                >= '0' and <= '9' => Numbers,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(Background)
                .SetFont(Font)
                .SetFps(1);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "AMPHIPODUNFOLDEDCOMPLETEENERGY ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
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