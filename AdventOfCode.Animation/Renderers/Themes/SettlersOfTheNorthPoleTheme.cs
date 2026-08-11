namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class SettlersOfTheNorthPoleTheme
    {
        public static readonly Color Background = Color.FromArgb(6, 10, 8);
        public static readonly Color Open = Color.FromArgb(30, 38, 30);
        public static readonly Color Trees = Color.FromArgb(70, 210, 105);
        public static readonly Color Lumberyard = Color.FromArgb(185, 130, 80);
        public static readonly Color Text = Color.FromArgb(220, 238, 220);
        public static readonly Color Numbers = Color.FromArgb(170, 245, 190);
        public static readonly Color Status = Color.FromArgb(255, 220, 130);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['|'] = '♣',
            ['#'] = '█',
            ['-'] = '─',
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
                '.' => Open,
                '|' => Trees,
                '#' => Lumberyard,

                >= '0' and <= '9' => Numbers,

                '-' or '/' => Status,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(SettlersOfTheNorthPoleTheme.Background)
                .SetFont(SettlersOfTheNorthPoleTheme.Font)
                .SetFps(8);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in SettlersOfTheNorthPoleTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        SettlersOfTheNorthPoleTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        SettlersOfTheNorthPoleTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "SETLERSOFTHNPLMIUTBWYADVCKJG0123456789 ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            SettlersOfTheNorthPoleTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
