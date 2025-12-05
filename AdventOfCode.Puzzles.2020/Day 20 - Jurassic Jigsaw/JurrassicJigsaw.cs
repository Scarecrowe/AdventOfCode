namespace AdventOfCode.Puzzles._2020.Day_20___Jurassic_Jigsaw
{
    using AdventOfCode.Core;

    public class JurrassicJigsaw
    {
        private static readonly int[,] SeaMonster = new int[3, 20]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
            { 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 1 },
            { 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0 }
        };

        public JurrassicJigsaw(string[] input) => this.Input = input;

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
    }
}
