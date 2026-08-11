namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class SpringdroidAdventureTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 6, 10);
        public static readonly Color Hull = Color.FromArgb(80, 90, 105);
        public static readonly Color Ground = Color.FromArgb(95, 120, 95);
        public static readonly Color Hole = Color.FromArgb(8, 10, 16);
        public static readonly Color Droid = Color.FromArgb(255, 220, 90);
        public static readonly Color Script = Color.FromArgb(120, 220, 255);
        public static readonly Color Success = Color.FromArgb(120, 255, 170);
        public static readonly Color Damage = Color.FromArgb(255, 120, 90);
        public static readonly Color Text = Color.FromArgb(220, 235, 255);
        public static readonly Color Numbers = Color.FromArgb(180, 255, 220);
        public static readonly Color Prompt = Color.FromArgb(190, 150, 255);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 26);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['#'] = '█',
            ['.'] = '·',
            ['@'] = '◆',
            ['-'] = '─',
            ['='] = '═',
            ['|'] = '│',
            ['>'] = '›',
            [':'] = ':'
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

                '#' => Ground,
                '.' => Hole,
                '@' => Droid,

                '>' => Prompt,
                ':' => Prompt,

                '-' or '=' or '|' => Hull,

                >= '0' and <= '9' => Numbers,

                'O' or 'K' => Success,

                'W' or 'A' or 'L' or 'R' or 'U' or 'N'
                    or 'T' or 'J' or 'C' or 'D' or 'H' or 'B' => Script,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(SpringdroidAdventureTheme.Background)
                .SetFont(SpringdroidAdventureTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in SpringdroidAdventureTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        SpringdroidAdventureTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        SpringdroidAdventureTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.,!?;:'\"()[]{}+-*/_=<> ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            SpringdroidAdventureTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}