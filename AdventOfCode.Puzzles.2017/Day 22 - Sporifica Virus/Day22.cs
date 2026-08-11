namespace AdventOfCode.Puzzles._2017.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Configuration;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2017.Day_22___Sporifica_Virus;

    public class Day22 : Puzzle, IPuzzle
    {
        public Day22()
            : base(2017, 22, "Sporifica Virus")
        {
        }

        public Day22(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new SporificaVirus(this.Input).Run(10000).InfectedCount}";

        [Slow]
        public string Gold() => $"{new SporificaVirus(this.Input).Run(10000000, true).InfectedCount}";

        public void SilverFrame(IFrameRenderer renderer) => new SporificaVirus(this.Input, renderer).Run(10000);
        
        public void GoldFrame(IFrameRenderer renderer) => new SporificaVirus(this.Input, renderer).Run(10000000, true);
        
        public IAsciiRendererConfiguration AsciiConfiguration()
        {
            IAsciiRendererConfiguration config = new AsciiRendererConfiguration(this.DayTitle)
                .SetBackgroundColor(BluePrintTheme.Background);

            ////ICharactersConfiguration characters = new CharactersConfiguration
            ////{
            ////    new CharacterConfiguration('#', config.Font, BluePrintTheme.Wall),
            ////    new CharacterConfiguration(' ', config.Font, BluePrintTheme.Empty),
            ////    new CharacterConfiguration('.', config.Font, BluePrintTheme.Visited),
            ////    new CharacterConfiguration('0', config.Font, BluePrintTheme.Start),
            ////    new CharacterConfiguration('1', config.Font, BluePrintTheme.Target),
            ////    new CharacterConfiguration('2', config.Font, BluePrintTheme.Target),
            ////    new CharacterConfiguration('3', config.Font, BluePrintTheme.Target),
            ////    new CharacterConfiguration('4', config.Font, BluePrintTheme.Target),
            ////    new CharacterConfiguration('5', config.Font, BluePrintTheme.Target),
            ////    new CharacterConfiguration('6', config.Font, BluePrintTheme.Target),
            ////    new CharacterConfiguration('7', config.Font, BluePrintTheme.Target),
            ////    new CharacterConfiguration('O', config.Font, BluePrintTheme.Frontier)
            ////};

            ////config.SetCharacters(characters);

            return config;
        }
    }
}
