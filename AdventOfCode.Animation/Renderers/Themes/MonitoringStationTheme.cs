namespace AdventOfCode.Animation.Renderers.Themes
{
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class MonitoringStationTheme
    {
        public static readonly Color Background = Color.FromArgb(2, 4, 10);
        public static readonly Color Empty = Color.FromArgb(10, 14, 24);

        public static readonly Color Asteroid = Color.FromArgb(130, 130, 145);
        public static readonly Color Station = Color.FromArgb(120, 220, 255);

        public static readonly Color Laser = Color.FromArgb(255, 70, 70);
        public static readonly Color Explosion = Color.FromArgb(255, 220, 80);

        public static readonly Color Target200 = Color.FromArgb(120, 255, 120);
        public static readonly Color Text = Color.FromArgb(220, 235, 255);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");

        public static readonly Font Font = new Font(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            ['.'] = '·',
            ['#'] = '●',
            ['S'] = '◉',
            ['*'] = '✸'
        };

        public static char GetCharacter(char c)
        {
            return CharacterMap.TryGetValue(c, out char replacement)
                ? replacement : c;
        }

        public static Color GetColor(char c)
        {
            return c switch
            {
                '#' => Asteroid,
                'S' => Station,
                '*' => Explosion,
                '2' => Target200,
                '.' => Empty,
                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(MonitoringStationTheme.Background)
                .SetFont(MonitoringStationTheme.Font)
                .SetFps(6);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in MonitoringStationTheme.CharacterMap)
            {
                characters.Add(new CharacterConfiguration(pair.Key, configuration.Font, MonitoringStationTheme.GetColor(pair.Key), pair.Value.ToString()));
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
