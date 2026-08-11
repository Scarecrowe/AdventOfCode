namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class ReindeerMazeTheme
    {
        public static readonly Color Background = Color.FromArgb(5, 8, 12);
        public static readonly Color Wall = Color.FromArgb(65, 72, 86);
        public static readonly Color Floor = Color.FromArgb(28, 36, 48);
        public static readonly Color Start = Color.FromArgb(120, 255, 180);
        public static readonly Color End = Color.FromArgb(255, 110, 130);
        public static readonly Color Trail = Color.FromArgb(80, 210, 255);
        public static readonly Color Seat = Color.FromArgb(255, 210, 90);
        public static readonly Color Reindeer = Color.FromArgb(255, 245, 210);
        public static readonly Color Text = Color.FromArgb(225, 235, 245);
        public static readonly Color Numbers = Color.FromArgb(150, 255, 220);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['#'] = '█',
            ['.'] = '·',
            ['S'] = 'S',
            ['E'] = 'E',
            ['~'] = '▓',
            ['O'] = '●',
            ['^'] = '▲',
            ['>'] = '▶',
            ['v'] = '▼',
            ['<'] = '◀',
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
                '#' => Wall,
                '.' => Floor,
                'S' => Start,
                'E' => End,
                '~' => Trail,
                'O' => Seat,
                '^' or '>' or 'v' or '<' => Reindeer,

                >= '0' and <= '9' => Numbers,

                '-' or '/' => Trail,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(ReindeerMazeTheme.Background)
                .SetFont(ReindeerMazeTheme.Font)
                .SetFps(10);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in ReindeerMazeTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        ReindeerMazeTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        ReindeerMazeTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "REINDEERMAZEBESTSCOREFINISHREACHEDALLSEATSFOUND TILES")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            ReindeerMazeTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
