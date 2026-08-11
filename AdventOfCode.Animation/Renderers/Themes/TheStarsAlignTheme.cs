namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class TheStarsAlignTheme
    {
        public static readonly Color Background = Color.FromArgb(2, 4, 14);
        public static readonly Color Star = Color.FromArgb(255, 245, 185);
        public static readonly Color Text = Color.FromArgb(205, 225, 255);
        public static readonly Color Numbers = Color.FromArgb(120, 255, 220);
        public static readonly Color Symbols = Color.FromArgb(120, 170, 255);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['#'] = '✦',
            ['/'] = '│',
            ['-'] = '─',
            ['+'] = '+',
            ['x'] = '×'
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
                '#' => Star,

                >= '0' and <= '9' => Numbers,

                '/' or '-' or '+' or 'x' => Symbols,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(TheStarsAlignTheme.Background)
                .SetFont(TheStarsAlignTheme.Font)
                .SetFps(10);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in TheStarsAlignTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        TheStarsAlignTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        TheStarsAlignTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "THESTARSALIGNMESSAGEAPPEARSLOCKEDSECONDTSKYWINDOWLIGHTS ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            TheStarsAlignTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
