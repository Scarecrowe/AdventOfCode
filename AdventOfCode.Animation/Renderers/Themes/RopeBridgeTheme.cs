namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class RopeBridgeTheme
    {
        public static readonly Color Background = Color.FromArgb(5, 8, 14);
        public static readonly Color Empty = Color.FromArgb(18, 24, 34);
        public static readonly Color Trail = Color.FromArgb(80, 180, 255);
        public static readonly Color Start = Color.FromArgb(255, 120, 120);
        public static readonly Color Head = Color.FromArgb(255, 230, 80);
        public static readonly Color Tail = Color.FromArgb(120, 255, 170);
        public static readonly Color Knot = Color.FromArgb(190, 150, 255);
        public static readonly Color Text = Color.FromArgb(220, 235, 255);
        public static readonly Color Number = Color.FromArgb(120, 255, 220);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['#'] = '▓',
            ['s'] = '◆',
            ['H'] = '●',
            ['T'] = '●'
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
                '.' => Empty,
                '#' => Trail,
                's' => Start,
                'H' => Head,
                'T' => Tail,
                >= '1' and <= '9' => Knot,
                >= '0' and <= '9' => Number,
                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(RopeBridgeTheme.Background)
                .SetFont(RopeBridgeTheme.Font)
                .SetFps(6);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in RopeBridgeTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        RopeBridgeTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        RopeBridgeTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "ROPEBRIDGEKNOTSSTEPVISITEDNORTHSOUTHWESTEAST ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            RopeBridgeTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}