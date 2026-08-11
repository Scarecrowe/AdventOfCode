namespace AdventOfCode.Puzzles._2019.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.Day_15___Oxygen_System;

    public class Day15 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day15()
            : base(2019, 15, "Oxygen System")
        {
        }

        public Day15(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver() => $"{new OxygenSystem(this.Input[0]).BuildMap().Distance}";

        public string Gold() => $"{new OxygenSystem(this.Input[0]).BuildMap().FillMap().Minutes}";

        public void SilverFrame(IFrameRenderer renderer)
            => new OxygenSystem(this.Input[0], renderer).RenderBuildMap();

        public void GoldFrame(IFrameRenderer renderer)
            => new OxygenSystem(this.Input[0], renderer).RenderFillMap();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => OxygenSystemTheme.ToAsciiConfiguration(this);
    }
}
