namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class ProbablyAFireHazardTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 6, 10);
        public static readonly Color Dim = Color.FromArgb(35, 45, 60);
        public static readonly Color Low = Color.FromArgb(80, 120, 170);
        public static readonly Color Medium = Color.FromArgb(100, 190, 230);
        public static readonly Color High = Color.FromArgb(255, 220, 110);
        public static readonly Color Hot = Color.FromArgb(255, 130, 80);
        public static readonly Color Active = Color.FromArgb(255, 255, 190);
        public static readonly Color Text = Color.FromArgb(220, 235, 255);
        public static readonly Color Numbers = Color.FromArgb(130, 255, 220);
        public static readonly Color Border = Color.FromArgb(100, 150, 200);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 8);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '░',
            [':'] = '▒',
            ['*'] = '▓',
            ['O'] = '◉',
            ['#'] = '█',
            ['@'] = '■',
            ['-'] = '─',
            ['/'] = '│',
            ['\''] = '\''
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
                '.' => Low,
                ':' => Medium,
                '*' => High,
                'O' => Hot,
                '#' => High,
                '@' => Active,

                '-' or '/' => Border,

                >= '0' and <= '9' => Numbers,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(ProbablyAFireHazardTheme.Background)
                .SetFont(ProbablyAFireHazardTheme.Font)
                .SetFps(10);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in ProbablyAFireHazardTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        ProbablyAFireHazardTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        ProbablyAFireHazardTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "PROBABLYAFIREHAZARDSILVERGOLDCOMPLETEINSTRUCTIONVISUALTOTALMAXWAITINGFORSANTASINSTRUCTIONSTURNONOFFTOGGLETHROUGH EACHCHARACTERREPRESENTSXBLOCKOFLIGHTS, ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            ProbablyAFireHazardTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
