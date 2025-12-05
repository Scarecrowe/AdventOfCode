namespace AdventOfCode.Puzzles._2021.Day_22___Reactor_Reboot
{
    using AdventOfCode.Core;

    public class Cube
    {
        public Cube(Vector<long> min, Vector<long> max)
        {
            this.Min = min;
            this.Max = max;
        }

        public Cube(Instruction instruction)
        {
            this.Min = instruction.Min;
            this.Max = instruction.Max;
        }

        public Vector<long> Min { get; }

        public Vector<long> Max { get; }

        public long Volume() => (this.Max.X - this.Min.X + 1) * (this.Max.Y - this.Min.Y + 1) * (this.Max.Z - this.Min.Z + 1);

        public bool Intersects(Cube cube) =>
            (this.Min.X <= cube.Max.X && this.Max.X >= cube.Min.X)
            && (this.Min.Y <= cube.Max.Y && this.Max.Y >= cube.Min.Y)
            && (this.Min.Z <= cube.Max.Z && this.Max.Z >= cube.Min.Z);

        public Cube Overlaps(Cube cube)
        {
            return new Cube(
               new(Math.Max(this.Min.X, cube.Min.X), Math.Max(this.Min.Y, cube.Min.Y), Math.Max(this.Min.Z, cube.Min.Z)),
               new(Math.Min(this.Max.X, cube.Max.X), Math.Min(this.Max.Y, cube.Max.Y), Math.Min(this.Max.Z, cube.Max.Z)));
        }
    }
}
