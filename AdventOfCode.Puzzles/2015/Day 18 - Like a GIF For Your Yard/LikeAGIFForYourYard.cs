namespace AdventOfCode.Puzzles._2015.Day_18___Like_a_GIF_For_Your_Yard
{
    using AdventOfCode.Core;

    public class LikeAGIFForYourYard
    {
        public LikeAGIFForYourYard(string[] input) => this.Map = new(input, (c) => c == '#' ? 1 : 0);

        public VectorArray<int, int> Map { get; private set; }

        public int IsNorthLit(Vector<int> point) => point.Y == 0 ? 0 : this.Map[point.Y - 1, point.X];

        public int IsNorthEastLit(Vector<int> point) => (point.Y == 0 || point.X == this.Map.Width - 1) ? 0 : this.Map[point.Y - 1, point.X + 1];

        public int IsEastLit(Vector<int> point) => (point.X == this.Map.Width - 1) ? 0 : this.Map[point.Y, point.X + 1];

        public int IsSouthEastLit(Vector<int> point) => (point.Y == this.Map.Height - 1 || point.X == this.Map.Width - 1) ? 0 : this.Map[point.Y + 1, point.X + 1];

        public int IsSouthLit(Vector<int> point) => (point.Y == this.Map.Height - 1) ? 0 : this.Map[point.Y + 1, point.X];

        public int IsSouthWestLit(Vector<int> point) => (point.Y == this.Map.Height - 1 || point.X == 0) ? 0 : this.Map[point.Y + 1, point.X - 1];

        public int IsWestLit(Vector<int> point) => (point.X == 0) ? 0 : this.Map[point.Y, point.X - 1];

        public int IsNorthWestLit(Vector<int> point) => (point.Y == 0 || point.X == 0) ? 0 : this.Map[point.Y - 1, point.X - 1];

        public int LitAdjacent(Vector<int> point)
        {
            return this.IsNorthLit(point)
                + this.IsNorthEastLit(point)
                + this.IsEastLit(point)
                + this.IsSouthEastLit(point)
                + this.IsSouthLit(point)
                + this.IsSouthWestLit(point)
                + this.IsWestLit(point)
                + this.IsNorthWestLit(point);
        }

        public LikeAGIFForYourYard Animate(int steps)
        {
            for (int step = 1; step <= steps; step++)
            {
                VectorArray<int, int> current = new(this.Map);

                foreach (VectorCell<int, int> cell in this.Map.AxisEnumerator())
                {
                    int lit = this.LitAdjacent(cell.Point);

                    if (cell.Value == 1)
                    {
                        if (!(lit >= 2 && lit <= 3))
                        {
                            current[cell.Point] = 0;
                        }

                        continue;
                    }

                    if (lit == 3)
                    {
                        current[cell.Point] = 1;
                    }
                }

                this.Map = new(current);
            }

            return this;
        }

        public LikeAGIFForYourYard AnimateGameOfLife(int steps)
        {
            this.Map[0, 0] = 1;
            this.Map[0, this.Map.Width - 1] = 1;
            this.Map[this.Map.Height - 1, 0] = 1;
            this.Map[this.Map.Height - 1, this.Map.Width - 1] = 1;

            for (int step = 1; step <= steps; step++)
            {
                VectorArray<int, int> current = new(this.Map);

                foreach (VectorCell<int, int> cell in this.Map.AxisEnumerator())
                {
                    int lit = this.LitAdjacent(cell.Point);

                    if (cell.Value == 1)
                    {
                        if (!(lit >= 2 && lit <= 3))
                        {
                            current[cell.Point] = 0;
                        }

                        continue;
                    }

                    if (lit == 3)
                    {
                        current[cell.Point] = 1;
                    }
                }

                this.Map = new(current);

                this.Map[0, 0] = 1;
                this.Map[0, this.Map.Width - 1] = 1;
                this.Map[this.Map.Height - 1, 0] = 1;
                this.Map[this.Map.Height - 1, this.Map.Width - 1] = 1;
            }

            return this;
        }

        public int CountLit() => this.Map.AxisEnumerator().Sum(cell => cell.Value);

        public void Print(int step)
        {
            if (step == 0)
            {
                PuzzleConsole.WriteLine($"Initial state:");
            }
            else
            {
                PuzzleConsole.WriteLine($"After {step} step:");
            }

            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = 0; x < this.Map.Width; x++)
                {
                    PuzzleConsole.Write(this.Map[y, x] == 1 ? "#" : ".");
                }

                PuzzleConsole.WriteLine();
            }

            PuzzleConsole.WriteLine();
        }
    }
}
