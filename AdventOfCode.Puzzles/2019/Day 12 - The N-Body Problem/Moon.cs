namespace AdventOfCode.Puzzles._2019.Day_12___The_N_Body_Problem
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class Moon
    {
        public Moon(string line)
        {
            int[] values = line
                .Replace("<")
                .Replace(">")
                .Split(", ")
                .Select(c => c.Split("=")[1].ToInt())
                .ToArray();

            this.Location = new(values);
            this.Velocity = new(0, 0, 0);
        }

        public Moon(Moon moon)
        {
            this.Location = new(moon.Location);
            this.Velocity = new(moon.Velocity);
        }

        public Vector<int> Location { get; set; }

        public Vector<int> Velocity { get; }

        public int PotentialEnergy() => this.Location.Absolute();

        public int KineticEnergy() => this.Velocity.Absolute();
    }
}
