namespace AdventOfCode.Puzzles._2017.Day_21___Fractal_Art
{
    using AdventOfCode.Core;

    public class Pattern : VectorArray<int, int>
    {
        public Pattern(string[] input)
            : base(input, (c) => c == '#' ? 1 : 0)
        {
        }

        public void Enhance(Dictionary<string, int[,]> rules)
        {
            int size = this.Height % 2 == 0 ? 2 : 3;
            int columns = this.Width / size;
            int rows = this.Height / size;
            this.Join(size, rows, columns, this.Split(size, rows, columns), rules);
        }

        public int PixelCount() => this.AxisEnumerator().Sum(x => x.Value);

        private List<string> Split(int size, int rows, int columns)
        {
            List<string> result = new();

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    int[,] map = new int[size, size];
                    int yIndex = 0;

                    for (int y = row * size; y < (row * size) + size; y++)
                    {
                        int xIndex = 0;

                        for (int x = column * size; x < (column * size) + size; x++)
                        {
                            map[yIndex, xIndex] = this[y, x];
                            xIndex++;
                        }

                        yIndex++;
                    }

                    result.Add(FractalArt.CreateKey(map));
                }
            }

            return result;
        }

        private void Join(int size, int rows, int columns, List<string> cells, Dictionary<string, int[,]> rules)
        {
            size++;
            this.Resize(columns * size, rows * size);
            this.Clear();
            int column = 0;
            int row = 0;

            foreach (string key in cells)
            {
                int[,] output = rules[key];

                int yIndex = 0;
                for (int y = row * size; y < (row * size) + size; y++)
                {
                    int xIndex = 0;

                    for (int x = column * size; x < (column * size) + size; x++)
                    {
                        this[y, x] = output[yIndex, xIndex];
                        xIndex++;
                    }

                    yIndex++;
                }

                column++;

                if (column == columns)
                {
                    column = 0;
                    row++;
                }
            }
        }
    }
}
