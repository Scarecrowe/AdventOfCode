namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class ChronalCoordinatesTheme
    {
        public static readonly Color Background = Color.FromArgb(5, 8, 14);
        public static readonly Color Empty = Color.FromArgb(18, 24, 34);
        public static readonly Color Tie = Color.FromArgb(55, 60, 72);
        public static readonly Color InfiniteArea = Color.FromArgb(80, 110, 150);
        public static readonly Color Coordinate = Color.FromArgb(255, 215, 95);
        public static readonly Color Scanner = Color.FromArgb(255, 90, 90);
        public static readonly Color SafeRegion = Color.FromArgb(115, 235, 255);
        public static readonly Color Text = Color.FromArgb(220, 235, 255);
        public static readonly Color Numbers = Color.FromArgb(165, 255, 220);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['#'] = '█',
            ['~'] = '▒',
            ['O'] = '◆',
            ['@'] = '✦',
            ['-'] = '─',
            ['/'] = '│',
            ['<'] = '<',
            ['>'] = '>'
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
                '.' => Tie,
                '#' => SafeRegion,
                '~' => InfiniteArea,
                'O' => Coordinate,
                '@' => Scanner,

                >= '0' and <= '9' => Numbers,

                '-' or '/' or '<' or '>' => SafeRegion,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(ChronalCoordinatesTheme.Background)
                .SetFont(ChronalCoordinatesTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in ChronalCoordinatesTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        ChronalCoordinatesTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        ChronalCoordinatesTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzCHRONALCOORDINATESCLAIMINGAREASLARGESTFINITEBESTSCANNINGSAFEREGIONDISTANCESIZEVIEWPORTXY ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            ChronalCoordinatesTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}