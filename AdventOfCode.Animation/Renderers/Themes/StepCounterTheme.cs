namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class StepCounterTheme
    {
        public static readonly Color Background = Color.FromArgb(5, 10, 8);
        public static readonly Color Rock = Color.FromArgb(58, 66, 62);
        public static readonly Color Garden = Color.FromArgb(18, 42, 25);
        public static readonly Color Current = Color.FromArgb(180, 255, 120);
        public static readonly Color Previous = Color.FromArgb(55, 130, 75);
        public static readonly Color Start = Color.FromArgb(255, 230, 120);
        public static readonly Color Grid = Color.FromArgb(45, 90, 65);
        public static readonly Color Text = Color.FromArgb(220, 245, 225);
        public static readonly Color Numbers = Color.FromArgb(130, 255, 190);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['#'] = '█',
            ['.'] = '·',
            [','] = '┊',
            ['S'] = 'S',
            ['@'] = '◆',
            ['O'] = '●',
            ['o'] = '·',
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
                '#' => Rock,
                '.' => Garden,
                ',' => Grid,
                'S' => Start,
                '@' => Start,
                'O' => Current,
                'o' => Previous,

                >= '0' and <= '9' => Numbers,

                '-' or '/' => Grid,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(StepCounterTheme.Background)
                .SetFont(StepCounterTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in StepCounterTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        StepCounterTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        StepCounterTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "STEPCOUNTERFINITEFARMINARGETKVWDOFHPQLXAB")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            StepCounterTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
