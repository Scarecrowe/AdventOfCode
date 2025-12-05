namespace AdventOfCode.Puzzles._2020.Day_20___Jurassic_Jigsaw
{
    using AdventOfCode.Core.Extensions;

    public class JigsawPiece
    {
        public JigsawPiece(int id, int[,] squares, int size = 10)
        {
            this.Id = id;

            this.Variations = new()
            {
                new(this.Id, (int[,])squares.Clone(), Orientation.Initial, size),
                new(this.Id, squares.FlipHorizontally(size), Orientation.FlippedHorizontally, size),
                new(this.Id, squares.FlipVertically(size), Orientation.FlippedVertically, size)
            };

            int[,] rotated = squares.RotateClockWise(size);
            this.Variations.Add(new(this.Id, rotated, Orientation.Rotated90, size));
            this.Variations.Add(new(this.Id, rotated.FlipHorizontally(size), Orientation.Rotated90FlippedHorizontally, size));
            this.Variations.Add(new(this.Id, rotated.FlipVertically(size), Orientation.Rotated90FlippedVertically, size));

            rotated = rotated.RotateClockWise(size);
            this.Variations.Add(new(this.Id, rotated, Orientation.Rotated180, size));

            rotated = rotated.RotateClockWise(size);
            this.Variations.Add(new(this.Id, rotated, Orientation.Rotated270, size));
        }

        public int Id { get; }

        public List<JigsawPieceVariation> Variations { get; }
    }
}
