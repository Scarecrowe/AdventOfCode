namespace AdventOfCode.Puzzles._2022.Day_09___Rope_Bridge
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using System.Text;

    public class RopeBridge
    {
        private const int ViewWidth = 80;
        private const int ViewHeight = 40;

        public RopeBridge(string[] moves, int knotCount)
        {
            this.Input = moves;
            this.KnotCount = knotCount;
            this.Reset();
        }

        public RopeBridge(string[] moves, int knotCount, IFrameRenderer renderer)
            : this(moves, knotCount)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private string[] Input { get; }

        private int KnotCount { get; }

        private List<Vector<int>> Knots { get; set; } = [];

        private HashSet<Vector<int>> Visits { get; set; } = [];

        public int Visited()
        {
            this.Reset();
            this.Simulate();
            return this.Visits.Count;
        }

        public RopeBridge RenderSilver(int renderEvery = 1)
            => this.Render("ROPE BRIDGE // KNOTS 02", renderEvery);

        public RopeBridge RenderGold(int renderEvery = 4)
            => this.Render("ROPE BRIDGE // KNOTS 10", renderEvery);

        private RopeBridge Render(string title, int renderEvery)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            this.Reset();

            int moveIndex = 0;

            this.Renderer.RenderFrame(
                new Frame(this.BuildFrame($"{title} // START")));

            this.Simulate((step, direction, moveStep, isEndOfMove) =>
            {
                if (!isEndOfMove)
                {
                    return;
                }

                moveIndex++;

                if (moveIndex % renderEvery != 0)
                {
                    return;
                }

                this.Renderer.RenderFrame(
                    new Frame(this.BuildFrame(
                        $"{title} // STEP {step:00000} // VISITED {this.Visits.Count:0000}")));
            });

            this.Renderer.RenderFrame(
                new Frame(this.BuildFrame(
                    $"{title} // COMPLETE // VISITED {this.Visits.Count:0000}")));

            return this;
        }

        private void Simulate(Action<int, Cardinal, int, bool>? render = null)
        {
            int step = 0;

            foreach ((Cardinal direction, int count) in this.Moves())
            {
                for (int i = 1; i <= count; i++)
                {
                    step++;

                    this.MoveHead(direction);

                    for (int j = 1; j < this.KnotCount; j++)
                    {
                        this.Follow(j);
                    }

                    this.Visits.Add(this.Knots[^1]);

                    render?.Invoke(step, direction, i, i == count);
                }
            }
        }

        private string[] BuildFrame(string title)
        {
            List<string> result = [];

            result.Add(title);
            result.Add(string.Empty);

            Vector<int> focus = this.Knots[0];

            int minX = focus.X - ViewWidth / 2;
            int minY = focus.Y - ViewHeight / 2;

            for (int y = minY; y < minY + ViewHeight; y++)
            {
                StringBuilder sb = new();

                for (int x = minX; x < minX + ViewWidth; x++)
                {
                    Vector<int> point = new(x, y);
                    char? knot = this.KnotAt(point);

                    if (knot != null)
                    {
                        sb.Append(knot.Value);
                    }
                    else if (point.X == 0 && point.Y == 0)
                    {
                        sb.Append('s');
                    }
                    else if (this.Visits.Contains(point))
                    {
                        sb.Append('#');
                    }
                    else
                    {
                        sb.Append('.');
                    }
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private char? KnotAt(Vector<int> point)
        {
            for (int i = 0; i < this.Knots.Count; i++)
            {
                if (this.Knots[i] != point)
                {
                    continue;
                }

                if (i == 0)
                {
                    return 'H';
                }

                if (this.KnotCount == 2 && i == 1)
                {
                    return 'T';
                }

                return i.ToString()[0];
            }

            return null;
        }

        private void Follow(int index)
        {
            Vector<int> leader = this.Knots[index - 1];
            Vector<int> follower = this.Knots[index];

            int dx = leader.X - follower.X;
            int dy = leader.Y - follower.Y;

            if (Math.Abs(dx) <= 1 && Math.Abs(dy) <= 1)
            {
                return;
            }

            this.Knots[index] = new(
                follower.X + Math.Sign(dx),
                follower.Y + Math.Sign(dy));
        }

        private void Reset()
        {
            this.Knots = [];
            this.Visits = [];

            for (int i = 0; i < this.KnotCount; i++)
            {
                this.Knots.Add(new(0, 0));
            }

            this.Visits.Add(new(0, 0));
        }

        private IEnumerable<(Cardinal Direction, int Count)> Moves()
        {
            foreach (string move in this.Input)
            {
                string[] tokens = move.SplitSpace();

                Cardinal direction = tokens[0] switch
                {
                    "U" => Cardinal.North,
                    "D" => Cardinal.South,
                    "L" => Cardinal.West,
                    "R" => Cardinal.East,
                    _ => throw new InvalidOperationException()
                };

                yield return (direction, int.Parse(tokens[1]));
            }
        }

        private void MoveHead(Cardinal direction)
        {
            this.Knots[0] = direction switch
            {
                Cardinal.North => new(this.Knots[0].X, this.Knots[0].Y - 1),
                Cardinal.South => new(this.Knots[0].X, this.Knots[0].Y + 1),
                Cardinal.West => new(this.Knots[0].X - 1, this.Knots[0].Y),
                Cardinal.East => new(this.Knots[0].X + 1, this.Knots[0].Y),
                _ => this.Knots[0]
            };
        }
    }
}