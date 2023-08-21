namespace AdventOfCode.Puzzles._2020.Day_20___Jurassic_Jigsaw
{
    using AdventOfCode.Core;

    public class Jigsaw
    {
        public Jigsaw() => this.Pieces = new();

        public int Size { get; private set; } = 10;

        public Dictionary<int, (int y, int x, JigsawPieceVariation piece)> Pieces { get; private set; }

        public int Rows { get; private set; }

        public int Columns { get; private set; }

        public void AddPiece(int y, int x, JigsawPieceVariation variation)
        {
            this.Pieces.Add(variation.Id, (y, x, variation));

            if (y > this.Rows)
            {
                this.Rows = y;
            }

            if (x > this.Columns)
            {
                this.Columns = x;
            }
        }

        public void RemoveEdges()
        {
            this.Size = 8;

            foreach (KeyValuePair<int, (int y, int x, JigsawPieceVariation piece)> piece in this.Pieces)
            {
                int[,] modified = new int[this.Size, this.Size];

                for (int y = 1; y <= this.Size; y++)
                {
                    for (int x = 1; x <= this.Size; x++)
                    {
                        modified[y - 1, x - 1] = piece.Value.piece.Square[y, x];
                    }
                }

                piece.Value.piece.NoBorders = modified;
            }
        }

        public int[,] JoinPieces()
        {
            int[,] result = new int[this.Size * (this.Rows + 1), this.Size * (this.Columns + 1)];

            for (int row = 0; row <= this.Rows; row++)
            {
                for (int column = 0; column <= this.Columns; column++)
                {
                    JigsawPieceVariation piece = this.Pieces.FirstOrDefault(c => c.Value.y == row && c.Value.x == column).Value.piece;

                    for (int y = 0; y < this.Size; y++)
                    {
                        for (int x = 0; x < this.Size; x++)
                        {
                            result[(row * this.Size) + y, (column * this.Size) + x] = piece.NoBorders?[y, x] ?? 0;
                        }
                    }
                }
            }

            return result;
        }

        public void Print(bool gaps = false)
        {
            for (int row = 0; row <= this.Rows; row++)
            {
                for (int y = 0; y < this.Size; y++)
                {
                    for (int column = 0; column <= this.Columns; column++)
                    {
                        JigsawPieceVariation piece = this.Pieces.FirstOrDefault(c => c.Value.y == row && c.Value.x == column).Value.piece;

                        for (int x = 0; x < this.Size; x++)
                        {
                            PuzzleConsole.Write(piece.Square[y, x] == 1 ? "#" : ".");
                        }

                        if (gaps)
                        {
                            PuzzleConsole.Write(" ");
                        }
                    }

                    PuzzleConsole.WriteLine();
                }
            }

            PuzzleConsole.WriteLine();
        }
    }
}
