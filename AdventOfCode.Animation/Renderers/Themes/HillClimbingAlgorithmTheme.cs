namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class HillClimbingAlgorithmTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 8, 10);
        public static readonly Color Low = Color.FromArgb(35, 90, 70);
        public static readonly Color Mid = Color.FromArgb(120, 150, 80);
        public static readonly Color High = Color.FromArgb(190, 180, 130);
        public static readonly Color Trail = Color.FromArgb(80, 255, 180);
        public static readonly Color Player = Color.FromArgb(255, 230, 90);
        public static readonly Color Start = Color.FromArgb(120, 220, 255);
        public static readonly Color End = Color.FromArgb(255, 100, 120);
        public static readonly Color Text = Color.FromArgb(225, 240, 230);
        public static readonly Color Numbers = Color.FromArgb(120, 255, 220);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 10);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['@'] = '◆',
            ['~'] = '▓',
            ['S'] = '●',
            ['E'] = '▲'
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
                '@' => Player,
                '~' => Trail,
                'S' => Start,
                'E' => End,

                >= '0' and <= '9' => Numbers,
                >= 'a' and <= 'h' => Low,
                >= 'i' and <= 'q' => Mid,
                >= 'r' and <= 'z' => High,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(Background)
                .SetFont(Font)
                .SetFps(10);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = 'a'; c <= 'z'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        GetColor(c),
                        c.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "HILLCLIMBSCENICHIKESTEPS /")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
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