namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class TwoFactorAuthenticationTheme
    {
        public static readonly Color Background = Color.FromArgb(2, 8, 12);
        public static readonly Color PixelOff = Color.FromArgb(10, 28, 35);
        public static readonly Color PixelOn = Color.FromArgb(90, 255, 180);
        public static readonly Color Glow = Color.FromArgb(170, 255, 215);
        public static readonly Color Text = Color.FromArgb(220, 245, 240);
        public static readonly Color Numbers = Color.FromArgb(120, 220, 255);
        public static readonly Color Status = Color.FromArgb(255, 220, 120);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 14);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '░',
            ['#'] = '█',
            ['/'] = '│',
            ['-'] = '─',
            ['='] = '═'
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
                '.' => PixelOff,
                '#' => PixelOn,

                >= '0' and <= '9' => Numbers,

                '/' or '-' or '=' => Glow,
                'X' or 'Y' => Status,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(TwoFactorAuthenticationTheme.Background)
                .SetFont(TwoFactorAuthenticationTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in TwoFactorAuthenticationTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        TwoFactorAuthenticationTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        TwoFactorAuthenticationTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "TWO-FACTORAUTHENTICATIONPIXELSCODESTEPBOOTINGDISPLAYRECTROTATEROWCOLUMNBYSHIFTCOMPLETELITXY/=- ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            TwoFactorAuthenticationTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
