namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class RainRiskTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 10, 18);
        public static readonly Color Axis = Color.FromArgb(40, 70, 95);
        public static readonly Color Trail = Color.FromArgb(80, 210, 255);
        public static readonly Color Ship = Color.FromArgb(255, 230, 90);
        public static readonly Color Waypoint = Color.FromArgb(255, 120, 180);
        public static readonly Color Vector = Color.FromArgb(120, 255, 170);
        public static readonly Color Text = Color.FromArgb(220, 240, 255);
        public static readonly Color Numbers = Color.FromArgb(130, 255, 220);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['-'] = '─',
            ['|'] = '│',
            ['+'] = '┼',
            ['~'] = '·',
            ['@'] = '◆',
            ['W'] = '◇',
            ['*'] = '•'
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
                '-' or '|' or '+' => Axis,
                '~' => Trail,
                '@' => Ship,
                'W' => Waypoint,
                '*' => Vector,
                >= '0' and <= '9' => Numbers,
                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(RainRiskTheme.Background)
                .SetFont(RainRiskTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in RainRiskTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        RainRiskTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(new CharacterConfiguration(c, configuration.Font, RainRiskTheme.GetColor(c), c.ToString()));
            }

            foreach (char c in "RAINRISKWAYPOINTHEADINGSTEPDISTANCESTARTNSEWLRF ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(new CharacterConfiguration(c, configuration.Font, RainRiskTheme.GetColor(c), c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}