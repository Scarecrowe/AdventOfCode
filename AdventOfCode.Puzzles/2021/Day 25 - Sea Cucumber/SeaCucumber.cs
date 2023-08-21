namespace AdventOfCode.Puzzles._2021.Day_25___Sea_Cucumber
{
    using AdventOfCode.Core;

    public class SeaCucumber
    {
        public SeaCucumber(string[] input) => this.Map = new(input, (c) => ".>v".IndexOf(c));

        public VectorArray<int, int> Map { get; private set; }

        public SeaCucumber Print()
        {
            string chars = ".>v";

            for (int y = 0; y < this.Map.Height; y++)
            {
                for (int x = 0; x < this.Map.Width; x++)
                {
                    PuzzleConsole.Write(chars[this.Map[y, x]]);
                }

                PuzzleConsole.WriteLine();
            }

            PuzzleConsole.WriteLine();

            return this;
        }

        public int Run()
        {
            int step = 0;
            bool moved = true;

            while (moved)
            {
                moved = false;

                VectorArray<int, int> state = this.Map.Clone();

                foreach (VectorCell<int, int> cell in this.Map.AxisEnumerator())
                {
                    if (this.Map[cell.Point] == 1 && this.Map[cell.Point.Y, (cell.Point.X + 1) % this.Map.Width] == 0)
                    {
                        state[cell.Point] = 0;
                        state[cell.Point.Y, (cell.Point.X + 1) % this.Map.Width] = 1;
                        moved = true;
                    }
                }

                this.Map = state.Clone();

                foreach (VectorCell<int, int> cell in this.Map.AxisEnumerator())
                {
                    if (this.Map[cell.Point] == 2 && this.Map[(cell.Point.Y + 1) % this.Map.Height, cell.Point.X] == 0)
                    {
                        state[cell.Point] = 0;
                        state[(cell.Point.Y + 1) % this.Map.Height, cell.Point.X] = 2;
                        moved = true;
                    }
                }

                this.Map = state.Clone();

                step++;
            }

            return step;
        }
    }
}
