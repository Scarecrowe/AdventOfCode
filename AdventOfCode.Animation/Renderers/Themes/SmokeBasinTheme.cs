namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class SmokeBasinTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 8, 12);
        public static readonly Color Ridge = Color.FromArgb(80, 88, 96);
        public static readonly Color Basin = Color.FromArgb(70, 210, 180);
        public static readonly Color TopBasin = Color.FromArgb(255, 170, 70);
        public static readonly Color Low = Color.FromArgb(120, 220, 255);
        public static readonly Color Current = Color.FromArgb(255, 245, 120);
        public static readonly Color Text = Color.FromArgb(225, 240, 245);
        public static readonly Color Numbers = Color.FromArgb(90, 150, 170);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['#'] = '█',
            ['~'] = '▒',
            ['▓'] = '▓',
            ['@'] = '◆',
            ['*'] = '✦'
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
                '#' => Ridge,
                '~' => Basin,
                '▓' => TopBasin,
                '@' => Current,
                '*' => Low,

                >= '0' and <= '8' => Numbers,
                '9' => Ridge,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(SmokeBasinTheme.Background)
                .SetFont(SmokeBasinTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in SmokeBasinTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        SmokeBasinTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        SmokeBasinTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "SMOKEBASINSCANNINGRISKLOWPOINTSFOUNDGROWINGCOMPLETEPRODUCTTHREELARGEST0123456789/ ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            SmokeBasinTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}