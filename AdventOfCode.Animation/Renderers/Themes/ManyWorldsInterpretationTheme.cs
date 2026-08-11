namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class ManyWorldsInterpretationTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 5, 10);

        public static readonly Color Wall = Color.FromArgb(80, 85, 100);
        public static readonly Color Floor = Color.FromArgb(20, 25, 35);
        public static readonly Color Entrance = Color.FromArgb(120, 220, 255);
        public static readonly Color Robot = Color.FromArgb(255, 220, 80);
        public static readonly Color Trail = Color.FromArgb(80, 255, 180);
        public static readonly Color Key = Color.FromArgb(255, 180, 60);
        public static readonly Color Door = Color.FromArgb(255, 80, 100);

        public static readonly Color Text = Color.FromArgb(220, 235, 255);
        public static readonly Color Numbers = Color.FromArgb(120, 255, 220);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");

        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['#'] = '█',
            ['.'] = '·',
            ['+'] = '◉',
            ['@'] = '◆',
            ['~'] = '▓',
            ['$'] = '♦',
            ['%'] = '▣',
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
                '+' => Entrance,
                '@' => Robot,
                '~' => Trail,
                '$' => Key,
                '%' => Door,

                >= '0' and <= '9' => Numbers,

                '-' or '/' => Wall,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(ManyWorldsInterpretationTheme.Background)
                .SetFont(ManyWorldsInterpretationTheme.Font)
                .SetFps(8);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in ManyWorldsInterpretationTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        ManyWorldsInterpretationTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        ManyWorldsInterpretationTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "COLLECTINGKEYSSTEPSALLKEYSCOLLECTEDROBOTVAULTCLEARED TOTAL")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            ManyWorldsInterpretationTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}