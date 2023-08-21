namespace AdventOfCode.Puzzles._2018.Day_22___Mode_Maze
{
    using System.Text;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class ModeMaze
    {
        public ModeMaze(string[] input, long xOffset, long yOffset)
        {
            this.Moves = new();
            this.Depth = input[0].Replace("depth: ").ToInt();
            this.Target = new(input[1].Replace("target: ").Split(",").ToLong());
            this.Offset = new(xOffset, yOffset);
            this.Map = new(this.Target.X + xOffset + 1, this.Target.Y + yOffset + 1);
            this.BuildMoves();
        }

        public int Depth { get; }

        public Vector<long> Target { get; }

        public Vector<long> Offset { get; }

        public VectorArray<long, MazeEntity> Map { get; }

        private Dictionary<MazeEntityType, Dictionary<MazeEntityType, Dictionary<MazeToolType, List<MazeMove>>>> Moves { get; set; }

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

        public ModeMaze Print()
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
