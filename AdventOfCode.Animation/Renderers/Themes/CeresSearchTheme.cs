namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class CeresSearchTheme
    {
        public static readonly Color Background = Color.FromArgb(5, 6, 14);
        public static readonly Color Grid = Color.FromArgb(70, 82, 110);
        public static readonly Color Match = Color.FromArgb(255, 225, 90);
        public static readonly Color Cursor = Color.FromArgb(120, 255, 220);
        public static readonly Color Text = Color.FromArgb(225, 235, 255);
        public static readonly Color Numbers = Color.FromArgb(160, 220, 255);
        public static readonly Color Divider = Color.FromArgb(80, 95, 130);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 10);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['-'] = '─',
            ['@'] = '◆',

            ['X'] = 'X',
            ['M'] = 'M',
            ['A'] = 'A',
            ['S'] = 'S',

            ['x'] = 'X',
            ['m'] = 'M',
            ['a'] = 'A',
            ['s'] = 'S'
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
                '-' => Divider,
                '@' => Cursor,

                'x' or 'm' or 'a' or 's' => Match,

                >= '0' and <= '9' => Numbers,

                'X' or 'M' or 'A' or 'S' => Grid,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(CeresSearchTheme.Background)
                .SetFont(CeresSearchTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in CeresSearchTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        CeresSearchTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        CeresSearchTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "CERSEARCHXMASFOUNDCOUNTSCANSCELLSCENTREOMPLETE ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            CeresSearchTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
