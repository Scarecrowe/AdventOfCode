namespace AdventOfCode.Puzzles._2020.Day_20___Jurassic_Jigsaw
{
    using AdventOfCode.Core;

    public class JigsawPieceVariation
    {
        public JigsawPieceVariation(int id, int[,] square, Orientation orientation, int size = 10)
        {
            this.Id = id;
            this.Square = square;
            this.Orientation = orientation;
            this.Size = size;
            this.NoBorders = new int[0, 0];
        }

        public int Size { get; }

        public int[,] Square { get; }

        public int[,] NoBorders { get; set; }

        public int Id { get; }

        public Orientation Orientation { get; }

        public bool TopToBottomEdge(JigsawPieceVariation variationB)
        {
            for (int x = 0; x < this.Size; x++)
            {
                if (this.Square[0, x] != variationB.Square[this.Size - 1, x])
                {
                    return false;
                }
            }

            return true;
        }

        public bool BottomToTopEdge(JigsawPieceVariation variationB)
        {
            for (int x = 0; x < this.Size; x++)
            {
                if (this.Square[this.Size - 1, x] != variationB.Square[0, x])
                {
                    return false;
                }
            }

            return true;
        }

        public bool RightToLeftEdge(JigsawPieceVariation variationB)
        {
            for (int y = 0; y < this.Size; y++)
            {
                if (this.Square[y, this.Size - 1] != variationB.Square[y, 0])
                {
                    return false;
                }
            }

            return true;
        }

        public bool LeftToRightEdge(JigsawPieceVariation variationB)
        {
            for (int y = 0; y < this.Size; y++)
            {
                if (this.Square[y, 0] != variationB.Square[y, this.Size - 1])
                {
                    return false;
                }
            }

            return true;
        }

        public void Print()
        {
            for (int y = 0; y < this.Size; y++)
            {
                for (int x = 0; x < this.Size; x++)
                {
                    switch (this.Square[y, x])
                    {
                        case 0:
                            PuzzleConsole.Write(".");
                            break;
                        case 1:
                            PuzzleConsole.Write("#");
                            break;
                        case 2:
                            PuzzleConsole.Write("0");
                            break;
                    }
                }

                PuzzleConsole.WriteLine();
            }

            PuzzleConsole.WriteLine();
        }
    }
}
