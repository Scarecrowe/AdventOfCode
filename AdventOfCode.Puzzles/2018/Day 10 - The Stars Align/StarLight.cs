namespace AdventOfCode.Puzzles._2018.Day_10___The_Stars_Align
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class StarLight
    {
        public StarLight(string line)
        {
            int[] tokens = line
                .Replace("position=<")
                .Replace(" velocity=<", ",")
                .Replace(">")
                .Split(",")
                .ToInt();

            this.Location = new(tokens[0], tokens[1]);
            this.Velocity = new(tokens[2], tokens[3]);
        }

        public StarLight(StarLight light)
        {
            this.Location = light.Location;
            this.Velocity = light.Velocity;
        }

        public Vector<int> Location { get; private set; }

        public Vector<int> Velocity { get; }

        public void Move() => this.Location += this.Velocity;

        public new string ToString() => $"{this.Location} -> {this.Velocity}";
    }
}
