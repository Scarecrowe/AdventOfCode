namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class SubterraneanSustainabilityTheme
    {
        public static readonly Color Background = Color.FromArgb(5, 8, 6);
        public static readonly Color Soil = Color.FromArgb(60, 42, 28);
        public static readonly Color EmptyPot = Color.FromArgb(35, 45, 35);
        public static readonly Color Plant = Color.FromArgb(80, 235, 120);
        public static readonly Color Glow = Color.FromArgb(150, 255, 170);
        public static readonly Color Zero = Color.FromArgb(255, 220, 90);
        public static readonly Color Ruler = Color.FromArgb(110, 140, 110);
        public static readonly Color Text = Color.FromArgb(215, 235, 210);
        public static readonly Color Numbers = Color.FromArgb(120, 255, 210);
        public static readonly Color Warning = Color.FromArgb(255, 170, 80);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['#'] = '♣',
            ['.'] = '·',
            ['^'] = '░',
            ['|'] = '│',
            ['+'] = '┼',
            [':'] = '╎',
            ['-'] = '─',
            ['/'] = '/',
            ['0'] = '0'
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
                '#' => Plant,
                '.' => EmptyPot,
                '^' => Glow,
                '|' or '+' or ':' or '-' => Ruler,
                '0' => Zero,
                >= '1' and <= '9' => Numbers,
                '/' => Ruler,
                _ when "STABLEPROJECTINGSHIFT".Contains(c) => Warning,
                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(SubterraneanSustainabilityTheme.Background)
                .SetFont(SubterraneanSustainabilityTheme.Font)
                .SetFps(8);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in SubterraneanSustainabilityTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        SubterraneanSustainabilityTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            SubterraneanSustainabilityTheme.GetColor(c),
                            c.ToString()));
                }
            }

            foreach (char c in "SUBTERRANEANSUSTAINABILITYSILVERGOLDGENERATIONSUMWINDOWPLANTSSTABLESHAPEDETECTEDPROJECTINGTOSHIFTACTIVESPREADSCROLLINGVIEWPORTEMPTY")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            SubterraneanSustainabilityTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
