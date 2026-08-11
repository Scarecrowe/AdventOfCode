namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class ALongWalkTheme
    {
        public static readonly Color Background = Color.FromArgb(5, 8, 10);
        public static readonly Color Forest = Color.FromArgb(20, 55, 34);
        public static readonly Color Path = Color.FromArgb(55, 68, 58);
        public static readonly Color Slope = Color.FromArgb(120, 180, 155);
        public static readonly Color Trail = Color.FromArgb(255, 190, 85);
        public static readonly Color Hiker = Color.FromArgb(255, 245, 180);
        public static readonly Color Start = Color.FromArgb(120, 255, 190);
        public static readonly Color End = Color.FromArgb(255, 110, 110);
        public static readonly Color Text = Color.FromArgb(220, 238, 225);
        public static readonly Color Numbers = Color.FromArgb(150, 220, 255);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['#'] = '█',
            ['.'] = '·',
            ['^'] = '▲',
            ['v'] = '▼',
            ['<'] = '◀',
            ['>'] = '▶',
            ['O'] = '▓',
            ['@'] = '◆',
            ['S'] = 'S',
            ['E'] = 'E'
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
                '#' => Forest,
                '.' => Path,
                '^' or 'v' or '<' or '>' => Slope,
                'O' => Trail,
                '@' => Hiker,
                'S' => Start,
                'E' => End,

                >= '0' and <= '9' => Numbers,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(ALongWalkTheme.Background)
                .SetFont(ALongWalkTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in ALongWalkTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        ALongWalkTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        ALongWalkTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "ALONGWALKICYSLOPESDRYTRAILSTEPSHIKEWINDOWRC-")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            ALongWalkTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
