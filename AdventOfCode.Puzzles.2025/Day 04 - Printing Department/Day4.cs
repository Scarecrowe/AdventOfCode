namespace AdventOfCode.Puzzles._2025.Days
{
    using AdventOfCode.Animation;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Animation.Renderers.AsciiRenderer;
    using AdventOfCode.Animation.Renderers.Themes;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2025.Day_04___Printing_Department;

    public class Day4 : Puzzle, IPuzzle, IAsciiAnimation
    {
        public Day4()
            : base(2025, 4, "Printing Department")
        {
        }

        public Day4(string[] input)
            : this()
        {
            this.Input = input;
        }

        public string Silver()
            => $"{new PrintingDepartment(this.Input).AccessableRolls()}";

        public string Gold()
            => $"{new PrintingDepartment(this.Input).AllAccessableRolls()}";

        public void SilverFrame(IFrameRenderer renderer)
            => new PrintingDepartment(this.Input, renderer).RenderSilver();

        public void GoldFrame(IFrameRenderer renderer)
            => new PrintingDepartment(this.Input, renderer).RenderGold();

        public IAsciiRendererConfiguration AsciiConfiguration()
            => PrintingDepartmentTheme.ToAsciiConfiguration(this);
    }
}
