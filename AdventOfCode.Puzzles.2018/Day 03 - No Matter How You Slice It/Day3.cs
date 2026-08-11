namespace AdventOfCode.Puzzles._2018.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_03___No_Matter_How_You_Slice_It;
    using AdventOfCode.Animation.Renderers.Themes;

    public class Day3 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day3()
            : base(2018, 3, "No Matter How You Slice It")
        {
        }

        public Day3(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new NoMatterHowYouSliceIt(this.Input).PlotClaims().OverlappingClaims()}";

        public string Gold() => $"{new NoMatterHowYouSliceIt(this.Input).PlotClaims().NonOverlappingClaim()}";

        public void SilverFrame(IFrameRenderer renderer) => new NoMatterHowYouSliceIt(this.Input, renderer).PlotClaims();

        public void GoldFrame(IFrameRenderer renderer) => new NoMatterHowYouSliceIt(this.Input, renderer).PlotClaims();

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
