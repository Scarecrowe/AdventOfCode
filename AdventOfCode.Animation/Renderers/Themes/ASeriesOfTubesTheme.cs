namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class ASeriesOfTubesTheme
    {
        public static readonly Color Background = Color.FromArgb(2, 5, 10);
        public static readonly Color Tube = Color.FromArgb(80, 120, 155);
        public static readonly Color Junction = Color.FromArgb(130, 210, 255);
        public static readonly Color Trail = Color.FromArgb(70, 255, 175);
        public static readonly Color Player = Color.FromArgb(255, 220, 70);
        public static readonly Color Letter = Color.FromArgb(255, 135, 210);
        public static readonly Color Text = Color.FromArgb(220, 235, 255);
        public static readonly Color Numbers = Color.FromArgb(120, 255, 220);
        public static readonly Color Rule = Color.FromArgb(35, 70, 95);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['|'] = '│',
            ['-'] = '─',
            ['+'] = '╋',
            ['~'] = '░',
            ['@'] = '◆',
            ['─'] = '─',
            ['/'] = '/'
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
                '|' or '-' => Tube,
                '+' => Junction,
                '~' => Trail,
                '@' => Player,
                '─' => Rule,

                >= 'A' and <= 'Z' => Letter,
                >= '0' and <= '9' => Numbers,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(ASeriesOfTubesTheme.Background)
                .SetFont(ASeriesOfTubesTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in ASeriesOfTubesTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        ASeriesOfTubesTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = 'A'; c <= 'Z'; c++)
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            ASeriesOfTubesTheme.GetColor(c),
                            c.ToString()));
                }
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        ASeriesOfTubesTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "A SERIES OF TUBES LETTERS STEPS PACKET DELIVERED VIEW XY")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            ASeriesOfTubesTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
