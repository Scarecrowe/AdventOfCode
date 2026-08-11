namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class BeverageBanditsTheme
    {
        public static readonly Color Background = Color.FromArgb(8, 7, 5);
        public static readonly Color Wall = Color.FromArgb(95, 78, 54);
        public static readonly Color Cavern = Color.FromArgb(35, 28, 20);
        public static readonly Color Elf = Color.FromArgb(105, 245, 165);
        public static readonly Color Goblin = Color.FromArgb(245, 95, 85);
        public static readonly Color Text = Color.FromArgb(235, 225, 205);
        public static readonly Color Numbers = Color.FromArgb(255, 210, 105);
        public static readonly Color Divider = Color.FromArgb(135, 115, 85);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['#'] = '█',
            ['.'] = '·',
            ['E'] = '♜',
            ['G'] = '♟',
            ['-'] = '─',
            ['/'] = '│',
            ['('] = '(',
            [')'] = ')',
            [','] = ',',
            [':'] = ':',
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
                '.' => Cavern,
                'E' => Elf,
                'G' => Goblin,

                >= '0' and <= '9' => Numbers,

                '-' or '/' => Divider,
                '(' or ')' or ',' or ':' => Divider,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(BeverageBanditsTheme.Background)
                .SetFont(BeverageBanditsTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in BeverageBanditsTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        BeverageBanditsTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        BeverageBanditsTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "BEVERAGEBANDITSTECHROUNDTURNHPINITIALPOSITIONSMOVESATTACKSKILLSWAITSFORELVESWINGOBLINSCOMBATENDSOUTCOME ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            BeverageBanditsTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
