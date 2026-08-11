namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class WarehouseWoesTheme
    {
        public static readonly Color Background = Color.FromArgb(5, 7, 10);
        public static readonly Color Wall = Color.FromArgb(80, 88, 104);
        public static readonly Color Floor = Color.FromArgb(22, 28, 36);
        public static readonly Color Box = Color.FromArgb(225, 150, 70);
        public static readonly Color WideBox = Color.FromArgb(245, 175, 85);
        public static readonly Color Robot = Color.FromArgb(105, 225, 255);
        public static readonly Color Text = Color.FromArgb(225, 235, 245);
        public static readonly Color Numbers = Color.FromArgb(145, 255, 190);
        public static readonly Color Direction = Color.FromArgb(255, 220, 100);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['#'] = '█',
            ['.'] = '·',
            ['O'] = '■',
            ['['] = '▰',
            [']'] = '▰',
            ['@'] = '◆',
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
                '.' => Floor,
                'O' => Box,
                '[' or ']' => WideBox,
                '@' => Robot,

                >= '0' and <= '9' => Numbers,

                '/' or '-' => Direction,
                '<' or '>' or '^' or 'v' => Direction,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(WarehouseWoesTheme.Background)
                .SetFont(WarehouseWoesTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in WarehouseWoesTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        WarehouseWoesTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        WarehouseWoesTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "WAREHOUSEWOESWIDECOMPLETEMOVEGPSNORTHSOUTHWESTEAST ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            WarehouseWoesTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
