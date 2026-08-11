namespace AdventOfCode.Puzzles._2020.Day_20___Jurassic_Jigsaw
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using System.Text;

    public class JurrassicJigsaw
    {
        private static readonly int[,] SeaMonster = new int[3, 20]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
            { 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 1 },
            { 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0 }
        };

        public JurrassicJigsaw(string[] input) => this.Input = input;

        public JurrassicJigsaw(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

        private string[] Input { get; }

        public static void Print(int[,] square, int length)
        {
            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    switch (square[y, x])
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

        public long Corners()
        {
            JigsawSolver solver = JigsawSolver.Parse(this.Input);

            long result = 0;

            foreach (KeyValuePair<int, JigsawPiece> pair in solver.Corners())
            {
                if (result == 0)
                {
                    result = pair.Key;
                    continue;
                }

                result *= pair.Key;
            }

            return result;
        }

        public long NotSeaMonster()
        {
            JigsawSolver solver = JigsawSolver.Parse(this.Input);

            foreach (KeyValuePair<int, JigsawPiece> pair in solver.Corners())
            {
                JigsawPieceVariation topLeft = solver.FindTopLeftVariation(pair.Value);

                Jigsaw jigsaw = solver.Assemble(topLeft);
                jigsaw.RemoveEdges();

                int[,] puzzle = jigsaw.JoinPieces();
                int count = 0;

                JigsawPiece final = new(0, puzzle, 24);

                int total = 0;

                foreach (JigsawPieceVariation variation in final.Variations)
                {
                    count = 0;

                    for (int y = 0; y < 96; y++)
                    {
                        for (int x = 0; x < 96; x++)
                        {
                            total += puzzle[y, x];
                        }
                    }

                    int row = 0;
                    int column = 0;
                    int monsterPartCount = 0;
                    bool parsed = false;

                    while (!parsed)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            for (int x = 0; x < 20; x++)
                            {
                                if (puzzle[row + y, column + x] == 1 && SeaMonster[y, x] == 1)
                                {
                                    monsterPartCount += 1;
                                }
                            }
                        }

                        if (column + 20 < 96)
                        {
                            if (monsterPartCount == 15)
                            {
                                DrawMonster(puzzle, row, column);
                                count++;
                            }

                            monsterPartCount = 0;
                            column++;
                        }
                        else
                        {
                            if (monsterPartCount == 15)
                            {
                                DrawMonster(puzzle, row - 2, column);
                                count++;
                            }

                            if (row + 3 < 96)
                            {
                                monsterPartCount = 0;
                                row++;
                                column = 0;
                                continue;
                            }

                            parsed = true;
                        }
                    }

                    if (count > 0)
                    {
                        return total - (count * 15);
                    }
                }
            }

            return -1;
        }

        public JurrassicJigsaw RenderSilver(int renderEvery = 1)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            JigsawSolver solver = JigsawSolver.Parse(this.Input);
            List<string[]> frames = [];

            foreach (KeyValuePair<int, JigsawPiece> corner in solver.Corners())
            {
                JigsawPieceVariation topLeft = solver.FindTopLeftVariation(corner.Value);
                Jigsaw jigsaw = new();
                jigsaw.AddPiece(0, 0, topLeft);

                frames.Add(this.BuildJigsawFrame(
                    jigsaw,
                    $"JURASSIC JIGSAW // START TILE {topLeft.Id}"));

                int step = 0;

                while (jigsaw.Pieces.Count != solver.Count)
                {
                    bool added = false;

                    foreach (KeyValuePair<int, JigsawPiece> tile in solver)
                    {
                        if (jigsaw.Pieces.ContainsKey(tile.Key))
                        {
                            continue;
                        }

                        foreach (JigsawPieceVariation variation in tile.Value.Variations)
                        {
                            if (jigsaw.Pieces.ContainsKey(tile.Key))
                            {
                                continue;
                            }

                            foreach (KeyValuePair<int, (int y, int x, JigsawPieceVariation variation)> piece in jigsaw.Pieces)
                            {
                                if (variation.TopToBottomEdge(piece.Value.variation))
                                {
                                    jigsaw.AddPiece(piece.Value.y + 1, piece.Value.x, variation);
                                    added = true;
                                    break;
                                }

                                if (variation.BottomToTopEdge(piece.Value.variation))
                                {
                                    jigsaw.AddPiece(piece.Value.y - 1, piece.Value.x, variation);
                                    added = true;
                                    break;
                                }

                                if (variation.LeftToRightEdge(piece.Value.variation))
                                {
                                    jigsaw.AddPiece(piece.Value.y, piece.Value.x + 1, variation);
                                    added = true;
                                    break;
                                }

                                if (variation.RightToLeftEdge(piece.Value.variation))
                                {
                                    jigsaw.AddPiece(piece.Value.y, piece.Value.x - 1, variation);
                                    added = true;
                                    break;
                                }
                            }

                            if (added)
                            {
                                step++;

                                if (step % renderEvery == 0 || jigsaw.Pieces.Count == solver.Count)
                                {
                                    frames.Add(this.BuildJigsawFrame(
                                        jigsaw,
                                        $"JURASSIC JIGSAW // PLACED {jigsaw.Pieces.Count:000}/{solver.Count:000} // TILE {variation.Id}"));
                                }

                                break;
                            }
                        }
                    }
                }

                for (int i = 0; i < 16; i++)
                {
                    frames.Add(this.BuildJigsawFrame(
                        jigsaw,
                        $"IMAGE ASSEMBLED // CORNER PRODUCT {this.Corners()}"));
                }

                this.RenderPaddedFrames(frames);
                return this;
            }

            return this;
        }

        public JurrassicJigsaw RenderGold(int renderEvery = 8)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            JigsawSolver solver = JigsawSolver.Parse(this.Input);
            List<string[]> frames = [];

            foreach (KeyValuePair<int, JigsawPiece> corner in solver.Corners())
            {
                JigsawPieceVariation topLeft = solver.FindTopLeftVariation(corner.Value);
                Jigsaw jigsaw = solver.Assemble(topLeft);

                frames.Add(this.BuildJigsawFrame(jigsaw, "FULL IMAGE WITH TILE BORDERS"));

                jigsaw.RemoveEdges();

                frames.Add(this.BuildJigsawFrame(jigsaw, "BORDERS REMOVED // TRUE IMAGE DATA"));

                int[,] joined = jigsaw.JoinPieces();
                int size = joined.GetLength(0);

                JigsawPiece image = new(0, joined, size);

                foreach (JigsawPieceVariation variation in image.Variations)
                {
                    int[,] scan = (int[,])variation.Square.Clone();
                    int monsters = 0;
                    int step = 0;

                    for (int y = 0; y <= size - 3; y++)
                    {
                        for (int x = 0; x <= size - 20; x++)
                        {
                            bool monster = this.IsMonster(scan, y, x);

                            if (monster)
                            {
                                DrawMonster(scan, y, x);
                                monsters++;
                            }

                            step++;

                            if (step % renderEvery == 0)
                            {
                                frames.Add(this.BuildImageFrame(
                                    scan,
                                    $"SCANNING FOR SEA MONSTERS // FOUND {monsters:000}"));
                            }
                        }
                    }

                    if (monsters > 0)
                    {
                        int roughness = this.CountHashes(scan);

                        for (int i = 0; i < 24; i++)
                        {
                            frames.Add(this.BuildImageFrame(
                                scan,
                                $"SEA MONSTERS FOUND {monsters:000} // WATER ROUGHNESS {roughness}"));
                        }

                        this.RenderPaddedFrames(frames);
                        return this;
                    }
                }
            }

            this.RenderPaddedFrames(frames);
            return this;
        }

        private bool IsMonster(int[,] puzzle, int row, int column)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 20; x++)
                {
                    if (SeaMonster[y, x] == 1 && puzzle[row + y, column + x] != 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private int CountHashes(int[,] puzzle)
        {
            int total = 0;

            for (int y = 0; y < puzzle.GetLength(0); y++)
            {
                for (int x = 0; x < puzzle.GetLength(1); x++)
                {
                    if (puzzle[y, x] == 1)
                    {
                        total++;
                    }
                }
            }

            return total;
        }

        private static void DrawMonster(int[,] puzzle, int row, int column)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 20; x++)
                {
                    if (SeaMonster[y, x] == 1)
                    {
                        puzzle[row + y, column + x] = 2;
                    }
                }
            }
        }

        private string[] BuildJigsawFrame(Jigsaw jigsaw, string title)
        {
            List<string> result = [];

            result.Add(title);
            result.Add(string.Empty);

            int minY = jigsaw.Pieces.Min(p => p.Value.y);
            int maxY = jigsaw.Pieces.Max(p => p.Value.y);
            int minX = jigsaw.Pieces.Min(p => p.Value.x);
            int maxX = jigsaw.Pieces.Max(p => p.Value.x);

            for (int row = minY; row <= maxY; row++)
            {
                for (int y = 0; y < jigsaw.Size; y++)
                {
                    StringBuilder sb = new();

                    for (int column = minX; column <= maxX; column++)
                    {
                        JigsawPieceVariation? piece = jigsaw.Pieces
                            .FirstOrDefault(c => c.Value.y == row && c.Value.x == column)
                            .Value.piece;

                        if (piece == null)
                        {
                            sb.Append(new string(' ', jigsaw.Size));
                        }
                        else
                        {
                            int[,] square = jigsaw.Size == 8 ? piece.NoBorders : piece.Square;

                            for (int x = 0; x < jigsaw.Size; x++)
                            {
                                sb.Append(square[y, x] == 1 ? '#' : '.');
                            }
                        }

                        sb.Append(' ');
                    }

                    result.Add(sb.ToString());
                }

                result.Add(string.Empty);
            }

            return [.. result];
        }

        private string[] BuildImageFrame(int[,] image, string title)
        {
            List<string> result = [];

            result.Add(title);
            result.Add(string.Empty);

            for (int y = 0; y < image.GetLength(0); y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < image.GetLength(1); x++)
                {
                    sb.Append(image[y, x] switch
                    {
                        0 => '.',
                        1 => '#',
                        2 => 'O',
                        _ => '?'
                    });
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
                this.Renderer?.RenderFrame(new Frame(PadFrame(frame, width, height)));
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
    }
}
