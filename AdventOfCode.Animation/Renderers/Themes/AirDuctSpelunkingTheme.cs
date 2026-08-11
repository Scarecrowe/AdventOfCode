namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class AirDuctSpelunkingTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 6, 10);
        public static readonly Color Wall = Color.FromArgb(75, 82, 94);
        public static readonly Color Passage = Color.FromArgb(25, 32, 44);
        public static readonly Color Trail = Color.FromArgb(60, 210, 170);
        public static readonly Color Robot = Color.FromArgb(255, 220, 90);
        public static readonly Color Interface = Color.FromArgb(120, 190, 255);
        public static readonly Color CompletedInterface = Color.FromArgb(130, 255, 170);
        public static readonly Color Text = Color.FromArgb(225, 235, 245);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['#'] = '█',
            ['.'] = '·',
            ['~'] = '▓',
            ['@'] = '◆',
            ['✓'] = '✓',
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
                '#' => Wall,
                '.' => Passage,
                '~' => Trail,
                '@' => Robot,
                '✓' => CompletedInterface,

                >= '0' and <= '9' => Interface,

                '-' or '/' => Interface,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(AirDuctSpelunkingTheme.Background)
                .SetFont(AirDuctSpelunkingTheme.Font)
                .SetFps(10);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in AirDuctSpelunkingTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        AirDuctSpelunkingTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        AirDuctSpelunkingTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "AIRDUCTSPELUNKINGRETURNTOSTARGETALLWIRESBYPASSEDANDROBOTRETURNED0123456789 ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            AirDuctSpelunkingTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
