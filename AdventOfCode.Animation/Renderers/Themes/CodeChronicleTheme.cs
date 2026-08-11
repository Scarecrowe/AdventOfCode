namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class CodeChronicleTheme
    {
        public static readonly Color Background = Color.FromArgb(6, 8, 14);
        public static readonly Color Lock = Color.FromArgb(120, 170, 255);
        public static readonly Color Key = Color.FromArgb(255, 210, 90);
        public static readonly Color Empty = Color.FromArgb(28, 34, 48);
        public static readonly Color Fit = Color.FromArgb(90, 255, 170);
        public static readonly Color Overlap = Color.FromArgb(255, 90, 90);
        public static readonly Color Text = Color.FromArgb(220, 235, 255);
        public static readonly Color Numbers = Color.FromArgb(130, 255, 235);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 18);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['#'] = '█',
            ['.'] = '·',
            ['o'] = '✓',
            ['x'] = '✗',
            ['/'] = '/',
            ['-'] = '-',
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
                '#' => Lock,
                '.' => Empty,
                'o' => Fit,
                'x' => Overlap,

                >= '0' and <= '9' => Numbers,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(CodeChronicleTheme.Background)
                .SetFont(CodeChronicleTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in CodeChronicleTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        CodeChronicleTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        CodeChronicleTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789/ .")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            CodeChronicleTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
