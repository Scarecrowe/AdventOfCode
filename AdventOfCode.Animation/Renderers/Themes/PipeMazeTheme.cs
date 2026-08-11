namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class PipeMazeTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 8, 12);
        public static readonly Color Pipe = Color.FromArgb(145, 165, 175);
        public static readonly Color Trail = Color.FromArgb(80, 255, 180);
        public static readonly Color Player = Color.FromArgb(255, 220, 80);
        public static readonly Color Inside = Color.FromArgb(255, 120, 180);
        public static readonly Color Outside = Color.FromArgb(40, 70, 95);
        public static readonly Color Text = Color.FromArgb(220, 235, 255);
        public static readonly Color Numbers = Color.FromArgb(120, 255, 220);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['|'] = '│',
            ['-'] = '─',
            ['L'] = '└',
            ['J'] = '┘',
            ['7'] = '┐',
            ['F'] = '┌',
            ['S'] = '◆',
            ['~'] = '▓',
            ['@'] = '●',
            ['I'] = '■',
            ['O'] = '·'
        };

        public static Color GetColor(char c)
        {
            return c switch
            {
                '~' => Trail,
                '@' => Player,
                'I' => Inside,
                'O' => Outside,
                '|' or '-' or 'L' or 'J' or '7' or 'F' or 'S' => Pipe,
                >= '0' and <= '9' => Numbers,
                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(Background)
                .SetFont(Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in CharacterMap)
            {
                characters.Add(new CharacterConfiguration(
                    pair.Key,
                    configuration.Font,
                    GetColor(pair.Key),
                    pair.Value.ToString()));
            }

            foreach (char c in "PIPEMAZELOOPFARTHESTSCANNINGOUTSIDECHECKEDENCLOSED0123456789/: ")
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