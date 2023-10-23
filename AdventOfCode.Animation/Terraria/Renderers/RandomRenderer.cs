namespace AdventOfCode.Animation.Terraria.Renderers
{
    using System.Drawing;
    using AdventOfCode.Animation.Terraria.Basins;
    using AdventOfCode.Animation.Terraria.Biomes;
    using AdventOfCode.Animation.Terraria.Random;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class RandomRenderer
    {
        public RandomRenderer(
            List<Basin> basins,
            Dictionary<BiomeType, Biome> biomes)
        {
            this.Basins = basins;
            this.Biomes = biomes;
        }

        public List<Basin> Basins { get; }

        public Dictionary<BiomeType, Biome> Biomes { get; }

        public void RenderCrystals(Graphics graphics)
        {
            Tileset crystals = new(Terraria.GetImage("Random\\Tiles_129.png"), 16, 16, new(2, 2));

            foreach (Basin basin in this.Basins)
            {
                if (basin.BiomeType != BiomeType.Hallowed)
                {
                    continue;
                }

                foreach (KeyValuePair<Vector<long>, BasinTileType> block in basin.Blocks)
                {
                    long x = block.Key.X * 16;
                    long y = block.Key.Y * 16;

                    if (RandomGenerator.NextDouble() > 0.2)
                    {
                        continue;
                    }

                    int value = RandomGenerator.Next(0, 17);

                    switch (block.Value)
                    {
                        case BasinTileType.TopLeft:
                            graphics.DrawImage(crystals[new(value, 0)], x, y - 16);
                            break;
                        case BasinTileType.TopRight:
                            graphics.DrawImage(crystals[new(value, 0)], x, y - 16);
                            break;
                        case BasinTileType.Left:
                            graphics.DrawImage(crystals[new(value, 2)], x - 16, y);
                            break;
                        case BasinTileType.Right:
                            graphics.DrawImage(crystals[new(value, 3)], x + 16, y);
                            break;
                        case BasinTileType.Bottom:
                            graphics.DrawImage(crystals[new(value, 1)], x, y + 16);
                            break;
                        case BasinTileType.BottomLeft:
                            graphics.DrawImage(crystals[new(value, 2)], x - 16, y);
                            break;
                        case BasinTileType.BottomRight:
                            graphics.DrawImage(crystals[new(value, 3)], x + 16, y);
                            break;
                    }
                }
            }
        }

        public void RenderRandomAssets(Graphics graphics, VectorArray<long, EntityType> map)
        {
            HashSet<Vector<int>> rendered = new();

            foreach (Basin basin in this.Basins)
            {
                Biome biome = this.Biomes[basin.BiomeType];

                bool hasCaptured = false;

                foreach (KeyValuePair<Vector<long>, BasinTileType> block in basin.Blocks)
                {
                    if (rendered.Contains(new Vector<int>(block.Key.X, block.Key.Y - 1)))
                    {
                        continue;
                    }

                    if ((block.Value == BasinTileType.Bottom && basin.IsBasin)
                        || (block.Value == BasinTileType.Top && !basin.IsBasin)
                        || (block.Value == BasinTileType.TopLeft && !basin.IsBasin)
                        || (block.Value == BasinTileType.TopRight && !basin.IsBasin))
                    {
                        double value = RandomGenerator.NextDouble();
                        IRandomAsset? randomAsset = biome.GetRandomAssetBy(value);

                        if (randomAsset == null || (randomAsset.RandomAssetType == RandomAssetType.Captured && hasCaptured))
                        {
                            randomAsset = biome.GetRandomAsset(RandomAssetType.Grass);

                            if (randomAsset == null)
                            {
                                continue;
                            }
                        }

                        bool result = randomAsset.Render(graphics, map, rendered, block.Key);

                        if (result && randomAsset.RandomAssetType == RandomAssetType.Captured)
                        {
                            hasCaptured = true;
                            continue;
                        }

                        if (!result)
                        {
                            randomAsset = biome.GetRandomAsset(RandomAssetType.Grass);

                            if (randomAsset == null)
                            {
                                continue;
                            }

                            randomAsset.Render(graphics, map, rendered, block.Key);
                        }
                    }
                }
            }
        }

        public void Render(Graphics graphics, VectorArray<long, EntityType> map)
        {
            Console.WriteLine("Rendering Random");
            this.RenderRandomAssets(graphics, map);
            this.RenderCrystals(graphics);
        }
    }
}
