namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class ChristmasTreeFarmTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 12, 9);
        public static readonly Color Tree = Color.FromArgb(70, 210, 120);
        public static readonly Color Empty = Color.FromArgb(18, 45, 32);
        public static readonly Color Overflow = Color.FromArgb(255, 90, 90);
        public static readonly Color Edge = Color.FromArgb(120, 255, 170);
        public static readonly Color Text = Color.FromArgb(220, 245, 230);
        public static readonly Color Numbers = Color.FromArgb(255, 220, 110);
        public static readonly Color Success = Color.FromArgb(120, 255, 170);
        public static readonly Color Warning = Color.FromArgb(255, 130, 100);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['#'] = '█',
            ['.'] = '·',
            ['+'] = '▓',
            ['['] = '▏',
            [']'] = '▕',
            ['>'] = '›',
            ['v'] = '⌄',
            ['/'] = '│',
            [':'] = ':',
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
                '#' => Tree,
                '.' => Empty,
                '+' => Overflow,
                '[' or ']' => Edge,
                '>' or 'v' => Edge,
                >= '0' and <= '9' => Numbers,
                'F' or 'I' or 'T' => Success,
                'N' or 'O' => Warning,
                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(ChristmasTreeFarmTheme.Background)
                .SetFont(ChristmasTreeFarmTheme.Font)
                .SetFps(10);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in ChristmasTreeFarmTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        ChristmasTreeFarmTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        ChristmasTreeFarmTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789:/. x+-[]>")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            ChristmasTreeFarmTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
