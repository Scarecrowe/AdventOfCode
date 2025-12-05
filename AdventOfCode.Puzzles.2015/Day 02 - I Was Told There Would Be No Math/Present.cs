namespace AdventOfCode.Puzzles._2015.Day_02___I_Was_Told_There_Would_Be_No_Math
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class Present
    {
        public Present(string dimension)
        {
            int[] units = dimension.Split("x").ToInt();

            this.Length = units[0];
            this.Width = units[1];
            this.Height = units[2];
        }

        public int Length { get; }

        public int Width { get; }

        public int Height { get; }

        public int WrappingPaper()
            => MathHelper.SurfaceArea(this.Width, this.Height, this.Length)
            + MathHelper.SmallestSurface(this.Width, this.Height, this.Length);

        public int Ribbon()
            => MathHelper.SmallestPerimeter(this.Width, this.Height, this.Length)
            + MathHelper.Volume(this.Width, this.Height, this.Length);
    }
}
