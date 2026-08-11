namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class SetAndForgetTheme
    {
        public static readonly Color Background = Color.FromArgb(3, 6, 10);

        public static readonly Color OpenSpace = Color.FromArgb(12, 18, 28);
        public static readonly Color Scaffold = Color.FromArgb(120, 220, 255);
        public static readonly Color Intersection = Color.FromArgb(255, 220, 80);
        public static readonly Color Cleaned = Color.FromArgb(80, 255, 180);
        public static readonly Color Robot = Color.FromArgb(255, 120, 80);
        public static readonly Color Text = Color.FromArgb(220, 235, 255);
        public static readonly Color Numbers = Color.FromArgb(120, 255, 220);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");

        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['#'] = '█',
            ['+'] = '✸',
            ['~'] = '▓',

            ['@'] = '◆',
            ['^'] = '▲',
            ['v'] = '▼',
            ['<'] = '◄',
            ['>'] = '►',
            ['X'] = '✖',

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
                '.' => OpenSpace,
                '#' => Scaffold,
                '+' => Intersection,
                '~' => Cleaned,

                '@' or '^' or 'v' or '<' or '>' or 'X' => Robot,

                >= '0' and <= '9' => Numbers,

                '/' or '-' => Scaffold,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(SetAndForgetTheme.Background)
                .SetFont(SetAndForgetTheme.Font)
                .SetFps(8);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in SetAndForgetTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        SetAndForgetTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        SetAndForgetTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "CAMERACALIBRATIONALIGNMENTINTERSECTIONFOUNDCOMPLETEVACUUMROBOTONLINECLEANINGSCAFFOLDSTEPSTURNDUSTCOLLECTED ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            SetAndForgetTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}