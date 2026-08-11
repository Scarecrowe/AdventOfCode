namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class TobogganTrajectoryTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 10, 8);
        public static readonly Color Open = Color.FromArgb(25, 45, 35);
        public static readonly Color Tree = Color.FromArgb(40, 150, 80);
        public static readonly Color Trail = Color.FromArgb(110, 220, 140);
        public static readonly Color Hit = Color.FromArgb(255, 80, 80);
        public static readonly Color Player = Color.FromArgb(255, 230, 90);
        public static readonly Color Text = Color.FromArgb(220, 245, 225);
        public static readonly Color Numbers = Color.FromArgb(130, 255, 180);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 24);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['#'] = '♣',
            ['O'] = '○',
            ['X'] = '✖',
            ['@'] = '☃',
            ['/'] = '│',
            ['-'] = '─'
        };

        public static Color GetColor(char c)
        {
            return c switch
            {
                ' ' => Background,
                '.' => Open,
                '#' => Tree,
                'O' => Trail,
                'X' => Hit,
                '@' => Player,
                >= '0' and <= '9' => Numbers,
                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(Background)
                .SetFont(Font)
                .SetFps(3);

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
                characters.Add(new CharacterConfiguration(c, configuration.Font, GetColor(c), c.ToString()));
            }

            foreach (char c in "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 /:-")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(new CharacterConfiguration(c, configuration.Font, GetColor(c), c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}