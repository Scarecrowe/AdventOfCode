namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class DumboOctopusTheme
    {
        public static readonly Color Background = Color.FromArgb(2, 6, 14);
        public static readonly Color Text = Color.FromArgb(220, 240, 255);
        public static readonly Color Flash = Color.FromArgb(255, 245, 130);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 16);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['*'] = '✹'
        };

        public static char GetCharacter(char c)
            => CharacterMap.TryGetValue(c, out char replacement)
                ? replacement
                : c;

        public static Color GetColor(char c)
        {
            return c switch
            {
                '*' => Flash,

                '0' => Color.FromArgb(25, 35, 55),
                '1' => Color.FromArgb(40, 70, 100),
                '2' => Color.FromArgb(50, 95, 130),
                '3' => Color.FromArgb(65, 120, 160),
                '4' => Color.FromArgb(80, 145, 185),
                '5' => Color.FromArgb(95, 170, 205),
                '6' => Color.FromArgb(120, 200, 220),
                '7' => Color.FromArgb(160, 225, 225),
                '8' => Color.FromArgb(210, 240, 210),
                '9' => Color.FromArgb(255, 230, 160),

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(DumboOctopusTheme.Background)
                .SetFont(DumboOctopusTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in DumboOctopusTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        DumboOctopusTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        DumboOctopusTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "DUMBOCTOPUSBEFREANYSTPSFLAHGINCRD")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            DumboOctopusTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}