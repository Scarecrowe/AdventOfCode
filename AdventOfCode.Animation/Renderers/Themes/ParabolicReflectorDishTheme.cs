namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class ParabolicReflectorDishTheme
    {
        public static readonly Color Background = Color.FromArgb(5, 7, 12);
        public static readonly Color Empty = Color.FromArgb(25, 32, 42);
        public static readonly Color CubeRock = Color.FromArgb(95, 105, 115);
        public static readonly Color RoundRock = Color.FromArgb(255, 216, 95);
        public static readonly Color Text = Color.FromArgb(220, 235, 255);
        public static readonly Color Numbers = Color.FromArgb(125, 245, 220);
        public static readonly Color Accent = Color.FromArgb(120, 190, 255);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['#'] = '█',
            ['O'] = '●',
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
                '#' => CubeRock,
                'O' => RoundRock,

                >= '0' and <= '9' => Numbers,

                '-' or '/' => Accent,
                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(ParabolicReflectorDishTheme.Background)
                .SetFont(ParabolicReflectorDishTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in ParabolicReflectorDishTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        ParabolicReflectorDishTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        ParabolicReflectorDishTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "PARABOLICREFLECTORDISHSPINCYCLELOADROUNDROCKSNORTHWESTSOUTHEASTMOVEDETECTEDSTARTLENGTHFINAL ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            ParabolicReflectorDishTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
