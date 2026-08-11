namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class GuardGallivantTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 8, 10);
        public static readonly Color Floor = Color.FromArgb(26, 38, 34);
        public static readonly Color Wall = Color.FromArgb(95, 112, 104);
        public static readonly Color Trail = Color.FromArgb(80, 235, 170);
        public static readonly Color LoopTrail = Color.FromArgb(115, 220, 255);
        public static readonly Color Guard = Color.FromArgb(255, 226, 92);
        public static readonly Color Obstruction = Color.FromArgb(255, 118, 96);
        public static readonly Color Candidate = Color.FromArgb(255, 190, 90);
        public static readonly Color Text = Color.FromArgb(220, 245, 235);
        public static readonly Color Numbers = Color.FromArgb(150, 255, 210);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 8);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['#'] = '█',
            ['X'] = '▓',
            ['|'] = '│',
            ['-'] = '─',
            ['+'] = '┼',
            ['^'] = '▲',
            ['>'] = '▶',
            ['v'] = '▼',
            ['<'] = '◀',
            ['O'] = '●',
            ['o'] = '•',
            ['?'] = '?'
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
                '.' => Floor,
                '#' => Wall,
                'X' => Trail,
                '|' or '-' or '+' => LoopTrail,
                '^' or '>' or 'v' or '<' => Guard,
                'O' => Obstruction,
                'o' => Candidate,
                '?' => Candidate,

                >= '0' and <= '9' => Numbers,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(GuardGallivantTheme.Background)
                .SetFont(GuardGallivantTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in GuardGallivantTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        GuardGallivantTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        GuardGallivantTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "GUARDGALLIVANTEXITEDVISITEDSTEPLEFTTHEMAPTESTINGOBSTRUCTIONSLOOPSFOUNDOPTIONSOTALSHOWING ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            GuardGallivantTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
