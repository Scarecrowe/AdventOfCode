namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class ClumsyCrucibleTheme
    {
        public static readonly Color Background = Color.FromArgb(5, 3, 2);
        public static readonly Color HeatLow = Color.FromArgb(85, 55, 35);
        public static readonly Color HeatMid = Color.FromArgb(180, 95, 35);
        public static readonly Color HeatHigh = Color.FromArgb(255, 150, 45);
        public static readonly Color Trail = Color.FromArgb(255, 225, 95);
        public static readonly Color Player = Color.FromArgb(90, 240, 255);
        public static readonly Color Text = Color.FromArgb(255, 235, 200);
        public static readonly Color Divider = Color.FromArgb(145, 75, 35);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 10);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['@'] = '◆',
            ['^'] = '▲',
            ['>'] = '▶',
            ['v'] = '▼',
            ['<'] = '◀',
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
                '@' => Player,
                '^' or '>' or 'v' or '<' => Trail,
                '-' or '/' => Divider,
                >= '1' and <= '3' => HeatLow,
                >= '4' and <= '6' => HeatMid,
                >= '7' and <= '9' => HeatHigh,
                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(ClumsyCrucibleTheme.Background)
                .SetFont(ClumsyCrucibleTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in ClumsyCrucibleTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        ClumsyCrucibleTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        ClumsyCrucibleTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "CLUMSYRIBEAHTONVDP0123456789")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            ClumsyCrucibleTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
