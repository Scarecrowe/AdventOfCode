namespace AdventOfCode.Animation.Renderers.Themes
{
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class SpaceImageFormatTheme
    {
        public static readonly Color Background = Color.FromArgb(2, 6, 14);

        public static readonly Color BlackPixel = Color.FromArgb(8, 12, 20);
        public static readonly Color WhitePixel = Color.FromArgb(180, 255, 220);
        public static readonly Color TransparentPixel = Color.FromArgb(30, 60, 90);

        public static readonly Color Header = Color.FromArgb(120, 220, 255);
        public static readonly Color Noise = Color.FromArgb(80, 100, 130);
        public static readonly Color Text = Color.FromArgb(210, 240, 255);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");

        public static readonly Font Font = new Font(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            ['0'] = ' ',
            ['1'] = '█',
            ['2'] = '░',
            ['#'] = '█',
            ['·'] = '░',
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
                '0' or ' ' => BlackPixel,
                '1' or '#' or '█' => WhitePixel,
                '2' or '·' or '░' => TransparentPixel,
                '-' or '─' => Header,
                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(SpaceImageFormatTheme.Background)
                .SetFont(SpaceImageFormatTheme.Font)
                .SetFps(6);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in SpaceImageFormatTheme.CharacterMap)
            {
                characters.Add(new CharacterConfiguration(pair.Key, configuration.Font, SpaceImageFormatTheme.GetColor(pair.Key), pair.Value.ToString()));
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
