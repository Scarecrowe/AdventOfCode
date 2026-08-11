using AdventOfCode.Animation.Renderers.AsciiRenderer;
using AdventOfCode.Animation.Renderers.Configuration;
using AdventOfCode.Core;

public static class RegolithReservoirTheme
{
    public static readonly Color Background = Color.FromArgb(6, 5, 4);
    public static readonly Color Air = Color.FromArgb(24, 20, 18);
    public static readonly Color Rock = Color.FromArgb(105, 95, 85);
    public static readonly Color Sand = Color.FromArgb(235, 185, 95);
    public static readonly Color FallingSand = Color.FromArgb(255, 230, 130);
    public static readonly Color Trail = Color.FromArgb(150, 110, 60);
    public static readonly Color Source = Color.FromArgb(255, 120, 80);
    public static readonly Color Text = Color.FromArgb(235, 220, 190);
    public static readonly Color Numbers = Color.FromArgb(255, 200, 120);

    public static readonly FontFamily FontFamily = new("Cascadia Mono");
    public static readonly Font Font = new(FontFamily, 12);

    public static readonly Dictionary<char, char> CharacterMap = new()
    {
        [' '] = ' ',
        ['.'] = '·',
        ['#'] = '█',
        ['o'] = '●',
        ['@'] = '◆',
        ['~'] = '░',
        ['+'] = '✚',
        ['/'] = '│',
        ['-'] = '─'
    };

    public static Color GetColor(char c) => c switch
    {
        '.' => Air,
        '#' => Rock,
        'o' => Sand,
        '@' => FallingSand,
        '~' => Trail,
        '+' => Source,
        >= '0' and <= '9' => Numbers,
        _ => Text
    };

    public static IAsciiRendererConfiguration ToAsciiConfiguration(IPuzzle puzzle)
    {
        IAsciiRendererConfiguration configuration = new AsciiRendererConfiguration(puzzle.DayTitle)
            .SetBackgroundColor(Background)
            .SetFont(Font)
            .SetFps(24);

        ICharactersConfiguration characters = new CharactersConfiguration();

        foreach (KeyValuePair<char, char> pair in CharacterMap)
        {
            characters.Add(new CharacterConfiguration(
                pair.Key,
                configuration.Font,
                GetColor(pair.Key),
                pair.Value.ToString()));
        }

        foreach (char c in "REGOLITHRESERVOIRFLOORABYSSSANDFALLSINTO THE RESTED0123456789")
        {
            if (!characters.Any(x => x.Character == c))
            {
                characters.Add(new CharacterConfiguration(
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