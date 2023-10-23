namespace AdventOfCode.Animation.Terraria.Renderers
{
    using System.Drawing;
    using AdventOfCode.Animation.Extensions;
    using AdventOfCode.Animation.Terraria.Biomes;
    using AdventOfCode.Animation.Terraria.Liquids;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class LiquidRenderer
    {
        public const double TransitionSpeed = 0.007;
        public const float Opacity = 0.6f;

        public LiquidRenderer(ReservoirResearch puzzle, Screen screen, Dictionary<BiomeType, Biome> biomes)
        {
            this.Puzzle = puzzle;
            this.Screen = screen;
            this.Biomes = biomes;
            this.OpacityA = Opacity;
            this.OpacityB = 0.0f;
            this.CurrentBiomeType = BiomeType.Overworld;
            this.TopIndex = 0;
            this.FlowIndex = 2;
            this.IntroLiquid = new(LiquidType.Water, new(Terraria.GetImage("Liquids\\water_0.png"), 16, 16), new(Terraria.GetImage("Liquids\\liquid_0.png"), 16, 16, new(2, 0)), Opacity);
        }

        private ReservoirResearch Puzzle { get; }

        private Screen Screen { get; }

        private Dictionary<BiomeType, Biome> Biomes { get; }

        private double OpacityA { get; set; }

        private double OpacityB { get; set; }

        private bool IsTransition { get; set; }

        private BiomeType BiomeTypeA { get; set; }

        private BiomeType BiomeTypeB { get; set; }

        private BiomeType CurrentBiomeType { get; set; }

        private int TopIndex { get; set; }

        private int FlowIndex { get; set; }

        private int FlowSkip { get; set; }

        private Liquid IntroLiquid { get; }

        private Image? ImageA { get; set; }

        private Image? ImageB { get; set; }

        public void Render(Graphics graphics)
        {
            if (this.Screen.CountOfWater(this.Puzzle.Map) == 0)
            {
                this.RenderIntroLiquid(graphics);
                return;
            }

            this.RenderLiquid(graphics);
        }

        private static void RenderLiquid(Graphics graphics, double opacity, Image image, Vector<long> point)
            => graphics.DrawImageWithOpacity((float)opacity, image, new Rectangle(point.X.ToInt(), point.Y.ToInt(), 16, 16));

        private void RenderIntroLiquid(Graphics graphics)
        {
            Vector<long> stream = new((WaterFallRenderer.ReservoirSourceX - this.Puzzle.ClayMin.X) - (this.Screen.Point.X / 16), (WaterFallRenderer.IntroLength + 260) / 16);

            int y = 0;

            while (this.Puzzle.Map[new(stream.X, ++y)] == EntityType.Air)
            {
                graphics.DrawImageWithOpacity(
                    Opacity,
                    this.IntroLiquid.Flow(this.FlowIndex),
                    new((int)stream.X * 16, (y * 16) - (int)this.Screen.Point.Y, 16, 16));

                this.IncrementFlow();
            }
        }

        private void SetTransition(
            Biome biomeA,
            Biome biomeB)
        {
            if (!this.IsTransition
               && this.BiomeTypeB != this.CurrentBiomeType)
            {
                this.IsTransition = true;
                this.OpacityA = biomeA.Liquids[0].Opacity;
                this.OpacityB = 0.0f;
            }
            else if (this.IsTransition)
            {
                this.OpacityA -= TransitionSpeed;
                this.OpacityB += TransitionSpeed;

                if (this.OpacityB > biomeB.Liquids[0].Opacity)
                {
                    this.OpacityA = biomeB.Liquids[0].Opacity;
                    this.OpacityB = biomeB.Liquids[0].Opacity;
                    this.IsTransition = false;
                    this.CurrentBiomeType = this.BiomeTypeB;
                }
            }
        }

        private void RenderLiquid(Graphics graphics)
        {
            var edges = this.Screen.Edges();

            this.BiomeTypeA = Biome.GetBiomeType(this.Biomes, edges.TopLeft.Y, WaterFallRenderer.IntroLength, this.Puzzle.Map.Height);
            this.BiomeTypeB = Biome.GetBiomeType(this.Biomes, edges.BottomLeft.Y, WaterFallRenderer.IntroLength, this.Puzzle.Map.Height);

            Biome biomeA = this.Biomes[this.BiomeTypeA];
            Biome biomeB = this.Biomes[this.BiomeTypeB];

            this.SetTransition(biomeA, biomeB);

            if (this.BiomeTypeB == this.CurrentBiomeType)
            {
                this.BiomeTypeA = this.BiomeTypeB;
                biomeA = biomeB;
            }

            for (long y = edges.TopLeft.Y; y <= edges.BottomLeft.Y; y++)
            {
                for (long x = edges.TopLeft.X; x <= edges.TopRight.X; x++)
                {
                    Vector<long> point = this.Screen.RenderPoint(x, y);
                    EntityType value = this.Puzzle.Map[new(x, y)];

                    if (value != EntityType.Water
                        && value != EntityType.Settled)
                    {
                        continue;
                    }

                    this.SetLiquidImages(biomeA, biomeB, x, y);

                    if (this.ImageA != null)
                    {
                        RenderLiquid(graphics, this.OpacityA, this.ImageA, point);
                    }

                    if (this.IsTransition && this.ImageB != null)
                    {
                        RenderLiquid(graphics, this.OpacityB, this.ImageB, point);
                    }
                }
            }
        }

        private void SetLiquidImages(
            Biome biomeA,
            Biome biomeB,
            long x,
            long y)
        {
            switch (this.GetLiquidTileType(new(x, y)))
            {
                case LiquidTileType.Settled:
                case LiquidTileType.FlowSplit:
                    this.ImageA = biomeA.Liquids[0].Settled;
                    this.ImageB = biomeB.Liquids[0].Settled;
                    break;
                case LiquidTileType.TopRight:
                    this.ImageA = biomeA.Liquids[0].TopRight;
                    this.ImageB = biomeB.Liquids[0].TopRight;
                    break;
                case LiquidTileType.TopLeft:
                    this.ImageA = biomeA.Liquids[0].TopLeft;
                    this.ImageB = biomeB.Liquids[0].TopLeft;
                    break;
                case LiquidTileType.Top:
                    this.ImageA = biomeA.Liquids[0].Top(this.TopIndex);
                    this.ImageB = biomeB.Liquids[0].Top(this.TopIndex);
                    this.IncrementTop();

                    break;
                case LiquidTileType.Flow:
                    this.ImageA = biomeA.Liquids[0].Flow(this.FlowIndex);
                    this.ImageB = biomeB.Liquids[0].Flow(this.FlowIndex);

                    if (this.FlowSkip >= 50)
                    {
                        this.IncrementFlow();

                        this.FlowSkip = 0;
                    }

                    this.FlowSkip++;

                    break;
            }
        }

        private LiquidTileType GetLiquidTileType(Vector<long> point)
        {
            EntityType value = this.Puzzle.Map[point];
            IEnumerable<VectorCell<long, EntityType>> adjacent = this.Puzzle.Map.AdjacentCardinal(point);

            EntityType north = adjacent.FirstOrDefault(x => x.Direction == Cardinal.North)?.Value ?? EntityType.Air;
            EntityType south = adjacent.FirstOrDefault(x => x.Direction == Cardinal.South)?.Value ?? EntityType.Air;
            EntityType east = adjacent.FirstOrDefault(x => x.Direction == Cardinal.East)?.Value ?? EntityType.Air;
            EntityType west = adjacent.FirstOrDefault(x => x.Direction == Cardinal.West)?.Value ?? EntityType.Air;

            if (south == EntityType.Clay
                && ((west == EntityType.Air && east == EntityType.Air)
                || (west == EntityType.Air && east == EntityType.Clay)
                || (west == EntityType.Clay && east == EntityType.Air)))
            {
                return LiquidTileType.Flow;
            }

            if (value == EntityType.Settled)
            {
                return (north != EntityType.Water && north != EntityType.Settled && north != EntityType.Clay)
                    ? LiquidTileType.Top : LiquidTileType.Settled;
            }

            if (north != EntityType.Water)
            {
                adjacent = this.Puzzle.Map.AdjacentInterCardinal(point);

                EntityType southWest = adjacent.FirstOrDefault(x => x.Direction == Cardinal.SouthWest)?.Value ?? EntityType.Air;

                if (east == EntityType.Air
                    && southWest == EntityType.Clay)
                {
                    return LiquidTileType.TopRight;
                }

                EntityType southEast = adjacent.FirstOrDefault(x => x.Direction == Cardinal.SouthEast)?.Value ?? EntityType.Air;

                if (west == EntityType.Air
                    && southEast == EntityType.Clay)
                {
                    return LiquidTileType.TopLeft;
                }

                return LiquidTileType.Top;
            }

            if (south == EntityType.Clay)
            {
                return LiquidTileType.FlowSplit;
            }

            if (east == EntityType.Water
                && west == EntityType.Water)
            {
                return LiquidTileType.Settled;
            }

            return LiquidTileType.Flow;
        }

        private void IncrementTop()
        {
            this.TopIndex++;

            if (this.TopIndex > 16)
            {
                this.TopIndex = 0;
            }
        }

        private void IncrementFlow()
        {
            this.FlowIndex += 5;

            if (this.FlowIndex > 77)
            {
                this.FlowIndex = 2;
            }
        }
    }
}
