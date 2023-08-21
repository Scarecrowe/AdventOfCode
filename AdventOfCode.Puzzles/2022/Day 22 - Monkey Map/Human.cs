namespace AdventOfCode.Puzzles._2022.Day_22___Monkey_Map
{
    using AdventOfCode.Core;

    public class Human
    {
        public Human(VectorArray<int, MonkeyMapState> map)
        {
            this.Point = new(0, 0);
            this.Visited = new();
            this.Turns = new()
            {
                {
                    "R", new()
                    {
                        { Cardinal.North, Cardinal.East },
                        { Cardinal.South, Cardinal.West },
                        { Cardinal.East, Cardinal.South },
                        { Cardinal.West, Cardinal.North },
                    }
                },
                {
                    "L", new()
                    {
                        { Cardinal.North, Cardinal.West },
                        { Cardinal.South, Cardinal.East },
                        { Cardinal.East, Cardinal.North },
                        { Cardinal.West, Cardinal.South },
                    }
                }
            };

            this.Moves = new()
            {
                { Cardinal.North, new(0, -1) },
                { Cardinal.South, new(0, 1) },
                { Cardinal.East, new(1, 0) },
                { Cardinal.West, new(-1, 0) }
            };

            this.Facing = Cardinal.East;

            for (int x = 0; x < map.Width; x++)
            {
                if (map[0, x] == MonkeyMapState.Open)
                {
                    this.Point.X = x;
                    break;
                }
            }

            this.Visited.Add(new(this.Point.X, 0), Cardinal.East);
        }

        public Vector<int> Point { get; private set; }

        public Cardinal Facing { get; private set; }

        public Dictionary<string, Dictionary<Cardinal, Cardinal>> Turns { get; private set; }

        public Dictionary<Cardinal, Vector<int>> Moves { get; private set; }

        public Dictionary<Vector<int>, Cardinal> Visited { get; private set; }

        public static Cardinal ReverseDirection(Cardinal facing)
        {
            return facing switch
            {
                Cardinal.North => Cardinal.South,
                Cardinal.South => Cardinal.North,
                Cardinal.East => Cardinal.West,
                Cardinal.West => Cardinal.East,
                _ => throw new InvalidOperationException(),
            };
        }

        public Human Turn(string turn)
        {
            this.Facing = this.Turns[turn][this.Facing];

            return this;
        }

        public MonkeyMapFace Face()
        {
            if (this.Point.Y >= 0 && this.Point.Y < 50 && this.Point.X >= 100 && this.Point.X < 150)
            {
                return MonkeyMapFace.Top;
            }
            else if (this.Point.Y >= 100 && this.Point.Y < 150 && this.Point.X >= 0 && this.Point.X < 50)
            {
                return MonkeyMapFace.Bottom;
            }
            else if (this.Point.Y >= 0 && this.Point.Y < 50 && this.Point.X >= 50 && this.Point.X < 100)
            {
                return MonkeyMapFace.East;
            }
            else if (this.Point.Y >= 50 && this.Point.Y < 100 && this.Point.X >= 50 && this.Point.X < 100)
            {
                return MonkeyMapFace.North;
            }
            else if (this.Point.Y >= 100 && this.Point.Y < 150 && this.Point.X >= 50 && this.Point.X < 100)
            {
                return MonkeyMapFace.West;
            }
            else if (this.Point.Y >= 150 && this.Point.Y < 200 && this.Point.X >= 0 && this.Point.X < 50)
            {
                return MonkeyMapFace.South;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public Human AddVisit()
        {
            if (!this.Visited.ContainsKey(this.Point))
            {
                this.Visited.Add(this.Point, this.Facing);
            }
            else
            {
                this.Visited[this.Point] = this.Facing;
            }

            return this;
        }

        public Human MoveTwoDimensional(int spaces, VectorArray<int, MonkeyMapState> map)
        {
            int width = map.Width;
            int height = map.Height;

            for (int i = 0; i < spaces; i++)
            {
                Vector<int> next = this.Moves[this.Facing];
                next = this.Point + next;

                if (next.X >= width)
                {
                    next.X = 0;
                }
                else if (next.X < 0)
                {
                    next.X = width - 1;
                }

                if (next.Y >= height)
                {
                    next.Y = 0;
                }
                else if (next.Y < 0)
                {
                    next.Y = height - 1;
                }

                switch (map[next.Y, next.X])
                {
                    case MonkeyMapState.Void:
                        switch (this.Facing)
                        {
                            case Cardinal.North:
                                for (int y = height - 1; y >= 0; y--)
                                {
                                    if (map[y, this.Point.X] == MonkeyMapState.Closed)
                                    {
                                        return this;
                                    }

                                    if (map[y, this.Point.X] == MonkeyMapState.Open)
                                    {
                                        this.Point.Y = y;
                                        break;
                                    }
                                }

                                break;
                            case Cardinal.South:
                                for (int y = 0; y < height; y++)
                                {
                                    if (map[y, this.Point.X] == MonkeyMapState.Closed)
                                    {
                                        return this;
                                    }

                                    if (map[y, this.Point.X] == MonkeyMapState.Open)
                                    {
                                        this.Point.Y = y;
                                        break;
                                    }
                                }

                                break;
                            case Cardinal.East:
                                for (int x = 0; x < width; x++)
                                {
                                    if (map[this.Point.Y, x] == MonkeyMapState.Closed)
                                    {
                                        return this;
                                    }

                                    if (map[this.Point.Y, x] == MonkeyMapState.Open)
                                    {
                                        this.Point.X = x;
                                        break;
                                    }
                                }

                                break;
                            case Cardinal.West:
                                for (int x = width - 1; x >= 0; x--)
                                {
                                    if (map[this.Point.Y, x] == MonkeyMapState.Closed)
                                    {
                                        return this;
                                    }

                                    if (map[this.Point.Y, x] == MonkeyMapState.Open)
                                    {
                                        this.Point.X = x;
                                        break;
                                    }
                                }

                                break;
                        }

                        break;
                    case MonkeyMapState.Closed:
                        this.AddVisit();
                        return this;
                    case MonkeyMapState.Open:
                        this.Point = next;
                        break;
                }

                this.AddVisit();
            }

            return this;
        }

        public Human MoveThreeDimensional(int spaces, VectorArray<int, MonkeyMapState> map)
        {
            for (int i = 0; i < spaces; i++)
            {
                MonkeyMapFace face = this.Face();
                Vector<int> next = this.Moves[this.Facing];
                next = this.Point + next;
                Cardinal facing = this.Facing;

                switch (face)
                {
                    case MonkeyMapFace.East:
                        switch (this.Facing)
                        {
                            case Cardinal.North:
                                if (next.Y < 0)
                                {
                                    next.Y = 150 + (next.X - 50);
                                    next.X = 0;
                                    facing = Cardinal.East;
                                }

                                break;
                            case Cardinal.West:
                                if (next.X < 50)
                                {
                                    next.X = 0;
                                    next.Y = 100 + (50 - next.Y) - 1;
                                    facing = Cardinal.East;
                                }

                                break;
                        }

                        break;
                    case MonkeyMapFace.West:
                        switch (this.Facing)
                        {
                            case Cardinal.East:
                                if (next.X >= 100)
                                {
                                    next.X = 150 - 1;
                                    next.Y = 50 - (next.Y - 100) - 1;
                                    facing = Cardinal.West;
                                }

                                break;
                            case Cardinal.South:
                                if (next.Y >= 150)
                                {
                                    next.Y = 150 + (next.X - 50);
                                    next.X = 50 - 1;
                                    facing = Cardinal.West;
                                }

                                break;
                        }

                        break;
                    case MonkeyMapFace.North:
                        switch (this.Facing)
                        {
                            case Cardinal.East:
                                if (next.X >= 100)
                                {
                                    next.X = 100 + (next.Y - 50);
                                    next.Y = 50 - 1;
                                    facing = Cardinal.North;
                                }

                                break;
                            case Cardinal.West:
                                if (next.X < 50)
                                {
                                    next.X = next.Y - 50;
                                    next.Y = 100;
                                    facing = Cardinal.South;
                                }

                                break;
                        }

                        break;
                    case MonkeyMapFace.South:
                        switch (this.Facing)
                        {
                            case Cardinal.East:
                                if (next.X >= 50)
                                {
                                    next.X = 50 + (next.Y - 150);
                                    next.Y = 150 - 1;
                                    facing = Cardinal.North;
                                }

                                break;
                            case Cardinal.West:
                                if (next.X < 0)
                                {
                                    next.X = 50 + (next.Y - 150);
                                    next.Y = 0;
                                    facing = Cardinal.South;
                                }

                                break;
                            case Cardinal.South:
                                if (next.Y >= 200)
                                {
                                    next.X = 100 + next.X;
                                    next.Y = 0;
                                    facing = Cardinal.South;
                                }

                                break;
                        }

                        break;
                    case MonkeyMapFace.Top:
                        switch (this.Facing)
                        {
                            case Cardinal.East:
                                if (next.X >= 150)
                                {
                                    next.X = 100 - 1;
                                    next.Y = 100 + (50 - next.Y) - 1;
                                    facing = Cardinal.West;
                                }

                                break;
                            case Cardinal.North:
                                if (next.Y < 0)
                                {
                                    next.X -= 100;
                                    next.Y = 200 - 1;
                                    facing = Cardinal.North;
                                }

                                break;
                            case Cardinal.South:
                                if (next.Y >= 50)
                                {
                                    next.Y = 50 + (next.X - 100);
                                    next.X = 100 - 1;
                                    facing = Cardinal.West;
                                }

                                break;
                        }

                        break;
                    case MonkeyMapFace.Bottom:
                        switch (this.Facing)
                        {
                            case Cardinal.West:
                                if (next.X < 0)
                                {
                                    next.X = 50;
                                    next.Y = (50 - (next.Y - 100)) - 1;
                                    facing = Cardinal.East;
                                }

                                break;
                            case Cardinal.North:
                                if (next.Y < 100)
                                {
                                    next.Y = 50 + next.X;
                                    next.X = 50;
                                    facing = Cardinal.East;
                                }

                                break;
                        }

                        break;
                }

                switch (map[next.Y, next.X])
                {
                    case MonkeyMapState.Closed:
                        return this;
                    case MonkeyMapState.Open:
                        this.Point = next;
                        this.Facing = facing; break;
                    case MonkeyMapState.Void:
                        break;
                }
            }

            return this;
        }

        public long Password() => (1000 * (this.Point.Y + 1)) + (4 * (this.Point.X + 1)) + (int)this.Facing;
    }
}
