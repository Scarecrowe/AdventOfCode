namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class JurassicJigsawTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 8, 10);
        public static readonly Color Water = Color.FromArgb(20, 45, 55);
        public static readonly Color Wave = Color.FromArgb(80, 180, 190);
        public static readonly Color Monster = Color.FromArgb(120, 255, 120);
        public static readonly Color Text = Color.FromArgb(220, 245, 235);
        public static readonly Color Numbers = Color.FromArgb(255, 220, 120);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 8);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['#'] = '█',
            ['O'] = '▓'
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
                '.' => Water,
                '#' => Wave,
                'O' => Monster,

                >= '0' and <= '9' => Numbers,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(JurassicJigsawTheme.Background)
                .SetFont(JurassicJigsawTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in JurassicJigsawTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        JurassicJigsawTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        JurassicJigsawTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "JURASSICJIGSAWSTARTTILEPLACEDIMAGEASSEMBLEDCORNERPRODUCTFULLWITHBORDERSREMOVEDTRUEIMAGEDATASCANNINGFORSEAMONSTERSFOUNDWATERROUGHNESS ")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            JurassicJigsawTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}