namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class LaboratoriesTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 6, 18);
        public static readonly Color Empty = Color.FromArgb(18, 24, 40);
        public static readonly Color Start = Color.FromArgb(255, 230, 90);
        public static readonly Color Splitter = Color.FromArgb(255, 110, 160);
        public static readonly Color HitSplitter = Color.FromArgb(255, 210, 90);
        public static readonly Color Beam = Color.FromArgb(80, 230, 255);
        public static readonly Color Active = Color.FromArgb(255, 255, 255);
        public static readonly Color TimelineLow = Color.FromArgb(120, 255, 180);
        public static readonly Color TimelineHigh = Color.FromArgb(180, 130, 255);
        public static readonly Color Text = Color.FromArgb(220, 235, 255);
        public static readonly Color Numbers = Color.FromArgb(120, 255, 220);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 10);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['S'] = '◆',
            ['^'] = '▲',
            ['|'] = '│',
            ['@'] = '●',
            ['*'] = '✦',
            ['+'] = '✚',
            ['H'] = 'H',
            ['K'] = 'K',
            ['M'] = 'M',
            ['B'] = 'B',
            ['Z'] = 'Z',
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
                '.' => Empty,
                'S' => Start,
                '^' => Splitter,
                '*' => HitSplitter,
                '|' => Beam,
                '@' => Active,
                '+' or 'H' or 'K' => TimelineLow,
                'M' or 'B' or 'Z' => TimelineHigh,

                >= '0' and <= '9' => Numbers,

                '-' or '/' => Splitter,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(LaboratoriesTheme.Background)
                .SetFont(LaboratoriesTheme.Font)
                .SetFps(10);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in LaboratoriesTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        LaboratoriesTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        LaboratoriesTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "LABORATORIESBEAMSPLITTERSSPLITSACTIVEEXITEDREPAIRDATAREADYMANYWORLDSTIMELINESQUANTUMMANIFOLDCOMPLETE ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            LaboratoriesTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
