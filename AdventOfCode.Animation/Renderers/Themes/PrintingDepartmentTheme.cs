namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class PrintingDepartmentTheme
    {
        public static readonly Color Background = Color.FromArgb(6, 8, 12);
        public static readonly Color Empty = Color.FromArgb(18, 24, 32);
        public static readonly Color Paper = Color.FromArgb(232, 224, 190);
        public static readonly Color Accessible = Color.FromArgb(255, 210, 80);
        public static readonly Color Removed = Color.FromArgb(90, 170, 255);
        public static readonly Color Text = Color.FromArgb(225, 235, 245);
        public static readonly Color Numbers = Color.FromArgb(140, 255, 220);
        public static readonly Color GridInfo = Color.FromArgb(150, 165, 185);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['@'] = '●',
            ['+'] = '◆',
            ['x'] = '×',
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
                '.' => Empty,
                '@' => Paper,
                '+' => Accessible,
                'x' => Removed,

                >= '0' and <= '9' => Numbers,

                '/' or '-' => GridInfo,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(PrintingDepartmentTheme.Background)
                .SetFont(PrintingDepartmentTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in PrintingDepartmentTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        PrintingDepartmentTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        PrintingDepartmentTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "PRINTINGDEPARTMENTACCESSIBLEROLLSINITIALSTATEWAVEREMOVINGTHISTOTALSTOPVIEWXYSCANN")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            PrintingDepartmentTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
