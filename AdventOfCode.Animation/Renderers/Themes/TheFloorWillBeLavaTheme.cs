namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class TheFloorWillBeLavaTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 3, 8);
        public static readonly Color Empty = Color.FromArgb(30, 24, 34);
        public static readonly Color Energized = Color.FromArgb(255, 92, 44);
        public static readonly Color Beam = Color.FromArgb(255, 236, 120);
        public static readonly Color Mirror = Color.FromArgb(120, 220, 255);
        public static readonly Color Splitter = Color.FromArgb(255, 150, 70);
        public static readonly Color EnergizedMirror = Color.FromArgb(170, 245, 255);
        public static readonly Color EnergizedSplitter = Color.FromArgb(255, 210, 110);
        public static readonly Color Text = Color.FromArgb(245, 230, 210);
        public static readonly Color Numbers = Color.FromArgb(255, 180, 80);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 10);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['#'] = '▓',
            ['>'] = '▶',
            ['<'] = '◀',
            ['^'] = '▲',
            ['v'] = '▼',
            ['/'] = '╱',
            ['\\'] = '╲',
            ['|'] = '│',
            ['-'] = '─',
            ['╱'] = '╱',
            ['╲'] = '╲',
            ['┃'] = '┃',
            ['━'] = '━'
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
                '.' => Empty,
                '#' => Energized,

                '>' or '<' or '^' or 'v' => Beam,

                '/' or '\\' => Mirror,
                '|' or '-' => Splitter,

                '╱' or '╲' => EnergizedMirror,
                '┃' or '━' => EnergizedSplitter,

                >= '0' and <= '9' => Numbers,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(TheFloorWillBeLavaTheme.Background)
                .SetFont(TheFloorWillBeLavaTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in TheFloorWillBeLavaTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        TheFloorWillBeLavaTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        TheFloorWillBeLavaTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "THEFLOORWILLBELAVABESTCONFIGURATIONMAXSTEPENERGIZED ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            TheFloorWillBeLavaTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
