namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class LavaductLagoonTheme
    {
        public static readonly Color Background = Color.FromArgb(8, 4, 3);
        public static readonly Color Ground = Color.FromArgb(34, 28, 22);
        public static readonly Color Trench = Color.FromArgb(255, 125, 50);
        public static readonly Color Lava = Color.FromArgb(255, 70, 20);
        public static readonly Color Digger = Color.FromArgb(255, 230, 90);
        public static readonly Color Header = Color.FromArgb(255, 220, 170);
        public static readonly Color Numbers = Color.FromArgb(255, 170, 80);
        public static readonly Color Divider = Color.FromArgb(115, 70, 45);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['~'] = '▓',
            ['@'] = '◆',
            ['-'] = '─',

            ['0'] = '█',
            ['1'] = '█',
            ['2'] = '█',
            ['3'] = '█',
            ['4'] = '█',
            ['5'] = '█',
            ['6'] = '█',
            ['7'] = '█',
            ['8'] = '█',
            ['9'] = '█',
            ['A'] = '█',
            ['B'] = '█',
            ['C'] = '█',
            ['D'] = '█',
            ['E'] = '█',
            ['F'] = '█'
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
                '.' => Ground,
                '~' => Lava,
                '@' => Digger,
                '-' => Divider,

                '0' => Color.FromArgb(180, 60, 40),
                '1' => Color.FromArgb(210, 70, 40),
                '2' => Color.FromArgb(235, 80, 35),
                '3' => Color.FromArgb(255, 95, 35),
                '4' => Color.FromArgb(255, 115, 40),
                '5' => Color.FromArgb(255, 135, 45),
                '6' => Color.FromArgb(255, 155, 50),
                '7' => Color.FromArgb(255, 175, 60),
                '8' => Color.FromArgb(255, 195, 75),
                '9' => Color.FromArgb(255, 215, 95),
                'A' => Color.FromArgb(255, 85, 80),
                'B' => Color.FromArgb(255, 105, 95),
                'C' => Color.FromArgb(255, 130, 105),
                'D' => Color.FromArgb(255, 155, 115),
                'E' => Color.FromArgb(255, 180, 130),
                'F' => Color.FromArgb(255, 205, 150),

                _ => Header
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(LavaductLagoonTheme.Background)
                .SetFont(LavaductLagoonTheme.Font)
                .SetFps(10);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in LavaductLagoonTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        LavaductLagoonTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            LavaductLagoonTheme.GetColor(c),
                            c.ToString()));
                }
            }

            foreach (char c in "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz/: ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            LavaductLagoonTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
