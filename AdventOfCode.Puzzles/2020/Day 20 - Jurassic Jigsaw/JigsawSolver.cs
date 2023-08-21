namespace AdventOfCode.Puzzles._2020.Day_20___Jurassic_Jigsaw
{
    using AdventOfCode.Core.Extensions;

    public class JigsawSolver : Dictionary<int, JigsawPiece>
    {
        public static JigsawSolver Parse(string[] input)
        {
            JigsawSolver tiles = new();

            int y = 0;
            int tileId = 0;
            int[,] squares = new int[10, 10];

            foreach (string line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    tiles.Add(tileId, new(tileId, squares));
                    continue;
                }

                if (line[0] == 'T')
                {
                    squares = new int[10, 10];
                    tileId = line.SplitSpace()[1].Replace(":").ToInt();
                    y = 0;
                    continue;
                }

                for (int x = 0; x < line.Length; x++)
                {
                    if (line[x] == '#')
                    {
                        squares[y, x] = 1;
                    }
                }

                y++;
            }

            return tiles;
        }

        public Dictionary<int, JigsawPiece> Corners()
        {
            Dictionary<int, JigsawPiece> pieces = new();

            foreach (KeyValuePair<int, JigsawPiece> pairA in this)
            {
                JigsawPiece pieceA = pairA.Value;
                int count = 0;
                List<(int idA, int idB)> processed = new();

                foreach (KeyValuePair<int, JigsawPiece> pairB in this)
                {
                    JigsawPiece pieceB = pairB.Value;

                    if (pieceA.Id != pieceB.Id)
                    {
                        if (processed.Contains((pieceA.Id, pieceB.Id)) || processed.Contains((pieceB.Id, pieceA.Id)))
                        {
                            continue;
                        }

                        foreach (JigsawPieceVariation variationA in pieceA.Variations)
                        {
                            bool match = false;

                            foreach (JigsawPieceVariation variationB in pieceB.Variations)
                            {
                                if (processed.Contains((pieceA.Id, pieceB.Id)) || processed.Contains((pieceB.Id, pieceA.Id)))
                                {
                                    break;
                                }

                                if (variationA.TopToBottomEdge(variationB))
                                {
                                    match = true;
                                    break;
                                }

                                if (variationA.BottomToTopEdge(variationB))
                                {
                                    match = true;
                                    break;
                                }

                                if (variationA.LeftToRightEdge(variationB))
                                {
                                    match = true;
                                    break;
                                }

                                if (variationA.RightToLeftEdge(variationB))
                                {
                                    match = true;
                                    break;
                                }
                            }

                            if (match)
                            {
                                count++;
                            }

                            processed.Add((pieceA.Id, pieceB.Id));
                        }
                    }

                    if (count > 2)
                    {
                        break;
                    }
                }

                if (count <= 2)
                {
                    pieces.Add(pairA.Key, pairA.Value);
                }
            }

            return pieces;
        }

        public JigsawPieceVariation FindTopLeftVariation(JigsawPiece corner)
        {
            foreach (JigsawPieceVariation variationA in corner.Variations)
            {
                bool match = false;

                foreach (KeyValuePair<int, JigsawPiece> pair in this)
                {
                    JigsawPiece piece = pair.Value;

                    if (corner.Id != piece.Id)
                    {
                        foreach (JigsawPieceVariation variationB in piece.Variations)
                        {
                            if (variationA.TopToBottomEdge(variationB))
                            {
                                match = true;
                                break;
                            }

                            if (variationA.LeftToRightEdge(variationB))
                            {
                                match = true;
                                break;
                            }
                        }

                        if (match)
                        {
                            break;
                        }
                    }
                }

                if (!match)
                {
                    return variationA;
                }
            }

            throw new InvalidOperationException();
        }

        public Jigsaw Assemble(JigsawPieceVariation start)
        {
            Jigsaw jigsaw = new();
            jigsaw.AddPiece(0, 0, start);

            while (jigsaw.Pieces.Count != this.Count)
            {
                foreach (KeyValuePair<int, JigsawPiece> tile in this)
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
                                break;
                            }

                            if (variation.BottomToTopEdge(piece.Value.variation))
                            {
                                jigsaw.AddPiece(piece.Value.y - 1, piece.Value.x, variation);
                                break;
                            }

                            if (variation.LeftToRightEdge(piece.Value.variation))
                            {
                                jigsaw.AddPiece(piece.Value.y, piece.Value.x + 1, variation);
                                break;
                            }

                            if (variation.RightToLeftEdge(piece.Value.variation))
                            {
                                jigsaw.AddPiece(piece.Value.y, piece.Value.x - 1, variation);
                                break;
                            }
                        }

                        if (jigsaw.Pieces.Count == this.Count)
                        {
                            return jigsaw;
                        }
                    }
                }
            }

            return jigsaw;
        }
    }
}
