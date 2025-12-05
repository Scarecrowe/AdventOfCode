namespace AdventOfCode.Puzzles._2018.Day_13___Mine_Cart_Madness
{
    using AdventOfCode.Core;

    public class MineCartMadness
    {
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
