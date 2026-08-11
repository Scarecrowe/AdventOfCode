namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class LikeAGIFForYourYardTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 6, 12);
        public static readonly Color Off = Color.FromArgb(18, 24, 38);
        public static readonly Color On = Color.FromArgb(255, 236, 135);
        public static readonly Color Corner = Color.FromArgb(255, 120, 80);
        public static readonly Color Text = Color.FromArgb(220, 235, 255);
        public static readonly Color Numbers = Color.FromArgb(130, 255, 210);
        public static readonly Color Slash = Color.FromArgb(110, 160, 220);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 10);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['#'] = '█',
            ['*'] = '◆',
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
                '.' => Off,
                '#' => On,
                '*' => Corner,

                >= '0' and <= '9' => Numbers,

                '/' => Slash,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(LikeAGIFForYourYardTheme.Background)
                .SetFont(LikeAGIFForYourYardTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in LikeAGIFForYourYardTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        LikeAGIFForYourYardTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        LikeAGIFForYourYardTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "LIKEAGIFFORYURDSTCKNESCOMPLETEPON ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            LikeAGIFForYourYardTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
