namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_21___Springdroid_Adventure;

    public class Day21 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day21()
            : base(2019, 21, "Springdroid Adventure")
        {
        }

        public Day21(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new SpringdroidAdventure(this.Input[0]).Run()}";

        public string Gold() => $"{new SpringdroidAdventure(this.Input[0]).RunWihtInreasedSensor()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new SpringdroidAdventure(this.Input[0], renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new SpringdroidAdventure(this.Input[0], renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => SpringdroidAdventureTheme.ToAsciiConfiguration(this);
    }
}
