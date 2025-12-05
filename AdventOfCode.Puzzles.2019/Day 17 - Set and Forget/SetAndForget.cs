namespace AdventOfCode.Puzzles._2019.Day_17___Set_and_Forget
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using AdventOfCode.Puzzles._2019.IntCode;

    public class SetAndForget
    {
        public SetAndForget(string program)
        {
            this.Cpu = new(program);
            this.Map = new();
            this.Intersections = new();
            this.Path = new();
            this.CompressedPaths = new();
            this.Movements = string.Empty;
        }

        public long AlignmentParameter { get; private set; }

        public long DustCollected { get; private set; }

        private IntcodeCpu Cpu { get; }

        private VectorDictionary<int, Entity> Map { get; }

        private HashSet<Vector<int>> Intersections { get; }

        private List<string> Path { get; }

        private List<string> CompressedPaths { get; }

        private string Movements { get; set; }

        public SetAndForget BuildMap(int mode = 1)
        {
            this.Cpu.Memory.Write(0, mode);
            this.Cpu.Run();

            int x = 0;
            int y = 0;

            while (this.Cpu.Output.Count > 0)
            {
                long value = this.Cpu.Output.Dequeue();

                if (value == 10)
                {
                    x = 0;
                    y++;
                    continue;
                }

                this.Map.Add(new(x, y), (Entity)value);

                x++;
            }

            this.BuildIntersections();

            return this;
        }

        public SetAndForget BuildIntersections()
        {
            foreach (KeyValuePair<Vector<int>, Entity> pair in this.Map.Where(c => c.Value == Entity.Scaffold))
            {
                Dictionary<Cardinal, VectorCell<int, Entity>> adjacent = this.Map.AdjacentCardinal(pair.Key)
                    .ToDictionary(c => c.Direction, c => c);

                if (adjacent.Count < 4)
                {
                    continue;
                }

                if (adjacent[Cardinal.North].Value == Entity.Scaffold
                    && adjacent[Cardinal.South].Value == Entity.Scaffold
                    && adjacent[Cardinal.East].Value == Entity.Scaffold
                    && adjacent[Cardinal.West].Value == Entity.Scaffold)
                {
                    this.Intersections.Add(pair.Key);
                    this.AlignmentParameter += pair.Key.X * pair.Key.Y;
                }
            }

            return this;
        }

        public SetAndForget PrintMap()
        {
            long minY = this.Map.Min(c => c.Key.Y);
            long maxY = this.Map.Max(c => c.Key.Y);
            long minX = this.Map.Min(c => c.Key.X);
            long maxX = this.Map.Max(c => c.Key.X);

            Dictionary<Entity, string> display = new()
            {
                { Entity.Scaffold, "#" },
                { Entity.OpenSpace, "." },
                { Entity.RobotUp, "^" },
                { Entity.RobotDown, "v" },
                { Entity.RobotLeft, "<" },
                { Entity.RobotRight, ">" },
                { Entity.RobotTumbling, "X" },
            };

            for (long y = minY; y <= maxY; y++)
            {
                for (long x = minX; x <= maxX; x++)
                {
                    if (this.Map.ContainsKey(new(x, y)))
                    {
                        PuzzleConsole.Write(display[this.Map[new(x, y)]]);
                    }
                    else
                    {
                        PuzzleConsole.Write("#");
                    }
                }

                PuzzleConsole.WriteLine();
            }

            return this;
        }

        public SetAndForget BuildPath()
        {
            Robot robot = this.GetRobot();
            HashSet<Vector<int>> visited = new() { robot.Location };

            while (true)
            {
                Dictionary<Cardinal, VectorCell<int, Entity>> adjacent = this.Map.AdjacentCardinal(robot.Location).ToDictionary(c => c.Direction, c => c);

                if (!adjacent.Any(c => c.Value.Value == Entity.Scaffold))
                {
                    break;
                }

                if (!adjacent.ContainsKey(robot.Direction) || adjacent[robot.Direction].Value == Entity.OpenSpace)
                {
                    this.Path.Add(robot.Turn(adjacent.FirstOrDefault(c => c.Value.Value == Entity.Scaffold
                        && !visited.Contains(c.Value.Point)).Value.Direction));
                }
                else
                {
                    int moves = 0;

                    while (adjacent.ContainsKey(robot.Direction)
                        && adjacent[robot.Direction].Value == Entity.Scaffold)
                    {
                        robot.Location = new(adjacent[robot.Direction].Point);

                        if (!this.Intersections.Contains(robot.Location))
                        {
                            this.Map[robot.Location] = Entity.OpenSpace;
                            visited.Add(robot.Location);
                        }

                        adjacent = this.Map.AdjacentCardinal(robot.Location).ToDictionary(c => c.Direction, c => c);
                        moves++;
                    }

                    this.Path.Add($"{moves}");
                }
            }

            return this;
        }

        public SetAndForget CompressPath()
        {
            string path = this.Path.Join(",");

            for (int aLen = 10; aLen >= 2; aLen -= 2)
            {
                string aPath = this.Path.GetRange(0, aLen).Join(",");

                if (aPath.Length > 20)
                {
                    continue;
                }

                int nextIndexA = aPath.Length + 1;
                int aCount = 1;

                while (path.Substring(nextIndexA, aPath.Length) == aPath)
                {
                    nextIndexA += aPath.Length + 1;
                    aCount++;
                }

                for (int bLen = 10; bLen >= 2; bLen -= 2)
                {
                    string bPath = this.Path.GetRange(aLen * aCount, bLen).Join(",");

                    if (bPath.Length > 20)
                    {
                        continue;
                    }

                    int nextIndexB = nextIndexA + bPath.Length + 1;
                    int bCount = 1;
                    int abCount = 0;
                    bool match = true;

                    while (match)
                    {
                        match = false;
                        if (path.Substring(nextIndexB, aPath.Length) == aPath)
                        {
                            match = true;
                            nextIndexB += aPath.Length + 1;
                            abCount++;
                        }

                        if (path.Substring(nextIndexB, bPath.Length) == bPath)
                        {
                            match = true;
                            nextIndexB += bPath.Length + 1;
                            bCount++;
                        }
                    }

                    for (int cLen = 10; cLen >= 2; cLen -= 2)
                    {
                        string cPath = this.Path.GetRange((aLen * (aCount + abCount)) + (bLen * bCount), cLen).Join(",");

                        if (cPath.Length > 20)
                        {
                            continue;
                        }

                        string? result = ConstructPaths(aPath, bPath, cPath, path);

                        if (result != null)
                        {
                            this.CompressedPaths.Add(aPath);
                            this.CompressedPaths.Add(bPath);
                            this.CompressedPaths.Add(cPath);
                            this.Movements = result;
                            return this;
                        }
                    }
                }
            }

            throw new InvalidOperationException();
        }

        public SetAndForget FindRobots()
        {
            this.BuildPath().CompressPath();

            this.Cpu.Run();
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii(this.Movements));
            this.Cpu.Run();
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii(this.CompressedPaths[0]));
            this.Cpu.Run();
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii(this.CompressedPaths[1]));
            this.Cpu.Run();
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii(this.CompressedPaths[2]));
            this.Cpu.Run();
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("n"));
            this.Cpu.Output.Clear();
            this.Cpu.Run();

            while (this.Cpu.Output.Count > 0)
            {
                long value = this.Cpu.Output.Dequeue();

                if (value > 255)
                {
                    this.DustCollected = value;
                    break;
                }
            }

            return this;
        }

        private static string? ConstructPaths(string a, string b, string c, string path)
        {
            int index = 0;

            List<string> movements = new();

            while (index < path.Length)
            {
                if (index + a.Length <= path.Length && path.Substring(index, a.Length) == a)
                {
                    index += a.Length + 1;
                    movements.Add("A");
                    continue;
                }
                else if (index + b.Length <= path.Length && path.Substring(index, b.Length) == b)
                {
                    index += b.Length + 1;
                    movements.Add("B");
                    continue;
                }
                else if (index + c.Length <= path.Length && path.Substring(index, c.Length) == c)
                {
                    index += c.Length + 1;
                    movements.Add("C");
                    continue;
                }
                else
                {
                    return null;
                }
            }

            string result = string.Join(",", movements);

            return result.Length <= 20 ? result : null;
        }

        private Robot GetRobot()
        {
            KeyValuePair<Vector<int>, Entity> robotPair = this.Map.FirstOrDefault(c => c.Value != Entity.Scaffold && c.Value != Entity.OpenSpace);

            return new(robotPair.Key, robotPair.Value);
        }
    }
}
