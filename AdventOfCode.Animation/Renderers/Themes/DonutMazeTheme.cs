namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class DonutMazeTheme
    {
        public static readonly Color Background = Color.FromArgb(3, 5, 12);
        public static readonly Color Wall = Color.FromArgb(70, 80, 100);
        public static readonly Color Passage = Color.FromArgb(25, 35, 50);
        public static readonly Color Portal = Color.FromArgb(120, 220, 255);
        public static readonly Color Trail = Color.FromArgb(80, 255, 180);
        public static readonly Color Player = Color.FromArgb(255, 220, 80);
        public static readonly Color Text = Color.FromArgb(220, 235, 255);
        public static readonly Color Numbers = Color.FromArgb(120, 255, 220);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['#'] = '█',
            ['.'] = '·',
            ['~'] = '▓',
            ['@'] = '◆',
            ['*'] = '✦',
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
                '.' => Passage,
                '~' => Trail,
                '@' => Player,
                '*' => Portal,

                >= '0' and <= '9' => Numbers,

                '-' or '/' => Portal,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(DonutMazeTheme.Background)
                .SetFont(DonutMazeTheme.Font)
                .SetFps(8);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in DonutMazeTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        DonutMazeTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        DonutMazeTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "DONUTMAZERECURSIVELEVELSTEPSZZREACHEDAT ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            DonutMazeTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}