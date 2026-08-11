namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class ModeMazeTheme
    {
        public static readonly Color Background = Color.FromArgb(4, 7, 12);
        public static readonly Color Rocky = Color.FromArgb(90, 96, 106);
        public static readonly Color Wet = Color.FromArgb(40, 120, 170);
        public static readonly Color Narrow = Color.FromArgb(170, 120, 60);
        public static readonly Color Mouth = Color.FromArgb(180, 240, 255);
        public static readonly Color Target = Color.FromArgb(255, 90, 120);
        public static readonly Color Trail = Color.FromArgb(110, 255, 170);
        public static readonly Color Scan = Color.FromArgb(255, 215, 100);
        public static readonly Color Torch = Color.FromArgb(255, 225, 90);
        public static readonly Color Gear = Color.FromArgb(170, 220, 255);
        public static readonly Color Neither = Color.FromArgb(220, 170, 255);
        public static readonly Color Text = Color.FromArgb(225, 235, 245);
        public static readonly Color Numbers = Color.FromArgb(130, 255, 220);
        public static readonly Color Divider = Color.FromArgb(70, 90, 120);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");
        public static readonly Font Font = new(FontFamily, 12);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            [' '] = ' ',
            ['.'] = '·',
            ['='] = '≈',
            ['|'] = '│',
            ['M'] = 'M',
            ['X'] = '◆',
            ['T'] = '♨',
            ['C'] = '▲',
            ['N'] = '●',
            ['*'] = '▓',
            ['+'] = '▒',
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
                '.' => Rocky,
                '=' => Wet,
                '|' => Narrow,
                'M' => Mouth,
                'X' => Target,
                'T' => Torch,
                'C' => Gear,
                'N' => Neither,
                '*' => Trail,
                '+' => Scan,
                '-' or '/' => Divider,

                >= '0' and <= '9' => Numbers,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
                .SetBackgroundColor(ModeMazeTheme.Background)
                .SetFont(ModeMazeTheme.Font)
                .SetFps(12);

            ICharactersConfiguration characters = new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in ModeMazeTheme.CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        ModeMazeTheme.GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            for (char c = '0'; c <= '9'; c++)
            {
                characters.Add(
                    new CharacterConfiguration(
                        c,
                        configuration.Font,
                        ModeMazeTheme.GetColor(c),
                        c.ToString()));
            }

            foreach (char c in "MODEMAZERISKSCANRESCUEROUTETIME TOOLTORCHGEARNEITHERSTEPSSWITCHESMOVESTARGETDEPTH")
            {
                if (!characters.Any(x => x.Character == c))
                {
                    characters.Add(
                        new CharacterConfiguration(
                            c,
                            configuration.Font,
                            ModeMazeTheme.GetColor(c),
                            c.ToString()));
                }
            }

            configuration.SetCharacters(characters);

            return configuration;
        }
    }
}
