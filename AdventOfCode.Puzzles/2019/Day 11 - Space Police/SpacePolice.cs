namespace AdventOfCode.Puzzles._2019.Day_11___Space_Police
{
    using System.Text;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.IntCode;

    public class SpacePolice
    {
        public SpacePolice(string program)
        {
            this.Robot = new(program);
            this.Hull = new();
        }

        public Robot Robot { get; }

        public VectorDictionary<int, PaintColour> Hull { get; }

        public string Print()
        {
            StringBuilder result = new();
            result.AppendLine().AppendLine();

            long minY = this.Hull.Min(c => c.Key.Y);
            long maxY = this.Hull.Max(c => c.Key.Y);
            long minX = this.Hull.Min(c => c.Key.X);
            long maxX = this.Hull.Max(c => c.Key.X);

            for (long y = minY; y <= maxY; y++)
            {
                for (long x = minX; x <= maxX; x++)
                {
                    if (this.Hull.ContainsKey(new(x, y)))
                    {
                        result.Append(this.Hull[new(x, y)] == PaintColour.Black ? " " : "#");
                    }
                    else
                    {
                        result.Append(' ');
                    }
                }

                result.AppendLine();
            }

            result.AppendLine();

            return result.ToString();
        }

        public SpacePolice PaintHull(int x, int y, PaintColour initialColour)
        {
            this.Robot.Cpu.Reset();
            this.Hull.Clear();
            this.Robot.SetPosition(new(x, y));
            this.Hull.Add(new(x, y), initialColour);

            while (this.Robot.Cpu.State != IntcodeCpuState.Terminated)
            {
                bool visited = this.Hull.ContainsKey(this.Robot.Point);

                PaintColour colour = visited
                    ? this.Hull[this.Robot.Point]
                    : PaintColour.Black;

                this.Robot.Cpu.Input.Enqueue((int)colour);
                this.Robot.Cpu.Run();

                colour = (PaintColour)this.Robot.Cpu.Output.Dequeue();
                RobotTurn turn = (RobotTurn)this.Robot.Cpu.Output.Dequeue();

                if (visited)
                {
                    this.Hull[this.Robot.Point] = colour;
                }
                else
                {
                    this.Hull.Add(this.Robot.Point, colour);
                }

                this.Robot.Turn(turn);
            }

            return this;
        }

        public SpacePolice RegistrationNumber()
        {
            this.PaintHull(0, 1, PaintColour.White);
            this.Print();

            return this;
        }
    }
}
