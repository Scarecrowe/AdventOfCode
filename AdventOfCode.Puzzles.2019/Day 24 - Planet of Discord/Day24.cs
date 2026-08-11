namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_24___Planet_of_Discord;

    public class Day24 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day24()
            : base(2019, 24, "Planet of Discord")
        {
        }

        public Day24(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new PlanetOfDiscord(this.Input).BiodiversityRating()}";

        public string Gold() => $"{new PlanetOfDiscord(this.Input).BugCount()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new PlanetOfDiscord(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new PlanetOfDiscord(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => PlanetOfDiscordTheme.ToAsciiConfiguration(this);
    }
}
