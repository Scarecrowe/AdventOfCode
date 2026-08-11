namespace AdventOfCode.Puzzles._2016.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2016.Day_13___A_Maze_of_Twisty_Little_Cubicles;

    public class Day13 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day13()
            : base(2016, 13, "A Maze of Twisty Little Cubicles")
        {
        }

        public Day13(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new AMazeOfTwistyLittleCubicles(this.Input, 31, 39).FindAllPaths().ShortestPath()}";

        public string Gold()
            => $"{new AMazeOfTwistyLittleCubicles(this.Input, 31, 39).FindAllPaths().UniqueLocations()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new AMazeOfTwistyLittleCubicles(this.Input, 31, 39, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new AMazeOfTwistyLittleCubicles(this.Input, 31, 39, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => TwistyLittleCubiclesTheme.ToAsciiConfiguration(this);
    }
}
