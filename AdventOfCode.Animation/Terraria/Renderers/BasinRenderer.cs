namespace AdventOfCode.Animation.Terraria.Renderers
{
    using System.Drawing;
    using AdventOfCode.Animation.Terraria.Basins;
    using AdventOfCode.Animation.Terraria.Biomes;
    using AdventOfCode.Core;

    public class BasinRenderer
    {
        public BasinRenderer(
            List<Basin> basins,
            Dictionary<BiomeType, Biome> biomes)
        {
            this.Basins = basins;
            this.Biomes = biomes;
        }

        public List<Basin> Basins { get; }

        public Dictionary<BiomeType, Biome> Biomes { get; }

        public void RenderWalls(Graphics graphics)
        {
            Console.WriteLine("Rendering basin walls");

            foreach (Basin basin in this.Basins.OrderByDescending(x => x.Width))
            {
                Biome biome = this.Biomes[basin.BiomeType];

                int index = RandomGenerator.Next(0, biome.BasinWalls.Count - 1);

                WallTileset tileset = biome.BasinWalls[index];
                long start = Math.Max(basin.Top.Left.Y, basin.Top.Right.Y);

                for (long y = start; y < Math.Max(basin.Top.Left.Y, basin.Top.Right.Y) + basin.Height; y++)
                {
                    for (long x = basin.Top.Left.X + 1; x <= basin.Top.Left.X + (basin.Width - 1); x++)
                    {
                        if(y == start)
                        {
                            graphics.DrawImage(tileset.Top(RandomGenerator.Next(0, 2)), x * 16, y * 16);
                        }
                        else
                        {
                            graphics.DrawImage(tileset.Middle(RandomGenerator.Next(0, 2)), x * 16, y * 16);
                        }
                    }
                }
            }
        }

        public void RenderBlocks(Graphics graphics)
        {
            Console.WriteLine("Rendering basin blocks");

            int clayTopIndex = 1;
            int clayBottomIndex = 1;
            int clayLeftIndex = 0;
            int clayRightIndex = 0;
            int clayMiddleIndex = 1;

            foreach (Basin basin in this.Basins)
            {
                IBlockTileset tileset = this.Biomes[basin.BiomeType].BasinTiles[RandomGenerator.Next(0, this.Biomes[basin.BiomeType].BasinTiles.Count - 1)];

                foreach (KeyValuePair<Vector<long>, BasinTileType> pair in basin.Blocks)
                {
                    long x = pair.Key.X * 16;
                    long y = pair.Key.Y * 16;

                    switch (pair.Value)
                    {
                        case BasinTileType.BottomLeft:
                            graphics.DrawImage(tileset.BottomLeft(), x, y);
                            break;
                        case BasinTileType.BottomRight:
                            graphics.DrawImage(tileset.BottomRight(), x, y);
                            break;
                        case BasinTileType.Top:
                            graphics.DrawImage(tileset.Top(clayTopIndex), x, y);

                            clayTopIndex++;
                            if (clayTopIndex >= 3)
                            {
                                clayTopIndex = 1;
                            }

                            break;
                        case BasinTileType.Bottom:
                            graphics.DrawImage(tileset.Bottom(clayBottomIndex), x, y);

                            clayBottomIndex++;
                            if (clayBottomIndex >= 3)
                            {
                                clayBottomIndex = 1;
                            }

                            break;
                        case BasinTileType.TopLeft:
                            graphics.DrawImage(tileset.TopLeft(), x, y);
                            break;
                        case BasinTileType.TopRight:
                            graphics.DrawImage(tileset.TopRight(), x, y);
                            break;
                        case BasinTileType.Left:
                            graphics.DrawImage(tileset.Left(clayLeftIndex), x, y);

                            clayLeftIndex++;
                            if (clayLeftIndex > 2)
                            {
                                clayLeftIndex = 0;
                            }

                            break;
                        case BasinTileType.Right:
                            graphics.DrawImage(tileset.Right(clayRightIndex), x, y);

                            clayRightIndex++;
                            if (clayRightIndex > 2)
                            {
                                clayRightIndex = 0;
                            }

                            break;
                        case BasinTileType.Middle:
                            graphics.DrawImage(tileset.Middle(clayMiddleIndex), x, y);

                            clayMiddleIndex++;
                            if (clayMiddleIndex > 3)
                            {
                                clayMiddleIndex = 1;
                            }

                            break;
                    }
                }
            }
        }
    }
}
