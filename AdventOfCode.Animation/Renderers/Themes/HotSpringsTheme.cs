namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class HotSpringsTheme
    {
        public static readonly Color Background = Color.FromArgb(5, 8, 12);
        public static readonly Color Text = Color.FromArgb(220, 238, 245);
        public static readonly Color Unknown = Color.FromArgb(130, 170, 190);
        public static readonly Color Operational = Color.FromArgb(80, 210, 170);
        public static readonly Color Damaged = Color.FromArgb(255, 105, 105);
        public static readonly Color Current = Color.FromArgb(255, 230, 120);
        public static readonly Color Numbers = Color.FromArgb(150, 230, 255);
        public static readonly Color Group = Color.FromArgb(190, 150, 255);
        public static readonly Color Accent = Color.FromArgb(255, 170, 80);
        public static readonly Color Dim = Color.FromArgb(80, 95, 110);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['?'] = '?',
            ['#'] = '█',
            ['.'] = '·',
            ['|'] = '│',
            ['>'] = '▶',
            ['['] = '[',
            [']'] = ']',
            [','] = ',',
            ['-'] = '─',
            ['/'] = '/',
            ['^'] = '▲',
            [':'] = ':',
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
                '?' => Unknown,
                '#' => Damaged,
                '.' => Operational,
                '|' or '^' => Current,
                '>' => Accent,
                '[' or ']' or ',' => Group,
                '-' or '/' or ':' => Dim,

                >= '0' and <= '9' => Numbers,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(HotSpringsTheme.Background)
                .SetFont(HotSpringsTheme.Font)
                .SetFps(10);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in HotSpringsTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        HotSpringsTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        HotSpringsTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "HOTSPRINGSUNFOLDEDRECORDSDAMAGEDTOTALROWCOUNTMODESILVERGOLDINDEXGROUPRUNBRANCHWAYSUNKNOWNOPERATIONALCURRENTCOMPLETEARRANGEMENTSROWSPROCESSEDSCANFRAMEAVOIDTINYSCALEDRENDER")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            HotSpringsTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
