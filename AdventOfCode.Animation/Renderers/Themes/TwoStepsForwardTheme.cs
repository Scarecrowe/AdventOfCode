namespace AdventOfCode.Animation.Renderers.Themes
{
    using System.Drawing;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Core;

    public static class TwoStepsForwardTheme
    {
        public static readonly Color Background = Color.FromArgb(5, 8, 14);

        public static readonly Color Wall = Color.FromArgb(70, 90, 110);

        public static readonly Color Room = Color.FromArgb(25, 35, 50);

        public static readonly Color OpenDoor = Color.FromArgb(120, 255, 180);

        public static readonly Color Trail = Color.FromArgb(80, 220, 255);

        public static readonly Color Player = Color.FromArgb(255, 220, 90);

        public static readonly Color Vault = Color.FromArgb(255, 120, 120);

        public static readonly Color Text = Color.FromArgb(220, 235, 255);

        public static readonly Color Hash = Color.FromArgb(255, 160, 80);

        public static readonly FontFamily FontFamily = new("Cascadia Mono");

        public static readonly Font Font = new(FontFamily, 16);

        public static readonly Dictionary<char, char> CharacterMap = new()
        {
            ['#'] = '█',
            ['.'] = '·',
            ['+'] = '▒',
            ['~'] = '▓',
            ['@'] = '◆',
            ['V'] = '◉'
        };

        public static Color GetColor(char c)
        {
            return c switch
            {
                '#' => Wall,
                '.' => Room,
                '+' => OpenDoor,
                '~' => Trail,
                '@' => Player,
                'V' => Vault,

                >= '0' and <= '9' => Hash,
                >= 'a' and <= 'f' => Hash,

                _ => Text
            };
        }

        public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
        {
            IAsciiRendererConfiguration configuration =
                new AsciiRendererConfiguration(puzzle.DayTitle)
                    .SetBackgroundColor(Background)
                    .SetFont(Font)
                    .SetFps(10);

            ICharactersConfiguration characters =
                new CharactersConfiguration();

            foreach (KeyValuePair<char, char> pair in CharacterMap)
            {
                characters.Add(
                    new CharacterConfiguration(
                        pair.Key,
                        configuration.Font,
                        GetColor(pair.Key),
                        pair.Value.ToString()));
            }

            foreach (char c in
                "TWOSTEPSFORWARDGOLDLENGTHPATHHASH abcdef0123456789")
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