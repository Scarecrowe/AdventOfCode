namespace AdventOfCode.Puzzles._2018.Day_13___Mine_Cart_Madness
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Text;

    public class MineCartMadness
    {
        private static readonly Vector<int> NoPoint = new(-1, -1);

        public MineCartMadness(string[] input)
        {
            this.Carts = new();
            this.Track = new(input, (c, x, y) =>
            {
                if ("<>^v".Contains(c))
                {
                    this.Carts.Add(new(new(x, y), c));
                    return c == '<' || c == '>' ? '-' : '|';
                }

                return c;
            });
        }

        public MineCartMadness(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        public VectorDictionary<int, char> Track { get; }

        public List<MineCart> Carts { get; private set; }

        public string Tick(bool exitOnCollision = true)
        {
            int count = this.Carts.Count;

            while (true)
            {
                if (!exitOnCollision && count == 1)
                {
                    return $"{this.Carts.First(x => !x.Collided).Point.X},{this.Carts.First(x => !x.Collided).Point.Y}";
                }

                foreach (MineCart cart in this.Carts.Where(x => !x.Collided).OrderBy(x => x.Point.ToTuple2D()))
                {
                    Vector<int> location = cart.PeekMove();

                    if (exitOnCollision && this.Carts.Any(x => x.Point == location))
                    {
                        return $"{location.X},{location.Y}";
                    }

                    int index = this.Carts.FindIndex(x => x.Point == location && !x.Collided);

                    if (index > -1)
                    {
                        cart.MarkCollided();
                        this.Carts[index].MarkCollided();
                        count -= 2;
                        continue;
                    }

                    cart.Move();

                    switch (this.Track[location])
                    {
                        case '+':
                            cart.IntersectionTurn();
                            break;
                        case '/':
                        case '\\':
                            cart.CornerTurn(this.Track[location]);
                            break;
                    }
                }
            }
        }

        public MineCartMadness RenderSilver(int renderEvery = 1)
        {
            return this.RenderUntilFirstCrash(renderEvery);
        }

        public MineCartMadness RenderGold(int renderEvery = 1)
        {
            return this.RenderUntilLastCart(renderEvery);
        }

        private MineCartMadness RenderUntilFirstCrash(int renderEvery)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            int tick = 0;
            int moves = 0;
            Vector<int> crash = NoPoint;
            List<string[]> frames = [];

            frames.Add(this.BuildFrame(
                tick,
                moves,
                NoPoint,
                "MINE CART MADNESS // FIRST CRASH SEARCH"));

            while (crash == NoPoint)
            {
                foreach (MineCart cart in this.Carts.Where(x => !x.Collided).OrderBy(x => x.Point.ToTuple2D()).ToList())
                {
                    Vector<int> location = cart.PeekMove();

                    if (this.Carts.Any(x => x.Point == location && !x.Collided))
                    {
                        crash = new(location);
                        cart.Move();

                        frames.Add(this.BuildFrame(
                            tick,
                            moves + 1,
                            crash,
                            $"FIRST CRASH AT {crash.X},{crash.Y} // TICK {tick:0000}"));

                        break;
                    }

                    cart.Move();
                    this.TurnCart(cart, location);
                    moves++;

                    if (moves % renderEvery == 0)
                    {
                        frames.Add(this.BuildFrame(
                            tick,
                            moves,
                            NoPoint,
                            $"MINE CART MADNESS // TICK {tick:0000} // MOVES {moves:000000}"));
                    }
                }

                tick++;
            }

            for (int i = 0; i < 24; i++)
            {
                frames.Add(this.BuildFrame(
                    tick,
                    moves,
                    crash,
                    $"FIRST CRASH AT {crash.X},{crash.Y}"));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private MineCartMadness RenderUntilLastCart(int renderEvery)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            int tick = 0;
            int moves = 0;
            int crashes = 0;
            List<Vector<int>> recentCrashes = [];
            List<string[]> frames = [];

            frames.Add(this.BuildFrame(
                tick,
                moves,
                NoPoint,
                "MINE CART MADNESS // LAST CART SEARCH"));

            while (this.Carts.Count(x => !x.Collided) > 1)
            {
                foreach (MineCart cart in this.Carts.Where(x => !x.Collided).OrderBy(x => x.Point.ToTuple2D()).ToList())
                {
                    if (cart.Collided)
                    {
                        continue;
                    }

                    Vector<int> location = cart.PeekMove();
                    int index = this.Carts.FindIndex(x => x.Point == location && !x.Collided);

                    if (index > -1)
                    {
                        cart.MarkCollided();
                        this.Carts[index].MarkCollided();
                        crashes++;
                        recentCrashes.Add(new(location));

                        if (recentCrashes.Count > 16)
                        {
                            recentCrashes.RemoveAt(0);
                        }

                        moves++;

                        frames.Add(this.BuildFrame(
                            tick,
                            moves,
                            location,
                            $"CRASH {crashes:000} AT {location.X},{location.Y} // CARTS {this.Carts.Count(x => !x.Collided):000}"));

                        continue;
                    }

                    cart.Move();
                    this.TurnCart(cart, location);
                    moves++;

                    if (moves % renderEvery == 0)
                    {
                        frames.Add(this.BuildFrame(
                            tick,
                            moves,
                            recentCrashes.Count > 0 ? recentCrashes.Last() : NoPoint,
                            $"MINE CART MADNESS // TICK {tick:0000} // CARTS {this.Carts.Count(x => !x.Collided):000}"));
                    }
                }

                tick++;
            }

            MineCart last = this.Carts.First(x => !x.Collided);

            for (int i = 0; i < 24; i++)
            {
                frames.Add(this.BuildFrame(
                    tick,
                    moves,
                    last.Point,
                    $"LAST CART AT {last.Point.X},{last.Point.Y} // TICK {tick:0000}"));
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private void TurnCart(MineCart cart, Vector<int> location)
        {
            switch (this.Track[location])
            {
                case '+':
                    cart.IntersectionTurn();
                    break;
                case '/':
                case '\\':
                    cart.CornerTurn(this.Track[location]);
                    break;
            }
        }

        private string[] BuildFrame(
            int tick,
            int moves,
            Vector<int> highlight,
            string title)
        {
            List<string> result = [];

            result.Add(title);
            result.Add($"TICK {tick:0000} // MOVES {moves:000000} // ACTIVE CARTS {this.Carts.Count(x => !x.Collided):000}");
            result.Add(string.Empty);

            for (int y = 0; y < this.Track.Height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < this.Track.Width; x++)
                {
                    Vector<int> point = new(x, y);
                    MineCart? cart = this.Carts.FirstOrDefault(c => c.Point == point && !c.Collided);

                    if (point == highlight)
                    {
                        sb.Append('X');
                    }
                    else if (cart != null)
                    {
                        sb.Append(cart.Direction);
                    }
                    else
                    {
                        sb.Append(this.Track[point]);
                    }
                }

                result.Add(sb.ToString());
            }

            return [.. result];
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

        private void Print()
        {
            for (int y = 0; y < this.Track.Height; y++)
            {
                for (int x = 0; x < this.Track.Width; x++)
                {
                    if (this.Track[new(x, y)] != 'X' && this.Carts.Any(c => c.Point == new Vector<int>(x, y) && !c.Collided))
                    {
                        PuzzleConsole.Write(this.Carts.First(c => c.Point == new Vector<int>(x, y) && !c.Collided).Direction);
                    }
                    else
                    {
                        PuzzleConsole.Write(this.Track[new(x, y)]);
                    }
                }

                PuzzleConsole.WriteLine();
            }

            PuzzleConsole.WriteLine();
        }
    }
}