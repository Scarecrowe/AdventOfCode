namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class TwistyLittleCubiclesTheme
    {
        public static readonly Color Background = Color.FromArgb(5, 6, 10);
        public static readonly Color Wall = Color.FromArgb(55, 62, 78);
        public static readonly Color Open = Color.FromArgb(24, 31, 42);
        public static readonly Color Trail = Color.FromArgb(70, 230, 165);
        public static readonly Color Search = Color.FromArgb(80, 140, 255);
        public static readonly Color Player = Color.FromArgb(255, 220, 85);
        public static readonly Color Start = Color.FromArgb(120, 255, 210);
        public static readonly Color Target = Color.FromArgb(255, 105, 120);
        public static readonly Color Text = Color.FromArgb(220, 235, 255);
        public static readonly Color Numbers = Color.FromArgb(145, 255, 225);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['#'] = '█',
            ['.'] = '·',
            ['~'] = '▓',
            ['o'] = '░',
            ['@'] = '◆',
            ['S'] = '◎',
            ['X'] = '✕',
            [','] = ',',
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
                '.' => Open,
                '~' => Trail,
                'o' => Search,
                '@' => Player,
                'S' => Start,
                'X' => Target,

                >= '0' and <= '9' => Numbers,

                ',' or '/' => Text,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(TwistyLittleCubiclesTheme.Background)
                .SetFont(TwistyLittleCubiclesTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in TwistyLittleCubiclesTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        TwistyLittleCubiclesTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        TwistyLittleCubiclesTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "AMZEOFTWISTYLITLECUBRGSNPH//ARGETREACHEDFWSCOUNQULIM ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            TwistyLittleCubiclesTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
