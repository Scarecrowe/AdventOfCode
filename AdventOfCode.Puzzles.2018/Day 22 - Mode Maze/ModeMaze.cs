namespace AdventOfCode.Puzzles._2018.Day_22___Mode_Maze
{
    using System.Text;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class ModeMaze
    {
        private const int ViewportWidth = 48;
        private const int ViewportHeight = 32;
        private const int HoldFrames = 24;

        public ModeMaze(string[] input, long xOffset, long yOffset)
        {
            this.Moves = new();
            this.Depth = input[0].Replace("depth: ").ToInt();
            this.Target = new(input[1].Replace("target: ").Split(",").ToLong());
            this.Offset = new(xOffset, yOffset);
            this.Map = new(this.Target.X + xOffset + 1, this.Target.Y + yOffset + 1);
            this.BuildMoves();
        }

        public ModeMaze(
            string[] input,
            long xOffset,
            long yOffset,
            IFrameRenderer renderer)
            : this(input, xOffset, yOffset)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        public int Depth { get; }

        public Vector<long> Target { get; }

        public Vector<long> Offset { get; }

        public VectorArray<long, MazeEntity> Map { get; }

        private Dictionary<MazeEntityType, Dictionary<MazeEntityType, Dictionary<MazeToolType, List<MazeMove>>>> Moves { get; set; }

        private readonly record struct PathKey(Vector<long> Point, MazeToolType Tool);

        public ModeMaze BuildMap()
        {
            for (int y = 0; y <= this.Target.Y + this.Offset.Y; y++)
            {
                for (int x = 0; x <= this.Target.X + this.Offset.X; x++)
                {
                    this.Map[y, x] = this.GetEntity(x, y);
                }
            }

            return this;
        }

        public long CountOfWetAndNarrow()
        {
            long result = 0;
            for (int y = 0; y <= this.Target.Y; y++)
            {
                for (int x = 0; x <= this.Target.X; x++)
                {
                    result += (int)this.Map[y, x].Region;
                }
            }

            return result;
        }

        public long WalkMaze()
        {
            long result = long.MaxValue;
            Queue<MazeState> queue = new();
            queue.Enqueue(new(new(), MazeToolType.Torch, 0, -1));
            queue.Enqueue(new(new(), MazeToolType.ClimbingGear, 1, -1));
            Dictionary<Vector<long>, Dictionary<MazeToolType, long>> visited = new();

            while (queue.Count > 0)
            {
                MazeState walk = queue.Dequeue();

                Vector<long> point = walk.Point.Clone();
                MazeEntity entity = this.Map[point];

                long score = walk.Score();

                if (score >= result)
                {
                    continue;
                }

                if (point == this.Target)
                {
                    if (entity.Region == MazeEntityType.Rocky && walk.Tool == MazeToolType.ClimbingGear)
                    {
                        score = ((walk.SwitchCount + 1) * 7) + walk.MoveCount;
                    }

                    result = Math.Min(score, result);
                    continue;
                }

                foreach (VectorCell<long, MazeEntity> adjacent in this.Map.AdjacentCardinal(walk.Point))
                {
                    List<MazeMove> moves = this.Moves[entity.Region][adjacent.Value.Region][walk.Tool];
                    visited.TryGetValue(adjacent.Point, out var visitedPoint);

                    foreach (MazeMove move in moves)
                    {
                        long switchCount = walk.SwitchCount + move.SwitchCount;
                        long walkCount = walk.MoveCount;
                        long adjacentScore = (switchCount * 7) + walk.MoveCount + 1;

                        if (visitedPoint != null)
                        {
                            if (visitedPoint.ContainsKey(move.Tool))
                            {
                                if (adjacentScore >= visitedPoint[move.Tool])
                                {
                                    continue;
                                }

                                visitedPoint[move.Tool] = adjacentScore;
                            }
                            else
                            {
                                visitedPoint.Add(move.Tool, adjacentScore);
                            }
                        }
                        else
                        {
                            visited.Add(adjacent.Point, new() { { move.Tool, adjacentScore } });
                            visitedPoint = visited[adjacent.Point];
                        }

                        queue.Enqueue(new(new(adjacent.Point), move.Tool, switchCount, walkCount));
                    }
                }
            }

            return result;
        }

        public ModeMaze RenderSilver()
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<string[]> frames = [];
            HashSet<Vector<long>> scanned = [];

            long risk = 0;

            for (long y = 0; y <= this.Target.Y; y++)
            {
                for (long x = 0; x <= this.Target.X; x++)
                {
                    Vector<long> point = new(x, y);
                    scanned.Add(point);
                    risk += (int)this.Map[y, x].Region;

                    if ((x + y) % 3 == 0 || point == this.Target)
                    {
                        frames.Add(this.BuildRiskFrame(point, scanned, risk));
                    }
                }
            }

            for (int i = 0; i < HoldFrames; i++)
            {
                frames.Add(this.BuildRiskFrame(this.Target, scanned, risk));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        public ModeMaze RenderGold(int renderEvery = 1)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<MazeState> path = this.WalkMazePath().ToList();

            if (path.Count == 0)
            {
                return this;
            }

            List<string[]> frames = [];
            HashSet<Vector<long>> trail = [];

            for (int i = 0; i < path.Count; i++)
            {
                MazeState state = path[i];
                trail.Add(state.Point);

                if (i % renderEvery == 0 || i == path.Count - 1)
                {
                    frames.Add(this.BuildPathFrame(state, trail, i, path.Count));
                }
            }

            MazeState last = path.Last();

            for (int i = 0; i < HoldFrames; i++)
            {
                frames.Add(this.BuildPathFrame(last, trail, path.Count, path.Count));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        public void Render()
        {
            this.RenderGold();
        }

        private IEnumerable<MazeState> WalkMazePath()
        {
            long result = long.MaxValue;

            Queue<MazeState> queue = new();
            queue.Enqueue(new(new(), MazeToolType.Torch, 0, -1));
            queue.Enqueue(new(new(), MazeToolType.ClimbingGear, 1, -1));

            Dictionary<PathKey, long> visited = new();
            Dictionary<PathKey, PathKey> cameFrom = new();
            Dictionary<PathKey, MazeState> states = new();

            PathKey startTorch = new(new(), MazeToolType.Torch);
            PathKey startGear = new(new(), MazeToolType.ClimbingGear);

            visited[startTorch] = 0;
            visited[startGear] = 7;
            states[startTorch] = new(new(), MazeToolType.Torch, 0, -1);
            states[startGear] = new(new(), MazeToolType.ClimbingGear, 1, -1);

            PathKey? bestTarget = null;

            while (queue.Count > 0)
            {
                MazeState walk = queue.Dequeue();

                Vector<long> point = walk.Point.Clone();
                MazeEntity entity = this.Map[point];

                long score = walk.Score();

                if (score >= result)
                {
                    continue;
                }

                if (point == this.Target)
                {
                    MazeState final = walk;

                    if (walk.Tool != MazeToolType.Torch)
                    {
                        final = new(walk.Point, MazeToolType.Torch, walk.SwitchCount + 1, walk.MoveCount);
                        score = final.Score();
                    }

                    if (score < result)
                    {
                        result = score;
                        bestTarget = new(final.Point, final.Tool);

                        PathKey oldKey = new(walk.Point, walk.Tool);
                        PathKey finalKey = new(final.Point, final.Tool);

                        states[finalKey] = final;

                        if (oldKey != finalKey)
                        {
                            cameFrom[finalKey] = oldKey;
                        }
                    }

                    continue;
                }

                foreach (VectorCell<long, MazeEntity> adjacent in this.Map.AdjacentCardinal(walk.Point))
                {
                    List<MazeMove> moves = this.Moves[entity.Region][adjacent.Value.Region][walk.Tool];

                    foreach (MazeMove move in moves)
                    {
                        long switchCount = walk.SwitchCount + move.SwitchCount;
                        long moveCount = walk.MoveCount + 1;

                        MazeState next = new(new(adjacent.Point), move.Tool, switchCount, moveCount);
                        long nextScore = next.Score();

                        PathKey currentKey = new(walk.Point, walk.Tool);
                        PathKey nextKey = new(next.Point, next.Tool);

                        if (visited.TryGetValue(nextKey, out long previousScore) &&
                            nextScore >= previousScore)
                        {
                            continue;
                        }

                        visited[nextKey] = nextScore;
                        cameFrom[nextKey] = currentKey;
                        states[nextKey] = next;

                        queue.Enqueue(next);
                    }
                }
            }

            if (bestTarget == null)
            {
                yield break;
            }

            foreach (MazeState state in ReconstructPath(cameFrom, states, bestTarget.Value))
            {
                yield return state;
            }
        }

        private string[] BuildRiskFrame(
            Vector<long> current,
            HashSet<Vector<long>> scanned,
            long risk)
        {
            string title = $"MODE MAZE // RISK SCAN // RISK {risk:00000}";
            return this.BuildFrame(
                current,
                MazeToolType.Torch,
                scanned,
                [],
                title,
                footer: $"TARGET {this.Target.X},{this.Target.Y} // DEPTH {this.Depth}");
        }

        private string[] BuildPathFrame(
            MazeState state,
            HashSet<Vector<long>> trail,
            int step,
            int totalSteps)
        {
            string title = $"MODE MAZE // RESCUE ROUTE // TIME {state.Score():0000} // TOOL {ToolName(state.Tool)}";
            string footer = $"STEP {step:0000}/{totalSteps:0000} // SWITCHES {state.SwitchCount:000} // MOVES {state.MoveCount:0000}";

            return this.BuildFrame(
                state.Point,
                state.Tool,
                [],
                trail,
                title,
                footer);
        }

        private string[] BuildFrame(
            Vector<long> current,
            MazeToolType tool,
            HashSet<Vector<long>> scanned,
            HashSet<Vector<long>> trail,
            string title,
            string footer)
        {
            Dictionary<MazeEntityType, char> display = new()
            {
                { MazeEntityType.Rocky, '.' },
                { MazeEntityType.Wet, '=' },
                { MazeEntityType.Narrow, '|' },
            };

            long mapWidth = this.Target.X + this.Offset.X + 1;
            long mapHeight = this.Target.Y + this.Offset.Y + 1;

            long startX = current.X - (ViewportWidth / 2);
            long startY = current.Y - (ViewportHeight / 2);

            startX = Math.Max(0, Math.Min(startX, Math.Max(0, mapWidth - ViewportWidth)));
            startY = Math.Max(0, Math.Min(startY, Math.Max(0, mapHeight - ViewportHeight)));

            List<string> result = [];
            result.Add(title);
            result.Add(new string('-', ViewportWidth));

            StringBuilder sb = new(ViewportWidth);

            for (long y = startY; y < startY + ViewportHeight; y++)
            {
                for (long x = startX; x < startX + ViewportWidth; x++)
                {
                    Vector<long> point = new(x, y);

                    if (point == current)
                    {
                        sb.Append(GetToolCharacter(tool));
                    }
                    else if (point == this.Target)
                    {
                        sb.Append('X');
                    }
                    else if (x == 0 && y == 0)
                    {
                        sb.Append('M');
                    }
                    else if (trail.Contains(point))
                    {
                        sb.Append('*');
                    }
                    else if (scanned.Contains(point))
                    {
                        sb.Append('+');
                    }
                    else if (x < 0 || y < 0 || x >= mapWidth || y >= mapHeight)
                    {
                        sb.Append(' ');
                    }
                    else
                    {
                        sb.Append(display[this.Map[y, x].Region]);
                    }
                }

                result.Add(sb.ToString());
                sb.Clear();
            }

            result.Add(new string('-', ViewportWidth));
            result.Add(footer);

            return [.. result];
        }

        private void RenderPaddedFrames(List<string[]> frames)
        {
            int width = frames.SelectMany(frame => frame).Max(row => row.Length);
            int height = frames.Max(frame => frame.Length);

            foreach (string[] frame in frames)
            {
                this.Renderer?.RenderFrame(new Frame(PadFrame(frame, width, height)));
            }
        }

        private static string[] PadFrame(string[] frame, int width, int height)
        {
            List<string> result = [];

            foreach (string row in frame)
            {
                result.Add(row.PadRight(width, ' '));
            }

            while (result.Count < height)
            {
                result.Add(new string(' ', width));
            }

            return [.. result];
        }

        private static char GetToolCharacter(MazeToolType tool)
        {
            return tool switch
            {
                MazeToolType.Torch => 'T',
                MazeToolType.ClimbingGear => 'C',
                MazeToolType.Neither => 'N',
                _ => '?'
            };
        }

        private static string ToolName(MazeToolType tool)
        {
            return tool switch
            {
                MazeToolType.Torch => "TORCH",
                MazeToolType.ClimbingGear => "GEAR",
                MazeToolType.Neither => "NEITHER",
                _ => "UNKNOWN"
            };
        }

        private static IEnumerable<MazeState> ReconstructPath(
            Dictionary<PathKey, PathKey> cameFrom,
            Dictionary<PathKey, MazeState> states,
            PathKey target)
        {
            Stack<MazeState> path = new();

            PathKey current = target;

            while (cameFrom.ContainsKey(current))
            {
                path.Push(states[current]);
                current = cameFrom[current];
            }

            while (path.Count > 0)
            {
                yield return path.Pop();
            }
        }

        private ModeMaze Print()
        {
            Dictionary<MazeEntityType, char> display = new()
            {
                { MazeEntityType.Rocky, '.' },
                { MazeEntityType.Wet, '=' },
                { MazeEntityType.Narrow, '|' },
            };

            StringBuilder sb = new();

            for (int y = 0; y <= this.Target.Y + this.Offset.Y; y++)
            {
                for (int x = 0; x <= this.Target.X + this.Offset.X; x++)
                {
                    sb.Append(display[this.Map[y, x].Region]);
                }

                sb.Append('\n');
            }

            PuzzleConsole.WriteLine(sb.ToString());

            return this;
        }

        private static MazeEntityType GetRegionType(long errosionLevel)
        {
            long regionType = errosionLevel % 3;
            if (regionType == 0)
            {
                return MazeEntityType.Rocky;
            }
            else if (regionType == 1)
            {
                return MazeEntityType.Wet;
            }
            else if (regionType == 2)
            {
                return MazeEntityType.Narrow;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private long GetGeoLogicIndex(long x, long y)
        {
            if ((y == 0 && x == 0) || (y == this.Target.Y && x == this.Target.X))
            {
                return 0;
            }
            else if (y == 0)
            {
                return x * 16807;
            }
            else if (x == 0)
            {
                return y * 48271;
            }
            else
            {
                long a;

                if (this.Map[y, x - 1] == null)
                {
                    a = this.GetErrosionLevel(x - 1, y);
                    this.Map[y, x - 1] = new(GetRegionType(a), a);
                }
                else
                {
                    a = this.Map[y, x - 1].ErrosionLevel;
                }

                long b;
                if (this.Map[y - 1, x] == null)
                {
                    b = this.GetErrosionLevel(y - 1, x);
                    this.Map[y - 1, x] = new(GetRegionType(b), b);
                }
                else
                {
                    b = this.Map[y - 1, x].ErrosionLevel;
                }

                return a * b;
            }
        }

        private long GetErrosionLevel(long x, long y) => (this.GetGeoLogicIndex(x, y) + this.Depth) % 20183;

        private MazeEntity GetEntity(long x, long y)
        {
            long errosionLevel = this.GetErrosionLevel(x, y);
            return new(GetRegionType(errosionLevel), errosionLevel);
        }

        private void BuildMoves()
        {
            // Rocky -> Rocky
            this.Moves = new Dictionary<MazeEntityType, Dictionary<MazeEntityType, Dictionary<MazeToolType, List<MazeMove>>>>
            {
                { MazeEntityType.Rocky, new() }
            };
            this.Moves[MazeEntityType.Rocky].Add(MazeEntityType.Rocky, new());
            this.Moves[MazeEntityType.Rocky][MazeEntityType.Rocky].Add(MazeToolType.ClimbingGear, new() { new(MazeToolType.ClimbingGear, 0), new(MazeToolType.Torch, 1) });
            this.Moves[MazeEntityType.Rocky][MazeEntityType.Rocky].Add(MazeToolType.Torch, new() { new(MazeToolType.Torch, 0), new(MazeToolType.ClimbingGear, 1) });

            // Rocky -> Wet
            this.Moves[MazeEntityType.Rocky].Add(MazeEntityType.Wet, new());
            this.Moves[MazeEntityType.Rocky][MazeEntityType.Wet].Add(MazeToolType.ClimbingGear, new() { new(MazeToolType.ClimbingGear, 0) });
            this.Moves[MazeEntityType.Rocky][MazeEntityType.Wet].Add(MazeToolType.Torch, new() { new(MazeToolType.ClimbingGear, 1) });

            // Rocky -> Narrow
            this.Moves[MazeEntityType.Rocky].Add(MazeEntityType.Narrow, new());
            this.Moves[MazeEntityType.Rocky][MazeEntityType.Narrow].Add(MazeToolType.ClimbingGear, new() { new(MazeToolType.Torch, 1) });
            this.Moves[MazeEntityType.Rocky][MazeEntityType.Narrow].Add(MazeToolType.Torch, new() { new(MazeToolType.Torch, 0) });

            // Wet -> Rocky
            this.Moves.Add(MazeEntityType.Wet, new());
            this.Moves[MazeEntityType.Wet].Add(MazeEntityType.Rocky, new());
            this.Moves[MazeEntityType.Wet][MazeEntityType.Rocky].Add(MazeToolType.ClimbingGear, new() { new(MazeToolType.ClimbingGear, 0) });
            this.Moves[MazeEntityType.Wet][MazeEntityType.Rocky].Add(MazeToolType.Neither, new() { new(MazeToolType.ClimbingGear, 1) });

            // Wet -> Wet
            this.Moves[MazeEntityType.Wet].Add(MazeEntityType.Wet, new());
            this.Moves[MazeEntityType.Wet][MazeEntityType.Wet].Add(MazeToolType.ClimbingGear, new() { new(MazeToolType.ClimbingGear, 0), new(MazeToolType.Neither, 1) });
            this.Moves[MazeEntityType.Wet][MazeEntityType.Wet].Add(MazeToolType.Neither, new() { new(MazeToolType.Neither, 0), new(MazeToolType.ClimbingGear, 1) });

            // Wet -> Narrow
            this.Moves[MazeEntityType.Wet].Add(MazeEntityType.Narrow, new());
            this.Moves[MazeEntityType.Wet][MazeEntityType.Narrow].Add(MazeToolType.ClimbingGear, new() { new(MazeToolType.Neither, 1) });
            this.Moves[MazeEntityType.Wet][MazeEntityType.Narrow].Add(MazeToolType.Neither, new() { new(MazeToolType.Neither, 0) });

            // Narrow -> Rocky
            this.Moves.Add(MazeEntityType.Narrow, new());
            this.Moves[MazeEntityType.Narrow].Add(MazeEntityType.Rocky, new());
            this.Moves[MazeEntityType.Narrow][MazeEntityType.Rocky].Add(MazeToolType.Torch, new() { new(MazeToolType.Torch, 0) });
            this.Moves[MazeEntityType.Narrow][MazeEntityType.Rocky].Add(MazeToolType.Neither, new() { new(MazeToolType.Torch, 1) });

            // Narrow -> Wet
            this.Moves[MazeEntityType.Narrow].Add(MazeEntityType.Wet, new());
            this.Moves[MazeEntityType.Narrow][MazeEntityType.Wet].Add(MazeToolType.Torch, new() { new(MazeToolType.Neither, 1) });
            this.Moves[MazeEntityType.Narrow][MazeEntityType.Wet].Add(MazeToolType.Neither, new() { new(MazeToolType.Neither, 0) });

            // Narrow -> Narrow
            this.Moves[MazeEntityType.Narrow].Add(MazeEntityType.Narrow, new());
            this.Moves[MazeEntityType.Narrow][MazeEntityType.Narrow].Add(MazeToolType.Torch, new() { new(MazeToolType.Torch, 0), new(MazeToolType.Neither, 1) });
            this.Moves[MazeEntityType.Narrow][MazeEntityType.Narrow].Add(MazeToolType.Neither, new() { new(MazeToolType.Neither, 0), new(MazeToolType.Torch, 1) });
        }
    }
}
