namespace AdventOfCode.Puzzles._2024.Day_15___Warehouse_Woes
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Text;

    public class WarehouseWoes
    {
        public VectorArray<int, char> Map { get; private set; }

        public List<Cardinal> Moves { get; private set; }

        public Vector<int> Robot { get; private set; }

        public bool Wider { get; private set; }

        public IFrameRenderer? Renderer { get; }

        public WarehouseWoes(string[] input, bool wider = false)
        {
            this.Moves = [];
            this.Wider = wider;
            this.Parse(input, wider);
            this.Robot = this.Map.AxisEnumerator().First(x => x.Value == '@').Point;
        }

        public WarehouseWoes(string[] input, IFrameRenderer renderer, bool wider = false)
            : this(input, wider)
        {
            this.Renderer = renderer;
        }

        private bool CanPush(Vector<int> point, Cardinal move)
        {
            while (true)
            {
                point = CardinalHelper.Transform(point, move);

                if (this.Map[point] == '.')
                {
                    return true;
                }
                else if (this.Map[point] == '#')
                {
                    return false;
                }
            }
        }

        private bool CanPushVertical(List<Vector<int>> group, Cardinal move)
        {
            foreach (var point in group)
            {
                var tmp = point.Clone().Transform(move);

                if (this.Map[tmp] == '#')
                {
                    return false;
                }
            }

            return true;
        }

        private List<Vector<int>> BoxGroupHorizontal(Vector<int> point)
        {
            List<Vector<int>> result = [point];
            HashSet<Vector<int>> visited = [point];

            Queue<Vector<int>> queue = new();
            queue.Enqueue(point);

            while (queue.Count > 0)
            {
                var state = queue.Dequeue();

                foreach (var cell in this.Map.AdjacentCardinal(state))
                {
                    if (cell.Direction == Cardinal.North
                        || cell.Direction == Cardinal.South
                        || visited.Contains(cell.Point))
                    {
                        continue;
                    }

                    visited.Add(cell.Point);

                    if (cell.Value == '[' || cell.Value == ']')
                    {
                        result.Add(cell.Point.Clone());
                        queue.Enqueue(cell.Point);
                    }
                }
            }

            return result;
        }

        private List<Vector<int>> BoxGroupVertical(Vector<int> point, Cardinal direction)
        {
            List<Vector<int>> result = [point];
            HashSet<Vector<int>> visited = [point];

            Queue<Vector<int>> queue = new();
            queue.Enqueue(point);

            if (this.Map[point] == ']')
            {
                result.Add(point.Clone() + Vector<int>.West);
                queue.Enqueue(point.Clone() + Vector<int>.West);
            }
            else
            {
                result.Add(point.Clone() + Vector<int>.East);
                queue.Enqueue(point.Clone() + Vector<int>.East);
            }

            while (queue.Count > 0)
            {
                var state = queue.Dequeue();

                foreach (var cell in this.Map.AdjacentCardinal(state))
                {
                    if (visited.Contains(cell.Point)
                        || cell.Direction != direction)
                    {
                        continue;
                    }

                    visited.Add(cell.Point);

                    if (cell.Value == '[')
                    {
                        result.Add(cell.Point.Clone());
                        result.Add(cell.Point.Clone() + Vector<int>.East);

                        queue.Enqueue(cell.Point.Clone());
                        queue.Enqueue(cell.Point.Clone() + Vector<int>.East);
                    }
                    else if (cell.Value == ']')
                    {
                        result.Add(cell.Point.Clone());
                        result.Add(cell.Point.Clone() + Vector<int>.West);

                        queue.Enqueue(cell.Point.Clone());
                        queue.Enqueue(cell.Point.Clone() + Vector<int>.West);
                    }
                }
            }

            return result.Distinct().ToList();
        }

        public int GpsCoordinate()
        {
            this.Move();

            char value = this.Wider ? '[' : 'O';

            return this.Map.AxisEnumerator()
                .Where(x => x.Value == value)
                .Sum(x => 100 * x.Point.Y + x.Point.X);
        }

        public void Move()
        {
            foreach (Cardinal direction in this.Moves)
            {
                this.ApplyMove(direction);
            }
        }

        public WarehouseWoes RenderSilver(int renderEvery = 12)
            => this.RenderWarehouse(renderEvery, "WAREHOUSE WOES");

        public WarehouseWoes RenderGold(int renderEvery = 12)
            => this.RenderWarehouse(renderEvery, "WIDE WAREHOUSE WOES");

        private void ApplyMove(Cardinal direction)
        {
            Vector<int> point = CardinalHelper.Transform(this.Robot, direction);

            switch (this.Map[point])
            {
                case '.':
                    this.MoveRobot(point);
                    break;
                case '#':
                    break;
                case 'O':
                    this.MoveSingle(point, direction);
                    break;
                case '[':
                case ']':
                    this.MoveGroup(point, direction);
                    break;
            }
        }

        private void MoveRobot(Vector<int> point)
        {
            this.Map[point] = '@';
            this.Map[this.Robot] = '.';
            this.Robot = point;
        }

        private void MoveSingle(Vector<int> point, Cardinal direction)
        {
            if (this.CanPush(point.Clone(), direction))
            {
                this.Map[point] = '@';
                this.Map[this.Robot] = '.';
                this.Robot = point;

                while (true)
                {
                    point = CardinalHelper.Transform(point, direction);

                    if (this.Map[point] == '.')
                    {
                        this.Map[point] = 'O';
                        break;
                    }
                }
            }
        }

        private void MoveGroup(Vector<int> point, Cardinal direction)
        {
            bool horizontal = direction == Cardinal.West || direction == Cardinal.East;
            var group = horizontal
                        ? this.BoxGroupHorizontal(point.Clone())
                        : this.BoxGroupVertical(point.Clone(), direction);

            if (horizontal)
            {
                if (this.CanPush(point.Clone(), direction))
                {
                    group.Reverse();

                    foreach (var item in group)
                    {
                        char value = this.Map[item];
                        this.Map[item] = '.';
                        item.Transform(direction);
                        this.Map[item] = value;
                    }

                    this.MoveRobot(point);
                }
            }
            else
            {
                if (this.CanPushVertical(group, direction))
                {
                    group.Reverse();

                    foreach (var item in group)
                    {
                        char value = this.Map[item];
                        this.Map[item] = '.';
                        item.Transform(direction);
                        this.Map[item] = value;
                    }

                    this.MoveRobot(point);
                }
            }
        }

        private WarehouseWoes RenderWarehouse(int renderEvery, string title)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<string[]> frames = [];

            frames.Add(this.BuildFrame(title, 0, this.GpsTotal(), Cardinal.North));

            for (int i = 0; i < this.Moves.Count; i++)
            {
                Cardinal move = this.Moves[i];

                this.ApplyMove(move);

                if (i % renderEvery == 0 || i == this.Moves.Count - 1)
                {
                    frames.Add(this.BuildFrame(title, i + 1, this.GpsTotal(), move));
                }
            }

            for (int i = 0; i < 24; i++)
            {
                frames.Add(this.BuildFrame($"{title} COMPLETE", this.Moves.Count, this.GpsTotal(), Cardinal.North));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private int GpsTotal()
        {
            char value = this.Wider ? '[' : 'O';

            return this.Map.AxisEnumerator()
                .Where(x => x.Value == value)
                .Sum(x => 100 * x.Point.Y + x.Point.X);
        }

        private string[] BuildFrame(string title, int moveNumber, int gpsTotal, Cardinal direction)
        {
            List<string> result = [];

            result.Add($"{title} // MOVE {moveNumber:00000}/{this.Moves.Count:00000} // GPS {gpsTotal:0000000} // {this.DirectionName(direction)}");
            result.Add(string.Empty);

            for (int y = 0; y < this.Map.Height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < this.Map.Width; x++)
                {
                    sb.Append(this.Map[y, x]);
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private string DirectionName(Cardinal direction)
        {
            return direction switch
            {
                Cardinal.North => "NORTH",
                Cardinal.South => "SOUTH",
                Cardinal.West => "WEST",
                Cardinal.East => "EAST",
                _ => "WAIT"
            };
        }

        private void RenderPaddedFrames(List<string[]> frames)
        {
            int width = frames.SelectMany(frame => frame).Max(row => row.Length);
            int height = frames.Max(frame => frame.Length);

            foreach (string[] frame in frames)
            {
                this.Renderer?.RenderFrame(
                    new Frame(PadFrame(frame, width, height)));
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

        private List<string> WiderMap(List<string> map)
        {
            List<string> result = [];

            foreach (string line in map)
            {
                StringBuilder sb = new();

                foreach (char chr in line)
                {
                    switch (chr)
                    {
                        case '#':
                            sb.Append("##");
                            break;
                        case '.':
                            sb.Append("..");
                            break;
                        case '@':
                            sb.Append("@.");
                            break;
                        case 'O':
                            sb.Append("[]");
                            break;
                    }
                }

                result.Add(sb.ToString());
            }

            return result;
        }

        private void Parse(string[] input, bool wider)
        {
            List<string> map = [];
            List<string> moves = [];
            List<string> current = map;

            foreach (string line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    current = moves;
                    continue;
                }

                current.Add(line);
            }

            this.ParseMap(map, wider);
            this.ParseMoves(moves);
        }

        private void ParseMap(List<string> map, bool wider) =>
            this.Map = new([.. wider ? this.WiderMap(map) : map], c => c);

        private void ParseMoves(List<string> moves)
        {
            foreach (string line in moves)
            {
                foreach (char move in line)
                {
                    this.Moves.Add(CardinalHelper.SymbolToCardinalMap[move]);
                }
            }
        }
    }
}
