namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class MovieTheaterTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 4, 10);
        public static readonly Color Empty = Color.FromArgb(16, 18, 30);
        public static readonly Color Green = Color.FromArgb(45, 105, 72);
        public static readonly Color Loop = Color.FromArgb(95, 190, 125);
        public static readonly Color RedTile = Color.FromArgb(255, 82, 82);
        public static readonly Color Rectangle = Color.FromArgb(255, 208, 92);
        public static readonly Color CornerA = Color.FromArgb(120, 220, 255);
        public static readonly Color CornerB = Color.FromArgb(255, 140, 220);
        public static readonly Color Text = Color.FromArgb(224, 232, 255);
        public static readonly Color Numbers = Color.FromArgb(140, 255, 220);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['-'] = '━',
            ['|'] = '┃',
            ['#'] = '◆',
            ['O'] = '█',
            ['A'] = 'A',
            ['B'] = 'B',
            [','] = ',',
            ['/'] = '/',
            [':'] = ':',
            ['>'] = '>',
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
                '.' => Green,
                '-' or '|' => Loop,
                '#' => RedTile,
                'O' => Rectangle,
                'A' => CornerA,
                'B' => CornerB,

                >= '0' and <= '9' => Numbers,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(MovieTheaterTheme.Background)
                .SetFont(MovieTheaterTheme.Font)
                .SetFps(8);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in MovieTheaterTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        MovieTheaterTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        MovieTheaterTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "MOVIETHEATERPARTLARGESTOUTSIDEINSIDERECTANGLECOMPLETEAREATESTEDCOMPRESSEDFULLVIEWPORTABXY ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            MovieTheaterTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
