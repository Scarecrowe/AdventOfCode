namespace AdventOfCode.Puzzles._2022.Day_17___Pyroclastic_Flow
{
    using AdventOfCode.Core;

    public class PyroclasticFlow
    {
        public PyroclasticFlow(string[] input)
        {
            this.FormationIndex = -1;
            this.JetIndex = -1;
            this.JetRepeatIndexes = new();
            this.Map = new(9, 1);
            this.Rocks = new();
            this.Formations = CreateRockFormations();
            this.JetDirections = CreateJetDirections(input[0]);

            for (int i = 0; i < 9; i++)
            {
                this.Map[0, i] = i switch
                {
                    0 or 8 => StateType.Corner,
                    _ => StateType.HorizontalWall,
                };
            }
        }

        public VectorArray<int, StateType> Map { get; private set; }

        public int FormationIndex { get; private set; }

        public int JetIndex { get; private set; }

        public bool JetWrapped { get; private set; }

        public Dictionary<int, Rock> Formations { get; private set; }

        public List<Rock> Rocks { get; private set; }

        public Dictionary<int, JetDirection> JetDirections { get; private set; }

        public int? JetRepeatIndex { get; private set; }

        public HashSet<int> JetRepeatIndexes { get; private set; }

        private Dictionary<StateType, char> CharMap { get; } = new()
        {
            { StateType.Air, '.' },
            { StateType.HorizontalWall, '-' },
            { StateType.VerticalWall, '|' },
            { StateType.Rock, '#' },
            { StateType.Corner, '+' }
        };

        public void PrintMap(int index, bool isFall = false)
        {
            PuzzleConsole.WriteLine($"Rock {index}, Action: {(!isFall ? "Jet" : "Fall")}{(!isFall ? $" {this.JetIndex} {(this.JetDirections[this.JetIndex] == JetDirection.Left ? '<' : '>')}" : string.Empty)}");

            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = 0; x < this.Map.Width; x++)
                {
                    PuzzleConsole.Write((char)this.Map[y, x]);
                }

                PuzzleConsole.WriteLine();
            }

            PuzzleConsole.WriteLine();
        }

        public int Top()
        {
            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = 0; x < this.Map.Width; x++)
                {
                    if (this.Map[y, x] == StateType.Rock)
                    {
                        return y;
                    }
                }
            }

            return 0;
        }

        public long Stack(long count)
        {
            this.JetRepeatIndexes = new();
            Rock rock = this.Formations[this.NextFormation()];
            this.ResizeMap(rock.Map.Height);
            rock.AddToMap(new(3, this.GetStart(rock)), this.Map);

            long targetRockFallIndex = count;
            long bottomHeight = 0;
            long bottomRockCount = 0;
            long repeatHeight = 0;
            long repeatRockCount = 0;

            for (long rockIndex = 0; rockIndex < targetRockFallIndex; rockIndex++)
            {
                while (true)
                {
                    this.NextJetDirection();

                    if (this.JetDirections[this.JetIndex] == JetDirection.Left && !rock.IsCollision(this.Map, new(rock.Point.X - 1, rock.Point.Y)))
                    {
                        rock.Move(this.JetDirections[this.JetIndex], this.Map);
                    }
                    else if (this.JetDirections[this.JetIndex] == JetDirection.Right && !rock.IsCollision(this.Map, new(rock.Point.X + 1, rock.Point.Y)))
                    {
                        rock.Move(this.JetDirections[this.JetIndex], this.Map);
                    }

                    if (rock.IsCollision(this.Map, new(rock.Point.X, rock.Point.Y + 1)))
                    {
                        break;
                    }

                    rock.Fall(this.Map);
                }

                this.Rocks.Add(rock);
                rock = this.Formations[this.NextFormation()];

                if (count > 2022 && this.JetWrapped && this.FormationIndex == 0)
                {
                    if (this.JetRepeatIndex == null)
                    {
                        if (this.JetRepeatIndexes.Contains(this.JetIndex))
                        {
                            bottomHeight = this.Map.Height - 5;
                            bottomRockCount = rockIndex;
                            this.JetRepeatIndex = this.JetIndex;
                        }
                        else
                        {
                            this.JetRepeatIndexes.Add(this.JetIndex);
                        }
                    }
                    else if (this.JetIndex == this.JetRepeatIndex)
                    {
                        repeatHeight = (this.Map.Height - 5) - bottomHeight;
                        repeatRockCount = rockIndex - bottomRockCount;
                        targetRockFallIndex = rockIndex + ((count - bottomRockCount) % repeatRockCount);
                    }
                }

                if (rockIndex < count - 1)
                {
                    this.ResizeMap(rock.Map.Height);
                    rock.AddToMap(new(3, this.GetStart(rock)), this.Map);
                }
            }

            if (count > 2022)
            {
                long moduloHeight = (this.Map.Height - 5) - bottomHeight - repeatHeight;
                long repeatCount = (repeatRockCount > 0) ? ((count - bottomRockCount) / repeatRockCount) : 0;

                return bottomHeight + (repeatCount * repeatHeight) + moduloHeight;
            }

            return this.Map.Height - rock.Map.Height - 1;
        }

        private static Dictionary<int, JetDirection> CreateJetDirections(string input)
        {
            Dictionary<int, JetDirection> result = new();

            for (int i = 0; i < input.Length; i++)
            {
                result.Add(i, input[i] == '<' ? JetDirection.Left : JetDirection.Right);
            }

            return result;
        }

        private static Dictionary<int, Rock> CreateRockFormations()
        {
            Dictionary<int, Rock> result = new();

            result.Add(0, new("####"));
            result.Add(1, new(".#.\r###\r.#."));
            result.Add(2, new("..#\r..#\r###"));
            result.Add(3, new("#\r#\r#\r#"));
            result.Add(4, new("##\r##;"));

            return result;
        }

        private void ResizeMap(int increase)
        {
            int top = this.Top() - (3 + increase);
            int height = this.Map.Height;

            if (top < 0)
            {
                height += Math.Abs(top);
            }

            increase = height - this.Map.Height;

            VectorArray<int, StateType> map = new(this.Map.Width, height);

            for (int y = increase; y < height; y++)
            {
                for (int x = 0; x < this.Map.Width; x++)
                {
                    map[y, x] = this.Map[y - increase, x];
                }
            }

            for (int y = 0; y < increase; y++)
            {
                map[y, 0] = StateType.VerticalWall;
                map[y, this.Map.Width - 1] = StateType.VerticalWall;
            }

            this.Map = map;
        }

        private int NextFormation()
        {
            this.FormationIndex++;
            this.FormationIndex %= this.Formations.Count;

            return this.FormationIndex;
        }

        private int NextJetDirection()
        {
            this.JetIndex++;
            this.JetIndex %= this.JetDirections.Count;

            if (this.JetIndex == 0)
            {
                this.JetWrapped = true;
            }

            return this.JetIndex;
        }

        private long GetStart(Rock rock)
        {
            long top = this.Top();

            if (top != 0)
            {
                top -= 3 + rock.Map.Height;
            }

            return top;
        }
    }
}
