namespace AdventOfCode.Animation.Terraria.Basins
{
    using System.Drawing;
    using AdventOfCode.Animation.Terraria.Biomes;
    using AdventOfCode.Animation.Terraria.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class Basin
    {
        public Basin()
        {
            this.Blocks = new();
            this.IsBasin = true;
            this.Width = 0;
            this.Height = 0;
            this.Top = (new(), new());
            this.Bottom = (new(), new());
        }

        public BiomeType BiomeType { get; private set; }

        public Dictionary<Vector<long>, BasinTileType> Blocks { get; }

        public bool IsBasin { get; private set; }

        public long Width { get; private set; }

        public long Height { get; private set; }

        public bool Intersects { get; private set; }

        public (Vector<long> Left, Vector<long> Right) Top { get; private set; }

        public (Vector<long> Left, Vector<long> Right) Bottom { get; private set; }

        public static void AddAnswerBasin(
            ReservoirResearch puzzle,
            List<string> input,
            Screen screen)
        {
            long basinMaxY = puzzle.ClayMax.Y + 30;
            int clayMinX = puzzle.ClayMin.X.ToInt();
            int scrollTileX = screen.Point.X.ToInt() / 16;
            int left = clayMinX + scrollTileX - 1;
            int right = left + (screen.Width / 16);
            int length = right - left;
            int width = (int)(length * 0.6);
            int lavaWidth = (length - width) / 2;

            for (int x = left + lavaWidth; x <= left + lavaWidth + width; x++)
            {
                input.Add($"x={x}, y={basinMaxY}..{basinMaxY + 40}");
            }
        }

        public static void AddFinalBasin(ReservoirResearch puzzle, List<string> input)
        {
            int clayMaxY = puzzle.ClayMax.Y.ToInt();
            int clayMinX = puzzle.ClayMin.X.ToInt() + 2;
            int clayMaxX = puzzle.ClayMax.X.ToInt() - 2;
            int basinMaxY = clayMaxY + 15;
            int bottom = clayMaxY + 75;

            input.Add($"x={clayMinX}, y={basinMaxY}..{bottom}");
            input.Add($"x={clayMaxX}, y={basinMaxY}..{bottom}");
            input.Add($"y={bottom}, x={clayMinX}..{clayMaxX}");
        }

        public static List<Basin> Parse(
            VectorArray<long, EntityType> map,
            Dictionary<BiomeType, Biome> biomes)
        {
            List<Basin> result = new();
            HashSet<Vector<long>> processed = new();
            Basin? basin = null;

            foreach (VectorCell<long, EntityType> cell in map.Values(EntityType.Clay))
            {
                if (processed.Contains(cell.Point))
                {
                    continue;
                }

                if (basin == null)
                {
                    basin = new Basin();
                    result.Add(basin);
                }

                ParseBasin(processed, map, basin, cell);
                basin.Update();

                if (basin.Blocks.ElementAt(0).Key.Y == 1)
                {
                    result.Remove(basin);
                    basin = null;
                    continue;
                }

                basin.BiomeType = Biome.GetBiomeType(biomes, Math.Min(basin.Top.Left.Y, basin.Top.Right.Y), WaterFallRenderer.IntroLength, map.Height);
                basin = null;
            }

            foreach(Basin basinA in result)
            {
                foreach (Basin basinB in result)
                {
                    if (basinA != basinB)
                    {
                        Rectangle regionA = basinA.Region();
                        Rectangle regionB = basinB.Region();

                        if (regionA.IntersectsWith(regionB))
                        {
                            if (regionA.Width > regionB.Width)
                            {
                                basinB.Intersects = true;
                            }
                            else
                            {
                                basinA.Intersects = true;
                            }
                        }
                    }
                }
            }

            return result;
        }

        public static BasinTileType GetBasinTileType(VectorArray<long, EntityType> map, VectorCell<long, EntityType> cell)
        {
            if (cell.Value != EntityType.Clay)
            {
                return BasinTileType.None;
            }

            var adjacent = map.AdjacentCardinal(cell.Point);

            BasinTileType result = IsTopLeftOrRight(map, adjacent, cell);

            if (result != BasinTileType.None)
            {
                return result;
            }

            result = IsLeftOrRight(map, adjacent, cell);

            if (result != BasinTileType.None)
            {
                return result;
            }

            result = IsBottomLeftOrRight(map, adjacent, cell);

            if (result != BasinTileType.None)
            {
                return result;
            }

            return IsBottom(adjacent);
        }

        public Rectangle Region()
        {
            long minY = Math.Min(this.Top.Left.Y, this.Top.Right.Y);

            return new((int)this.Top.Left.X, (int)minY, (int)this.Top.Right.X - (int)this.Top.Left.X, (int)this.Bottom.Left.Y - (int)minY);
        }

        private static void ParseBasin(HashSet<Vector<long>> processed, VectorArray<long, EntityType> map, Basin basin, VectorCell<long, EntityType> cell)
        {
            BasinTileType basinTileType = GetBasinTileType(map, cell);
            basin.Blocks.Add(cell.Point, basinTileType);
            processed.Add(cell.Point);

            var adjacent = map.AdjacentCardinal(cell.Point);

            if (basinTileType == BasinTileType.TopLeft
                && adjacent.First(x => x.Direction == Cardinal.East).Value == EntityType.Clay)
            {
                basin.IsBasin = false;
            }

            foreach (VectorCell<long, EntityType> linkedCell in adjacent.Where(x => x.Value == EntityType.Clay))
            {
                if (processed.Contains(linkedCell.Point))
                {
                    continue;
                }

                ParseBasin(processed, map, basin, linkedCell);
            }
        }

        private static BasinTileType IsTopLeftOrRight(VectorArray<long, EntityType> map, IEnumerable<VectorCell<long, EntityType>> adjacent, VectorCell<long, EntityType> cell)
        {
            if (adjacent.Any(x => x.Direction == Cardinal.North && x.Value != EntityType.Clay)
                && adjacent.Any(x => x.Direction == Cardinal.South && x.Value == EntityType.Clay))
            {
                long y = cell.Point.Y;

                while (true)
                {
                    y++;
                    EntityType value = map[new(cell.Point.X, y)];
                    if (value != EntityType.Clay)
                    {
                        value = map[new(cell.Point.X - 1, y - 1)];
                        if (value == EntityType.Clay)
                        {
                            return BasinTileType.TopRight;
                        }

                        value = map[new(cell.Point.X + 1, y - 1)];
                        if (value == EntityType.Clay)
                        {
                            return BasinTileType.TopLeft;
                        }
                    }
                }
            }

            return BasinTileType.None;
        }

        private static BasinTileType IsLeftOrRight(VectorArray<long, EntityType> map, IEnumerable<VectorCell<long, EntityType>> adjacent, VectorCell<long, EntityType> cell)
        {
            if (adjacent.Any(x => x.Direction == Cardinal.North && x.Value == EntityType.Clay)
                && adjacent.Any(x => x.Direction == Cardinal.South && x.Value == EntityType.Clay))
            {
                long y = cell.Point.Y;

                while (true)
                {
                    y++;
                    EntityType value = map[new(cell.Point.X, y)];
                    if (value != EntityType.Clay)
                    {
                        value = map[new(cell.Point.X - 1, y - 1)];
                        if (value == EntityType.Clay)
                        {
                            return BasinTileType.Right;
                        }

                        value = map[new(cell.Point.X + 1, y - 1)];
                        if (value == EntityType.Clay)
                        {
                            return BasinTileType.Left;
                        }
                    }
                }
            }

            return BasinTileType.None;
        }

        private static BasinTileType IsBottomLeftOrRight(VectorArray<long, EntityType> map, IEnumerable<VectorCell<long, EntityType>> adjacent, VectorCell<long, EntityType> cell)
        {
            if (adjacent.Any(x => x.Direction == Cardinal.North && x.Value == EntityType.Clay)
                && adjacent.Any(x => x.Direction == Cardinal.South && x.Value != EntityType.Clay))
            {
                EntityType value = map[new(cell.Point.X - 1, cell.Point.Y)];
                if (value != EntityType.Clay)
                {
                    return BasinTileType.BottomLeft;
                }

                value = map[new(cell.Point.X + 1, cell.Point.Y)];
                if (value != EntityType.Clay)
                {
                    return BasinTileType.BottomRight;
                }
            }

            return BasinTileType.None;
        }

        private static BasinTileType IsBottom(IEnumerable<VectorCell<long, EntityType>> adjacent)
        {
            if (adjacent.Any(x => x.Direction == Cardinal.East && x.Value == EntityType.Clay)
                         && adjacent.Any(x => x.Direction == Cardinal.West && x.Value == EntityType.Clay))
            {
                return BasinTileType.Bottom;
            }

            return BasinTileType.None;
        }

        private void FillBasin()
        {
            if (this.IsBasin)
            {
                return;
            }

            for (long y = this.Top.Left.Y + 1; y <= this.Bottom.Left.Y - 1; y++)
            {
                for (long x = this.Top.Left.X + 1; x <= this.Top.Right.X - 1; x++)
                {
                    this.Blocks.TryAddValue(new(x, y), BasinTileType.Middle);
                }
            }

            for (long x = this.Top.Left.X + 1; x <= this.Top.Right.X - 1; x++)
            {
                this.Blocks[new(x, this.Top.Left.Y)] = BasinTileType.Top;
            }
        }

        private void Update()
        {
            foreach (KeyValuePair<Vector<long>, BasinTileType> pair in this.Blocks)
            {
                switch (pair.Value)
                {
                    case BasinTileType.TopLeft:
                        this.Top.Left.X = pair.Key.X;
                        this.Top.Left.Y = pair.Key.Y;
                        break;
                    case BasinTileType.TopRight:
                        this.Top.Right.X = pair.Key.X;
                        this.Top.Right.Y = pair.Key.Y;
                        break;
                    case BasinTileType.BottomLeft:
                        this.Bottom.Left.X = pair.Key.X;
                        this.Bottom.Left.Y = pair.Key.Y;
                        break;
                    case BasinTileType.BottomRight:
                        this.Bottom.Right.X = pair.Key.X;
                        this.Bottom.Right.Y = pair.Key.Y;
                        break;
                }
            }

            long minY = Math.Min(this.Top.Left.Y, this.Top.Right.Y);

            this.Width = Math.Abs(this.Top.Right.X - this.Top.Left.X);
            this.Height = Math.Abs(this.Bottom.Left.Y - minY);

            this.FillBasin();
        }
    }
}
