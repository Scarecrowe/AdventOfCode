namespace AdventOfCode.Puzzles._2022.Day_17___Pyroclastic_Flow
{
    using AdventOfCode.Core;

    public class Rock
    {
        public Rock(string formation)
        {
            string[] tokens = formation.Split('\r');
            this.Map = new(tokens[0].Length, tokens.Length);
            this.Point = new(0, 0);

            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = 0; x < this.Map.Width; x++)
                {
                    if (tokens[y][x] == '#')
                    {
                        this.Map[y, x] = StateType.Rock;
                    }
                }
            }
        }

        private Rock(VectorArray<int, StateType> map)
        {
            this.Map = map;
            this.Point = new(0, 0);
        }

        public VectorArray<int, StateType> Map { get; private set; }

        public Vector<int> Point { get; }

        public Rock Clone() => new(this.Map);

        public void AddToMap(Vector<int> point, VectorArray<int, StateType> map)
        {
            this.Point.X = point.X;
            this.Point.Y = point.Y;

            if (point.Y < 0)
            {
                return;
            }

            for (long y = this.Point.Y; y < this.Point.Y + this.Map.Height; y++)
            {
                for (long x = this.Point.X; x < this.Point.X + this.Map.Width; x++)
                {
                    if (this.Map[y - this.Point.Y, x - this.Point.X] != StateType.Air)
                    {
                        map[y, x] = this.Map[y - this.Point.Y, x - this.Point.X];
                    }
                }
            }
        }

        public void RemoveFromMap(VectorArray<int, StateType> map)
        {
            if (this.Point.Y < 0)
            {
                return;
            }

            for (long y = this.Point.Y; y < this.Point.Y + this.Map.Height; y++)
            {
                for (long x = this.Point.X; x < this.Point.X + this.Map.Width; x++)
                {
                    if (this.Map[y - this.Point.Y, x - this.Point.X] != StateType.Air)
                    {
                        map[y, x] = StateType.Air;
                    }
                }
            }
        }

        public void Move(JetDirection direction, VectorArray<int, StateType> map)
        {
            this.RemoveFromMap(map);
            this.AddToMap(new(direction == JetDirection.Left ? this.Point.X - 1 : this.Point.X + 1, this.Point.Y), map);
        }

        public void Fall(VectorArray<int, StateType> map)
        {
            this.RemoveFromMap(map);
            this.AddToMap(new(this.Point.X, this.Point.Y + 1), map);
        }

        public bool IsCollision(VectorArray<int, StateType> map, Vector<int> point)
        {
            if (point.X == 0 || point.X == map.Width - 1 || point.Y == map.Height - 1)
            {
                return true;
            }

            this.RemoveFromMap(map);

            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = 0; x < this.Map.Width; x++)
                {
                    if (this.Map[y, x] == StateType.Rock
                        && map[y + point.Y, point.X + x] != StateType.Air)
                    {
                        this.AddToMap(new(this.Point.X, this.Point.Y), map);
                        return true;
                    }
                }
            }

            this.AddToMap(new(this.Point.X, this.Point.Y), map);

            return false;
        }
    }
}
