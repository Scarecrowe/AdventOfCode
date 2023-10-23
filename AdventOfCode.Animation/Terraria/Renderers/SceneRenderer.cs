namespace AdventOfCode.Animation.Terraria.Renderers
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using AdventOfCode.Animation.Terraria.Basins;
    using AdventOfCode.Animation.Terraria.Biomes;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class SceneRenderer
    {
        public SceneRenderer(
            ReservoirResearch puzzle,
            Dictionary<BiomeType, Biome> biomes,
            List<Basin> basins)
        {
            this.Puzzle = puzzle;
            this.Max = new(this.Puzzle.Map.Width * 16, this.Puzzle.Map.Height * 16);

            this.Scene = new(this.Max.X.ToInt(), this.Max.Y.ToInt() + 1500, PixelFormat.Format32bppPArgb);
            this.Graphics = Graphics.FromImage(this.Scene);
            this.Graphics.Clear(Color.Black);
            this.Biomes = biomes;
            this.Basins = basins;
            this.BasinRenderer = new(this.Basins, this.Biomes);
            this.RandomRenderer = new(this.Basins, this.Biomes);
        }

        public ReservoirResearch Puzzle { get; }

        public Bitmap Scene { get; private set; }

        public Vector<long> Max { get; }

        public List<Basin> Basins { get; }

        public int BiomeStart { get; private set; }

        public int BiomeLength { get; private set; }

        public int IntroLength { get; private set; }

        private Graphics Graphics { get; set; }

        private Dictionary<BiomeType, Biome> Biomes { get; }

        private BasinRenderer BasinRenderer { get; }

        private RandomRenderer RandomRenderer { get; }

        public Bitmap Render(string renderPath, OutroRenderer outroRenderer)
        {
            Console.WriteLine("Rendering Scene");
            Console.WriteLine($"Scene {this.Max.X}px/{this.Max.Y}px ({this.Puzzle.Map.Width}/{this.Puzzle.Map.Height})");
            Console.WriteLine($"Biome Start: {this.BiomeStart}px");
            Console.WriteLine($"Biome Length: {this.BiomeLength}px");

            string scenePath = $"{renderPath}\\scene.jpg";

            if (File.Exists(scenePath))
            {
                Console.Write("Scene.jpg already exists, use? [y,n] ");
                ConsoleKeyInfo input = Console.ReadKey();
                Console.WriteLine();

                if ((char)input.Key == 'y' || input.Key.ToString().ToUpper() == "Y")
                {
                    this.Scene = (Bitmap)Image.FromFile(scenePath);
                    this.Graphics = Graphics.FromImage(this.Scene);
                }
                else
                {
                    this.RenderScene();
                }
            }
            else
            {
                this.RenderScene();
            }

            this.BasinRenderer.RenderWalls(this.Graphics);
            this.BasinRenderer.RenderBlocks(this.Graphics);
            this.RandomRenderer.Render(this.Graphics, this.Puzzle.Map);
            outroRenderer.RenderAnswerStatues(this.Graphics);
            this.Graphics.Dispose();

            this.Scene.Save($"{renderPath}\\{RandomGenerator.Seed}.jpg", ImageFormat.Jpeg);

            return this.Scene;
        }

        private void RenderIntro()
        {
            Console.WriteLine("Rendering Intro");
            Image sky = Terraria.GetImage("backgrounds\\background_0.png");
            Image layer1 = Terraria.GetImage("Backgrounds\\background.png");
            Image layer2 = Terraria.GetImage("Backgrounds\\Background_251.png");

            this.BiomeStart = (sky.Height + layer1.Height + layer2.Height) - 310;
            this.IntroLength = (sky.Height + layer1.Height + layer2.Height) - 310;
            this.BiomeLength = (this.Max.Y.ToInt() - this.BiomeStart) / 5;

            for (int i = 0; i <= this.Scene.Width / sky.Width; i++)
            {
                this.Graphics.DrawImage(sky, new Point(i * sky.Width, 0));
            }

            for (int i = 0; i <= this.Scene.Width / layer1.Width; i++)
            {
                this.Graphics.DrawImage(layer1, new Point(i * layer1.Width, sky.Height - 110));
            }

            for (int i = 0; i <= this.Scene.Width / layer2.Width; i++)
            {
                this.Graphics.DrawImage(layer2, new Point(i * layer2.Width, (sky.Height + layer1.Height) - 310));
            }
        }

        private void RenderScene()
        {
            this.RenderIntro();

            List<Bitmap> scenes = new();

            for (int biome = 1; biome <= 5; biome++)
            {
                int length = this.BiomeLength + 500;

                if (biome == 5)
                {
                    length += 1500;
                }

                BiomeRenderer biomeRenderer = new(this.Max.X.ToInt(), length);

                scenes.Add(biomeRenderer.RenderBackground(this.Biomes[(BiomeType)biome]));
            }

            for (int i = 0; i < 5; i++)
            {
                long start = this.BiomeStart + (i * this.BiomeLength);

                if (i == 0)
                {
                    this.Graphics.DrawImage(scenes[i], 0, start);
                    continue;
                }

                this.Graphics.DrawImage(BiomeRenderer.RenderBiomeJoin(scenes[i]), 0, start - 500);
            }

            this.Scene.Save($"{Animation.GetRenderPath()}\\scene.jpg", ImageFormat.Png);
        }
    }
}
