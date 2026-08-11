namespace AdventOfCode.Animation.Renderers.Themes
{
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class PointOfIncidenceTheme
    {
        public static readonly Color Background = Color.FromArgb(8, 8, 12);
        public static readonly Color Ash = Color.FromArgb(55, 58, 66);
        public static readonly Color Rock = Color.FromArgb(190, 196, 210);
        public static readonly Color Mirror = Color.FromArgb(120, 230, 255);
        public static readonly Color Smudge = Color.FromArgb(255, 95, 95);
        public static readonly Color Arrow = Color.FromArgb(255, 220, 120);
        public static readonly Color Text = Color.FromArgb(225, 230, 240);
        public static readonly Color Numbers = Color.FromArgb(150, 255, 210);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['#'] = '█',
            ['|'] = '│',
            ['-'] = '─',
            ['!'] = '◆',
            ['>'] = '▶',
            ['<'] = '◀',
            ['v'] = '▼',
            ['^'] = '▲',
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
                '.' => Ash,
                '#' => Rock,
                '|' or '-' or '/' => Mirror,
                '!' => Smudge,
                '>' or '<' or 'v' or '^' => Arrow,

                >= '0' and <= '9' => Numbers,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(PointOfIncidenceTheme.Background)
                .SetFont(PointOfIncidenceTheme.Font)
                .SetFps(10);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in PointOfIncidenceTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        PointOfIncidenceTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        PointOfIncidenceTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "POINTOFINCIDENCESMUDGEMODESCANNINGREFLECTIONFOUNDPATTERNVERTICALHORIZONTALCOLUMNSROWSINEDIFFTOTALSELECTEDSCORESEARCHINGFORREQUIREDANIMATIONCOMPLETESUMMARY ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            PointOfIncidenceTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
