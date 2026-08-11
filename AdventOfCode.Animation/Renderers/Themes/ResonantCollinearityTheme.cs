namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class ResonantCollinearityTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 8, 16);
        public static readonly Color Empty = Color.FromArgb(24, 34, 52);
        public static readonly Color Antenna = Color.FromArgb(150, 190, 255);
        public static readonly Color ActiveAntenna = Color.FromArgb(255, 220, 90);
        public static readonly Color AntiNode = Color.FromArgb(110, 255, 190);
        public static readonly Color Overlap = Color.FromArgb(255, 120, 210);
        public static readonly Color Text = Color.FromArgb(225, 238, 255);
        public static readonly Color Numbers = Color.FromArgb(255, 220, 90);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['#'] = '◆',
            ['@'] = '◈',
            ['*'] = '✦',
            ['1'] = '❶',
            ['2'] = '❷',
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
                '.' => Empty,
                '#' => AntiNode,
                '@' => Overlap,
                '*' => ActiveAntenna,
                '1' or '2' => ActiveAntenna,

                >= '0' and <= '9' => Numbers,
                >= 'a' and <= 'z' => Antenna,
                >= 'A' and <= 'Z' => Text,

                '-' or '/' => AntiNode,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(ResonantCollinearityTheme.Background)
                .SetFont(ResonantCollinearityTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in ResonantCollinearityTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        ResonantCollinearityTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            ResonantCollinearityTheme.GetColor(c),
                            c.ToString()));
                }
            }

            for (char c = 'a'; c <= 'z'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        ResonantCollinearityTheme.GetColor(c),
                        c.ToString()));
            }

            for (char c = 'A'; c <= 'Z'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        ResonantCollinearityTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "RESONANTCOLLINEARITYHARMONICSFREQUENCYPAIRANTINODESCOMPLETESTARTUNIQUE ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            ResonantCollinearityTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
