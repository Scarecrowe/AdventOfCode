namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class ReservoirResearchTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 7, 12);
        public static readonly Color Sand = Color.FromArgb(34, 29, 21);
        public static readonly Color Clay = Color.FromArgb(154, 98, 54);
        public static readonly Color FlowingWater = Color.FromArgb(75, 175, 255);
        public static readonly Color SettledWater = Color.FromArgb(35, 115, 215);
        public static readonly Color Spring = Color.FromArgb(160, 240, 255);
        public static readonly Color Frontier = Color.FromArgb(255, 235, 120);
        public static readonly Color Text = Color.FromArgb(222, 238, 255);
        public static readonly Color Numbers = Color.FromArgb(125, 245, 255);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 9);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['#'] = '█',
            ['|'] = '│',
            ['~'] = '▓',
            ['+'] = '✦',
            ['@'] = '◆',
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
                '.' => Sand,
                '#' => Clay,
                '|' => FlowingWater,
                '~' => SettledWater,
                '+' => Spring,
                '@' => Frontier,

                >= '0' and <= '9' => Numbers,

                '/' => FlowingWater,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(ReservoirResearchTheme.Background)
                .SetFont(ReservoirResearchTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in ReservoirResearchTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        ReservoirResearchTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        ReservoirResearchTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "RESERVOIRARCHSCANWATERSTREAMREACHEDRETAINEDFURTHESTMARKEDx ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            ReservoirResearchTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
