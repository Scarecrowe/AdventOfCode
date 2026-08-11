namespace AdventOfCode.Puzzles._2019.Day_11___Space_Police
{
    using System.Text;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.IntCode;

    public class SpacePolice
    {
        public SpacePolice(string program)
        {
            this.Robot = new(program);
            this.Hull = new();
        }

        public SpacePolice(string program, IFrameRenderer renderer)
            : this(program)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        public Robot Robot { get; }

        public VectorDictionary<int, PaintColour> Hull { get; }

        private sealed record HullSnapshot(
            Dictionary<Vector<int>, PaintColour> Hull,
            Vector<int> RobotPoint);

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

        public void RenderPaintHull(
            int x,
            int y,
            PaintColour initialColour,
            int renderEvery = 25)
        {
            if (this.Renderer == null)
            {
                return;
            }

            List<HullSnapshot> snapshots = this.GetPaintSnapshots(
                x,
                y,
                initialColour,
                renderEvery);

            int padding = 2;

            HullSnapshot final = snapshots.Last();

            List<Vector<int>> whitePanels = final.Hull
                .Where(x => x.Value == PaintColour.White)
                .Select(x => x.Key)
                .ToList();

            int minX = whitePanels.Min(p => p.X) - padding;
            int maxX = whitePanels.Max(p => p.X) + padding;
            int minY = whitePanels.Min(p => p.Y) - padding;
            int maxY = whitePanels.Max(p => p.Y) + padding;

            foreach (HullSnapshot snapshot in snapshots)
            {
                this.Renderer.RenderFrame(new Frame(this.BuildFrame(snapshot, minX, maxX, minY, maxY)));
            }

            this.Renderer.RenderFrame(new Frame(this.BuildFinalFrame(final, minX, maxX, minY, maxY)));
        }


        public SpacePolice RegistrationNumber()
        {
            this.PaintHull(0, 1, PaintColour.White);
            this.Print();

            return this;
        }

        private void RenderFrame()
        {
            if (this.Renderer == null || this.Hull.Count == 0)
            {
                return;
            }

            string[] frame = this.BuildFrame();

            this.Renderer.RenderFrame(new Frame(frame));
        }

        private string[] BuildFrame()
        {
            int padding = 4;

            long minY = Math.Min(this.Hull.Min(c => c.Key.Y), this.Robot.Point.Y) - padding;
            long maxY = Math.Max(this.Hull.Max(c => c.Key.Y), this.Robot.Point.Y) + padding;
            long minX = Math.Min(this.Hull.Min(c => c.Key.X), this.Robot.Point.X) - padding;
            long maxX = Math.Max(this.Hull.Max(c => c.Key.X), this.Robot.Point.X) + padding;

            List<string> result = [];

            for (long y = minY; y <= maxY; y++)
            {
                StringBuilder sb = new();

                for (long x = minX; x <= maxX; x++)
                {
                    Vector<int> point = new((int)x, (int)y);

                    if (point == this.Robot.Point)
                    {
                        sb.Append(GetRobotCharacter());
                    }
                    else if (this.Hull.TryGetValue(point, out PaintColour colour))
                    {
                        sb.Append(colour == PaintColour.White ? '#' : '.');
                    }
                    else
                    {
                        sb.Append(' ');
                    }
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private char GetRobotCharacter()
        {
            return this.Robot.Direction switch
            {
                Cardinal.North => '^',
                Cardinal.South => 'v',
                Cardinal.West => '<',
                Cardinal.East => '>',
                _ => '@'
            };
        }

        private List<HullSnapshot> GetPaintSnapshots(
            int x,
            int y,
            PaintColour initialColour,
            int renderEvery)
        {
            this.Robot.Cpu.Reset();
            this.Hull.Clear();

            this.Robot.SetPosition(new(x, y));
            this.Hull.Add(new(x, y), initialColour);

            List<HullSnapshot> snapshots = [];

            int step = 0;

            snapshots.Add(this.CreateSnapshot());

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

                step++;

                if (step % renderEvery == 0)
                {
                    snapshots.Add(this.CreateSnapshot());
                }
            }

            snapshots.Add(this.CreateSnapshot());

            return snapshots;
        }

        private HullSnapshot CreateSnapshot()
        {
            return new HullSnapshot(
                this.Hull.ToDictionary(x => x.Key, x => x.Value),
                this.Robot.Point);
        }

        private string[] BuildFrame(
            HullSnapshot snapshot,
            int minX,
            int maxX,
            int minY,
            int maxY)
        {
            List<string> result = [];

            for (int y = minY; y <= maxY; y++)
            {
                StringBuilder sb = new();

                for (int x = minX; x <= maxX; x++)
                {
                    Vector<int> point = new(x, y);

                    if (point == snapshot.RobotPoint)
                    {
                        sb.Append('@');
                    }
                    else if (snapshot.Hull.TryGetValue(point, out PaintColour colour))
                    {
                        sb.Append(colour == PaintColour.White ? '#' : ' ');
                    }
                    else
                    {
                        sb.Append(' ');
                    }
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private string[] BuildFinalFrame(
            HullSnapshot snapshot,
            int minX,
            int maxX,
            int minY,
            int maxY)
        {
            List<string> result = [];

            for (int y = minY; y <= maxY; y++)
            {
                StringBuilder sb = new();

                for (int x = minX; x <= maxX; x++)
                {
                    Vector<int> point = new(x, y);

                    if (snapshot.Hull.TryGetValue(point, out PaintColour colour))
                    {
                        sb.Append(colour == PaintColour.White ? '#' : ' ');
                    }
                    else
                    {
                        sb.Append(' ');
                    }
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }
    }
}
