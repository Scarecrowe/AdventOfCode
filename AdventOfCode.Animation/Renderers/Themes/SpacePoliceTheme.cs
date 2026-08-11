namespace AdventOfCode.Animation.Renderers.Themes
{
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class SpacePoliceTheme
    {
        public static readonly Color Background = Color.FromArgb(3, 6, 12);

        public static readonly Color BlackPanel = Color.FromArgb(10, 14, 22);
        public static readonly Color WhitePanel = Color.FromArgb(230, 240, 255);

        public static readonly Color Robot = Color.FromArgb(255, 200, 80);
        public static readonly Color RobotGlow = Color.FromArgb(120, 255, 200, 80);

        public static readonly Color Grid = Color.FromArgb(25, 35, 50);
        public static readonly Color Text = Color.FromArgb(210, 230, 255);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");

        public static readonly Font Font = new Font(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            ['#'] = '█',
            ['.'] = '·',
            ['^'] = '▲',
            ['v'] = '▼',
            ['<'] = '◄',
            ['>'] = '►',
            ['@'] = '◆',
            [' '] = ' '
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
                '#' => WhitePanel,
                '.' => BlackPanel,
                '^' or 'v' or '<' or '>' or '@' => Robot,
                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(SpacePoliceTheme.Background)
                .SetFont(SpacePoliceTheme.Font)
                .SetFps(6);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in SpacePoliceTheme.CharacterMap)
            {
                characters.Add(new CharacterConfiguration(pair.Key, configuration.Font, SpacePoliceTheme.GetColor(pair.Key), pair.Value.ToString()));
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
