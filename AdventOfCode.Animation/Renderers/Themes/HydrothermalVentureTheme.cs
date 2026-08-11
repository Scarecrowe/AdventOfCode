namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class HydrothermalVentureTheme
    {
        public static readonly Color Background = Color.FromArgb(2, 8, 15);
        public static readonly Color Empty = Color.FromArgb(10, 25, 35);
        public static readonly Color Vent = Color.FromArgb(80, 210, 255);
        public static readonly Color Danger = Color.FromArgb(255, 105, 85);
        public static readonly Color Current = Color.FromArgb(255, 235, 120);
        public static readonly Color Text = Color.FromArgb(220, 240, 255);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 8);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['@'] = '◆',
            ['+'] = '█',
        };

        public static char GetCharacter(char c)
            => CharacterMap.TryGetValue(c, out char replacement) ? replacement : c;

        public static Color GetColor(char c)
        {
            return c switch
            {
                ' ' => Background,
                '.' => Empty,
                '@' => Current,
                '+' => Danger,
                '1' => Vent,
                >= '2' and <= '9' => Danger,
                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(Background)
                .SetFont(Font)
                .SetFps(20);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in CharacterMap)
            {
                characters.Add(new CharacterConfiguration(
                    pair.Key,
                    configuration.Font,
                    GetColor(pair.Key),
                    pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(new CharacterConfiguration(
                    c,
                    configuration.Font,
                    GetColor(c),
                    c.ToString()));
            }

            foreach (char c in "HYDROTHERMALVENTUREALLLINESSTRAIGHTDANGERMAPCOMPLE0123456789/ ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(new CharacterConfiguration(
                        c,
                        configuration.Font,
                        GetColor(c),
                        c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}