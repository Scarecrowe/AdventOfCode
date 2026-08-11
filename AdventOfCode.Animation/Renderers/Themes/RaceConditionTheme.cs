namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class RaceConditionTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 6, 12);
        public static readonly Color Wall = Color.FromArgb(48, 58, 78);
        public static readonly Color Track = Color.FromArgb(28, 38, 52);
        public static readonly Color Trail = Color.FromArgb(70, 210, 170);
        public static readonly Color Runner = Color.FromArgb(255, 225, 90);
        public static readonly Color Start = Color.FromArgb(120, 255, 170);
        public static readonly Color End = Color.FromArgb(255, 120, 120);
        public static readonly Color Cheat = Color.FromArgb(255, 90, 190);
        public static readonly Color Text = Color.FromArgb(220, 235, 255);
        public static readonly Color Numbers = Color.FromArgb(130, 230, 255);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['#'] = '█',
            ['.'] = '·',
            ['~'] = '▓',
            ['@'] = '◆',
            ['S'] = 'S',
            ['E'] = 'E',
            ['C'] = 'C',
            ['T'] = 'T',
            ['x'] = '✦',
            ['-'] = '─',
            ['/'] = '│'
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
                '.' => Track,
                '~' => Trail,
                '@' => Runner,
                'S' => Start,
                'E' => End,
                'C' or 'T' or 'x' => Cheat,

                >= '0' and <= '9' => Numbers,

                '-' or '/' => Cheat,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(RaceConditionTheme.Background)
                .SetFont(RaceConditionTheme.Font)
                .SetFps(10);

            ICharactersConfiguration characters = new CharactersConfiguration();

            for (char c = (char)32; c <= (char)126; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        RaceConditionTheme.GetColor(c),
                        RaceConditionTheme.GetCharacter(c).ToString()));
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
