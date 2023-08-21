namespace AdventOfCode.Puzzles._2017.Day_20___Particle_Swarm
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class Particle
    {
        public Particle(string line, int number)
        {
            this.Number = number;

            string[] tokens = line.Split(", ");
            this.Position = new(tokens[0].Replace("p=<").Replace(">").SplitComma().ToInt());
            this.Velocity = new(tokens[1].Replace("v=<").Replace(">").SplitComma().ToInt());
            this.Acceleration = new(tokens[2].Replace("a=<").Replace(">").SplitComma().ToInt());
        }

        public int Number { get; }

        public Vector<int> Position { get; private set; }

        public Vector<int> Velocity { get; private set; }

        public Vector<int> Acceleration { get; private set; }

        public void Move()
        {
            this.Velocity += this.Acceleration;
            this.Position += this.Velocity;
        }

        public void Print()
        {
            PuzzleConsole.Write($"p=<{this.Position.X}, {this.Position.Y},{this.Position.Z}>, ");
            PuzzleConsole.Write($"v=<{this.Velocity.X}, {this.Velocity.Y},{this.Velocity.Z}>, ");
            PuzzleConsole.WriteLine($"a=<{this.Acceleration.X}, {this.Acceleration.Y},{this.Acceleration.Z}> ---> {this.Distance()}");
        }

        public int Distance() => this.Position.Absolute();
    }
}
