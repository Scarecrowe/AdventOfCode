namespace AdventOfCode.Puzzles._2024.Day_15___Warehouse_Woes
{
    using AdventOfCode.Core;
    using System.Drawing;
    using System.Text;

    public class WarehouseWoes
    {
        public VectorArray<int, char> Map { get; private set; }

        public List<Cardinal> Moves { get; private set; }

        public Vector<int> Robot { get; private set; }

        public bool Wider { get; private set; } 

        public WarehouseWoes(string[] input, bool wider = false)
        {
            this.Moves = [];
            this.Wider = wider;
            this.Parse(input, wider);
            this.Robot = this.Map.AxisEnumerator().First(x => x.Value == '@').Point;
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
            foreach(var point in group)
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

                foreach(var cell in this.Map.AdjacentCardinal(state))
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
            } else
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
            bool horizontal = (direction == Cardinal.West || direction == Cardinal.East);
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
