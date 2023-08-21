namespace AdventOfCode.Puzzles._2019.Day_12___The_N_Body_Problem
{
    using AdventOfCode.Core;

    public class TheNBodyProblem
    {
        public TheNBodyProblem(string[] input) => this.Moons = Parse(input);

        public List<Moon> Moons { get; }

        public long Simulate(int steps = -1)
        {
            int step = 0;
            Vector<long> point = new(-1, -1, -1);

            while (true)
            {
                this.ApplyGravity();
                this.ApplyVelocity();

                step++;

                if (steps > -1 && steps == step)
                {
                    return this.TotalEnergy();
                }

                if (point.X == -1 && this.Moons.All(c => c.Velocity.X == 0))
                {
                    point.X = step;
                }

                if (point.Y == -1 && this.Moons.All(c => c.Velocity.Y == 0))
                {
                    point.Y = step;
                }

                if (point.Z == -1 && this.Moons.All(c => c.Velocity.Z == 0))
                {
                    point.Z = step;
                }

                if (point.X > -1 && point.Y > -1 && point.Z > -1)
                {
                    return MathHelper.LeastCommonMultiple(point.ToList3D()) * 2;
                }
            }

            throw new InvalidOperationException();
        }

        private static List<Moon> Parse(string[] input) => input.Select(x => new Moon(x)).ToList();

        private void PrintStep(int step)
        {
            PuzzleConsole.WriteLine($"After {step}{(step != 1 ? "s" : string.Empty)}");

            foreach (Moon moon in this.Moons)
            {
                PuzzleConsole.WriteLine($"pos=<x={moon.Location.X}, y={moon.Location.Y}, z={moon.Location.Z}>, vel=<x={moon.Velocity.X}, y={moon.Velocity.Y}, z={moon.Velocity.Z}>");
            }

            PuzzleConsole.WriteLine();
        }

        private void ApplyGravity()
        {
            foreach (Moon moonA in this.Moons)
            {
                foreach (Moon moonB in this.Moons)
                {
                    if (moonA != moonB)
                    {
                        if (moonA.Location.X > moonB.Location.X)
                        {
                            moonA.Velocity.X--;
                            moonB.Velocity.X++;
                        }
                        else if (moonB.Location.X < moonA.Location.X)
                        {
                            moonB.Velocity.X--;
                            moonA.Velocity.X++;
                        }

                        if (moonA.Location.Y > moonB.Location.Y)
                        {
                            moonA.Velocity.Y--;
                            moonB.Velocity.Y++;
                        }
                        else if (moonB.Location.Y < moonA.Location.Y)
                        {
                            moonB.Velocity.Y--;
                            moonA.Velocity.Y++;
                        }

                        if (moonA.Location.Z > moonB.Location.Z)
                        {
                            moonA.Velocity.Z--;
                            moonB.Velocity.Z++;
                        }
                        else if (moonB.Location.Z < moonA.Location.Z)
                        {
                            moonB.Velocity.Z--;
                            moonA.Velocity.Z++;
                        }
                    }
                }
            }
        }

        private void ApplyVelocity() => this.Moons.ForEach(x => x.Location += x.Velocity);

        private int TotalEnergy() => this.Moons.Sum(c => c.PotentialEnergy() * c.KineticEnergy());
    }
}
