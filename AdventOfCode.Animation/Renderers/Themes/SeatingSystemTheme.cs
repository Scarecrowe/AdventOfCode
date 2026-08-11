namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class SeatingSystemTheme
    {
        public static readonly Color Background = Color.FromArgb(5, 8, 14);
        public static readonly Color Floor = Color.FromArgb(30, 34, 42);
        public static readonly Color EmptySeat = Color.FromArgb(80, 150, 255);
        public static readonly Color OccupiedSeat = Color.FromArgb(255, 210, 90);
        public static readonly Color Text = Color.FromArgb(225, 235, 255);
        public static readonly Color Numbers = Color.FromArgb(120, 255, 220);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['L'] = '□',
            ['#'] = '■',
            ['/'] = '│'
        };

        public static Color GetColor(char c)
        {
            return c switch
            {
                '.' => Floor,
                'L' => EmptySeat,
                '#' => OccupiedSeat,
                >= '0' and <= '9' => Numbers,
                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(Background)
                .SetFont(Font)
                .SetFps(8);

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
                characters.Add(new CharacterConfiguration(
                    c,
                    configuration.Font,
                    GetColor(c),
                    c.ToString()));
            }

            foreach (char c in "ADJACENTVISIBLESEATINGSYSTEMROUNDSTABLEOCCUPIED ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(new CharacterConfiguration(
                        c,
                        configuration.Font,
                        GetColor(c),
                        c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}