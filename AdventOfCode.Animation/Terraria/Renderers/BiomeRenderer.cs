namespace AdventOfCode.Animation.Terraria.Renderers
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using AdventOfCode.Animation.Terraria.Biomes;

    public class BiomeRenderer
    {
        public BiomeRenderer(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public static Bitmap JoinNoise(int octave)
        {
            RandomGenerator.Initialise();
            float[][] noise = NoiseGenerator.WhiteNoise(3328, 500, RandomGenerator.Seed);
            noise = NoiseGenerator.PerlinNoise(noise, 8);
            noise = NoiseGenerator.SmoothEdges(noise, 3328, 500, 40);

            Bitmap result = new(3328, 500, PixelFormat.Format32bppArgb);

            for (int x = 0; x < 3328; x++)
            {
                for (int y = 0; y < 500; y++)
                {
                    if (noise[x][y] < 0.5f)
                    {
                        result.SetPixel(x, y, Color.White);
                        continue;
                    }

                    result.SetPixel(x, y, Color.Black);
                }
            }

            return result;
        }

        public static Bitmap RenderBiomeJoin(Bitmap biome)
        {
            int curveHeight = 40;
            float[][] noise = NoiseGenerator.WhiteNoise(biome.Width, 500, RandomGenerator.Seed);
            noise = NoiseGenerator.PerlinNoise(noise, 8);
            noise = NoiseGenerator.SmoothEdges(noise, biome.Width, 500, curveHeight);

            for (int x = 0; x < biome.Width; x++)
            {
                for (int y = 0; y < 500; y++)
                {
                    if (noise[x][y] < 0.5f)
                    {
                        biome.SetPixel(x, y, Color.Transparent);
                    }
                }
            }

            return biome;
        }

        public Bitmap RenderBackground(Biome biome)
        {
            Console.WriteLine();
            Console.WriteLine($"Rendering {biome.BiomeType} biome");

            Bitmap result = this.RenderBackgroundWall(biome.Backgrounds[0].Image);

            foreach ((Image Image, double Weight) tileset in biome.Backgrounds.Skip(1))
            {
                Bitmap input = this.RenderBackgroundWall(tileset.Image);
                result = this.RenderBackgroundWallNoise(result, input, tileset.Weight);
            }

            return result;
        }

        private Bitmap RenderBackgroundWall(Image wallTile)
        {
            Console.WriteLine("Rendering Background Wall");
            Bitmap result = new(this.Width, this.Height, PixelFormat.Format32bppPArgb);
            Graphics graphics = Graphics.FromImage(result);

            for (long y = 0; y <= this.Height; y += wallTile.Height)
            {
                for (long x = 0; x <= this.Width / wallTile.Width; x++)
                {
                    graphics.DrawImage(wallTile, x * wallTile.Width, y);
                }
            }

            graphics.Dispose();

            return result;
        }

        private Bitmap RenderBackgroundWallNoise(Bitmap inputA, Bitmap inputB, double size = 0.5)
        {
            Console.WriteLine("Generating background noise");

            float[][] raw = NoiseGenerator.PerlinNoise(NoiseGenerator.WhiteNoise(this.Width, this.Height, RandomGenerator.Seed), 8);

            Console.WriteLine("Rendering merged background");

            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    if (raw[x][y] > size)
                    {
                        inputA.SetPixel(x, y, inputB.GetPixel(x, y));
                    }
                }
            }

            return inputA;
        }
    }
}
