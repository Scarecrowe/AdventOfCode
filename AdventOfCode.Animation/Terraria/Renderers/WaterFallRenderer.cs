namespace AdventOfCode.Animation.Terraria.Renderers
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using AdventOfCode.Animation.FFmpeg;
    using AdventOfCode.Animation.Terraria.Basins;
    using AdventOfCode.Animation.Terraria.Biomes;
    using AdventOfCode.Animation.Terraria.Npcs;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class WaterFallRenderer
    {
        public const int Fps = 30;
        public const int AudioFadeDuration = 5;
        public const double Gravity = 1.8;
        public const double TimeDelta = 0.2;
        public const int IntroLength = 2836;
        public const int ReservoirSourceX = 500;

        public WaterFallRenderer(int width = 1920, int height = 1080)
        {
            this.Width = width;
            this.Height = height;
            LoadRandomSeed();

            this.Biomes = LoadBiomes();
            this.Parameters = this.LoadRenderParameters();
            this.OutroRenderer = new(this.Parameters);
            this.Puzzle = this.LoadPuzzle();
            this.Basins = this.LoadBasins();
            this.OutroRenderer.SetAnswerBasin(this.Basins[^1]);
            this.Collisions = this.LoadCollisions();
            this.Npcs = this.LoadNpcs();
            this.Screen = new(width, height, new(this.Puzzle.Map.Width, this.Puzzle.Map.Height), 16, 16, this.Puzzle.ClayMin.X);
            this.SceneRenderer = new(this.Puzzle, this.Biomes, this.Basins);
            this.LiquidRenderer = new(this.Puzzle, this.Screen, this.Biomes);
            this.NpcRenderer = new(this.Screen, this.Npcs);
            this.FadeRenderer = new(this.Width, this.Height, FadeType.Out);
            this.Frame = new(width, height, PixelFormat.Format32bppPArgb);
            this.SplashRenderer = new(this.Screen);
            this.IntroRenderer = new(this.Screen);
            this.FadePoint = new(this.OutroRenderer.AnswerBasin.Top.Left.X, this.OutroRenderer.AnswerBasin.Top.Left.Y - 3);
        }

        private int Width { get; }

        private int Height { get; }

        private ReservoirResearch Puzzle { get; }

        private SceneRenderer SceneRenderer { get; }

        private LiquidRenderer LiquidRenderer { get; set; }

        private NpcRenderer NpcRenderer { get; set; }

        private SplashRenderer SplashRenderer { get; set; }

        private IntroRenderer IntroRenderer { get; set; }

        private OutroRenderer OutroRenderer { get; set; }

        private FadeRenderer FadeRenderer { get; set; }

        private Dictionary<BiomeType, Biome> Biomes { get; }

        private RenderParameters Parameters { get; }

        private List<Basin> Basins { get; }

        private List<INpc> Npcs { get; set; }

        private HashSet<Vector<long>> Collisions { get; }

        private Screen Screen { get; set; }

        private Bitmap? Scene { get; set; }

        private Bitmap Frame { get; set;  }

        private FFmpegBuilder? FFmpeg { get; set; }

        private int LiquidCount { get; set; }

        private bool ScrollStopped { get; set; }

        private Vector<long> FadePoint { get; }

        public static string[] LoadPuzzleInput(bool addAnswerBasin, Screen screen)
        {
            List<string> input = new();

            foreach (string line in Animation.GetInput(2018, 17).ToList())
            {
                string[] tokens = line.Split(", ");

                if (tokens[0].StartsWith("y"))
                {
                    input.Add($"y={int.Parse(tokens[0].Replace("y=", string.Empty)) + (IntroLength / 16)}, {tokens[1]}");
                }
                else
                {
                    int[] range = tokens[1].Replace("y=", string.Empty).Split("..").Select(x => int.Parse(x) + (IntroLength / 16)).ToArray();

                    input.Add($"{tokens[0]}, y={range[0]}..{range[1]}");
                }
            }

            input.Add($"y=1, x=510..520");

            ReservoirResearch puzzle = new(input.ToArray());

            Basin.AddFinalBasin(puzzle, input);

            if (addAnswerBasin)
            {
                Basin.AddAnswerBasin(puzzle, input, screen);
            }

            return input.ToArray();
        }

        public void Render()
        {
            this.Scene = this.SceneRenderer.Render(Animation.GetRenderPath(), this.OutroRenderer);
            this.RenderFullScene();

            Console.WriteLine($"Frames: {this.Parameters.Frames}");
            Console.WriteLine($"Video length: {TimeSpan.FromSeconds(this.Parameters.Frames / Fps)}");
            using Graphics graphics = Graphics.FromImage(this.Frame);

            int frame = 0;
            int frames = 0;
            bool finished = false;
            this.FFmpeg = new VideoRenderer(this.Parameters).Build().Run();

            this.SplashRenderer.Render(this.FFmpeg);
            this.IntroRenderer.Render(graphics, this.Frame, this.Scene, this.FFmpeg, this.Puzzle);

            while (!finished)
            {
                frame++;
                frames++;
                this.UpdateLoop(frame, ref finished);
                this.RenderLoop(graphics, this.Scene, frame);
                this.FFmpeg.WriteBitmap(this.Frame, ImageFormat.Jpeg);
                ResetFrame(ref frame);
            }

            this.FFmpeg.Quit();
        }

        private static void LoadRandomSeed()
        {
            Console.WriteLine("Random Seed [-2,147,483,647 to 2,147,483,647]");
            Console.WriteLine("Leave empty for a randomly generated seed");
            string? seed = Console.ReadLine();

            if (seed != null
                && seed != string.Empty)
            {
                RandomGenerator.Initialise(int.Parse(seed));
            }
            else
            {
                RandomGenerator.Initialise();
            }

            Console.WriteLine($"Seed: {RandomGenerator.Seed}");
        }

        private static Dictionary<BiomeType, Biome> LoadBiomes()
        {
            Console.WriteLine("Loading Biomes");

            return Biome.LoadOrderedBiomes();
        }

        private static void ResetFrame(ref int frame)
            => frame = frame == Fps ? 0 : frame;

        private HashSet<Vector<long>> LoadCollisions()
        {
            Console.WriteLine("Loading NPC Collisions");

            return this.Puzzle.Map.Values(EntityType.Clay).Select(cell => cell.Point).ToHashSet();
        }

        private RenderParameters LoadRenderParameters()
        {
            Console.WriteLine("Loading Render Parameters");

            return new(this.Width, this.Height, this.Biomes);
        }

        private ReservoirResearch LoadPuzzle()
        {
            Console.WriteLine("Loading Puzzle");

            return new(LoadPuzzleInput(true, this.Parameters.Screen));
        }

        private List<Basin> LoadBasins()
        {
            Console.WriteLine("Loading Basins");

            List<Basin> result = Basin.Parse(this.Puzzle.Map, this.Biomes);

            return result;
        }

        private List<INpc> LoadNpcs()
        {
            Console.Write("Spawn All NPCs? [y, n] ");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            Console.WriteLine();

            if ((char)keyInfo.Key != 'y'
                && keyInfo.Key.ToString().ToUpper() != "Y")
            {
                string[] npcs = Enum.GetNames(typeof(NpcType));

                for(int i = 0; i < npcs.Length; i++)
                {
                    Console.WriteLine($"({i + 1}) - {npcs[i]}");
                }

                Console.WriteLine($"Select an NPC [1 to {npcs.Length}]: ");

                string? input = Console.ReadLine();
                int index = 1;

                if (!string.IsNullOrEmpty(input))
                {
                    int.TryParse(input, out index);
                }

                index -= 1;

                NpcType npcName = (NpcType)(Enum.GetValues(typeof(NpcType))?.GetValue(index) ?? NpcType.Guide);

                return NpcSpawner.Single(npcName, this.Basins, this.Biomes, this.Collisions, this.Puzzle.Map);
            }

            List<INpc> result = NpcSpawner.Multiple(this.Basins, this.Biomes, this.Collisions, this.Puzzle.Map);

            return result;
        }

        private void IsMaxScrollHeight()
        {
            if (this.Screen.Point.Y > this.OutroRenderer.MaxY)
            {
                this.Screen.Point.Y = this.OutroRenderer.MaxY;
                this.ScrollStopped = true;
            }
        }

        private bool IsFadeOut()
            => this.Puzzle.Map[this.FadePoint] == EntityType.Water
                || this.Puzzle.Map[this.FadePoint] == EntityType.Settled;

        private void UpdateLiquid(int frame, ref bool finished)
        {
            if (frame % (Fps / 3) != 0)
            {
                return;
            }

            if (this.ScrollStopped
                || this.Screen.LiquidMax.Y * 16 < this.Screen.Point.Y + (this.Screen.Height - 100))
            {
                while (true)
                {
                    Vector<long> stream = this.Puzzle.Animate();

                    if (stream == new Vector<long>(-1, -1))
                    {
                        finished = true;
                        return;
                    }

                    this.Screen.SetLiquidMax(stream);

                    int liquidCount = this.Screen.CountOfWater(this.Puzzle.Map);

                    if ((this.LiquidCount == liquidCount
                        && stream.Y < (this.Screen.Point.Y + this.Screen.Height) / 16)
                        || (this.ScrollStopped && this.LiquidCount == liquidCount))
                    {
                        continue;
                    }

                    this.LiquidCount = liquidCount;

                    break;
                }
            }
        }

        private void UpdateNpc(int frame)
        {
            Edges edges = this.Screen.Edges();

            foreach (INpc npc in this.Npcs)
            {
                Vector<long> point = npc.TilePoint();

                if (point.Y >= edges.TopLeft.Y - 2 && point.Y <= edges.BottomLeft.Y + 2)
                {
                    npc.Update(this.Collisions, this.Puzzle.Map, frame);
                }
            }
        }

        private void UpdateLoop(int frame, ref bool finished)
        {
            this.UpdateLiquid(frame, ref finished);
            this.UpdateNpc(frame);
            this.IsMaxScrollHeight();

            if (!this.ScrollStopped)
            {
                this.Screen.Scroll();
            }
        }

        private void RenderLoop(
            Graphics graphics,
            Bitmap scene,
            int frame)
        {
            this.RenderScene(graphics, scene);
            this.NpcRenderer.Render(graphics, frame);
            this.LiquidRenderer.Render(graphics);

            if (this.IsFadeOut())
            {
                this.FadeRenderer.Render(graphics);
            }
        }

        private void RenderScene(Graphics graphics, Bitmap scene)
            => graphics.DrawImage(
                scene,
                new Rectangle(0, 0, this.Width, this.Height),
                new Rectangle((int)this.Screen.Point.X, (int)this.Screen.Point.Y, this.Width, this.Height),
                GraphicsUnit.Pixel);

        private void RenderFullScene()
        {
            Console.Write("Render full scene with liquid? [y,n] ");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            Console.WriteLine();

            if (this.Scene == null
                || ((char)keyInfo.Key != 'y'
                    && keyInfo.Key.ToString().ToUpper() != "Y"))
            {
                return;
            }

            Bitmap bitmap = (Bitmap)this.Scene.Clone();

            Screen screen = new(bitmap.Width, bitmap.Height, new(this.Parameters.Puzzle.Map.Width, this.Parameters.Puzzle.Map.Height), 16, 16, this.Parameters.Puzzle.ClayMin.X);
            LiquidRenderer liquidRenderer = new(this.Parameters.Puzzle, screen, this.Biomes);

            using Graphics graphics = Graphics.FromImage(bitmap);

            liquidRenderer.Render(graphics);

            graphics.DrawRectangle(new Pen(new SolidBrush(Color.White), 5), new Rectangle((int)this.Parameters.Screen.Point.X, (int)this.Parameters.Screen.Point.Y, this.Screen.Width, this.Screen.Height));

            string renderPath = $"{Animation.GetRenderPath()}\\{RandomGenerator.Seed}-liquid.jpg";

            bitmap.Save(renderPath, ImageFormat.Jpeg);

            Console.WriteLine($"Saved: {renderPath}");
        }
    }
}
