namespace AdventOfCode.Animation.Terraria
{
    using System.Drawing;
    using AdventOfCode.Animation.Terraria.Audio;
    using AdventOfCode.Animation.Terraria.Biomes;
    using AdventOfCode.Animation.Terraria.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class RenderParameters
    {
        public RenderParameters(
            int width,
            int height,
            Dictionary<BiomeType, Biome> biomes)
        {
            this.FadeType = FadeType.In;
            this.FadeRenderer = new(width, height, FadeType.In);
            this.Playlist = new();
            this.Puzzle = new(Animation.GetInput(2018, 17));
            this.Puzzle = new(WaterFallRenderer.LoadPuzzleInput(false, new(width, height, new(this.Puzzle.Map.Width, this.Puzzle.Map.Height), 16, 16, this.Puzzle.ClayMin.X)));
            this.Screen = new(width, height, new(this.Puzzle.Map.Width, this.Puzzle.Map.Height), 16, 16, this.Puzzle.ClayMin.X);
            this.Frames = 0;
            this.Load(biomes);
        }

        public Dictionary<BiomeType, BiomeAudio> Playlist { get; private set; }

        public Screen Screen { get; }

        public ReservoirResearch Puzzle { get; private set; }

        public int Frames { get; private set; }

        private FadeRenderer FadeRenderer { get; }

        private FadeType FadeType { get; set; }

        private int LiquidCount { get; set; }

        public void SetPlaylist(Dictionary<BiomeType, BiomeAudio> playlist)
        {
            this.Playlist = playlist;
        }

        private void Load(Dictionary<BiomeType, Biome> biomes)
        {
            List<int> seconds = new();
            Bitmap bmp = new(this.Screen.Width, this.Screen.Height);
            using Graphics graphics = Graphics.FromImage(bmp);

            this.FadeInTiming(graphics);
            this.TitleTiming(graphics);
            this.FadeOutTiming(graphics);
            this.IntroLiquidTiming();
            this.LiquidTiming(biomes, seconds);
            this.PlaylistTiming(biomes, seconds);
        }

        private void FadeInTiming(Graphics graphics)
        {
            this.FadeRenderer.SetFadeIn();

            while (true)
            {
                this.Frames++;

                this.FadeRenderer.Render(graphics);

                if (this.FadeRenderer.Opacity == 0)
                {
                    break;
                }
            }
        }

        private void FadeOutTiming(Graphics graphics)
        {
            this.FadeRenderer.SetFadeOut();

            while (true)
            {
                this.Frames++;

                this.FadeRenderer.Render(graphics);

                if (this.FadeRenderer.Opacity == 255)
                {
                    break;
                }
            }
        }

        private void TitleTiming(Graphics graphics)
        {
            int index = 0;
            int opacity = 0;

            while (true)
            {
                this.Frames++;

                if (this.FadeType == FadeType.In)
                {
                    if (opacity >= 255)
                    {
                        this.FadeType = FadeType.Out;
                    }
                    else
                    {
                        opacity += 3;
                    }
                }
                else
                {
                    if (opacity <= 0)
                    {
                        this.FadeType = FadeType.In;
                        index++;

                        if (index == 2)
                        {
                            break;
                        }
                    }
                    else
                    {
                        opacity -= 3;
                    }
                }
            }
        }

        private void IntroLiquidTiming()
        {
            int flowLength = 0;
            Vector<long> stream = new(WaterFallRenderer.ReservoirSourceX - this.Puzzle.ClayMin.X, (WaterFallRenderer.IntroLength + 260) / 16);

            while (true)
            {
                this.Frames++;

                int waterTop = (11 * 16) + 48;

                waterTop = (waterTop - 4) + (5 * 16);

                if (waterTop + (flowLength * 16) >= WaterFallRenderer.IntroLength + 64)
                {
                    break;
                }

                if (this.Frames % 3 == 0)
                {
                    flowLength++;
                }

                if (flowLength > 40)
                {
                    this.Screen.SetLiquidMax(new(stream.X, stream.Y));
                    this.Screen.Scroll();
                }
            }
        }

        private void LiquidTiming(
            Dictionary<BiomeType, Biome> biomes,
            List<int> seconds)
        {
            int frame = 0;
            int biomeIndex = 0;
            bool finished = false;
            Vector<long> stream = new();
            List<(BiomeType BiomeType, long Y)> heights = Biome.GetBiomeHeights(biomes, WaterFallRenderer.IntroLength, this.Puzzle.Map.Height);

            while (!finished)
            {
                frame++;

                if (frame % (WaterFallRenderer.Fps / 3) == 0
                    && (this.Screen.LiquidMax.Y * 16) < this.Screen.Point.Y + (this.Screen.Height - 100))
                {
                    while (true)
                    {
                        stream = this.Puzzle.Animate();

                        if (stream == new Vector<long>(-1, -1))
                        {
                            finished = true;
                            break;
                        }

                        int liquidCount = this.Screen.CountOfWater(this.Puzzle.Map);

                        this.Screen.SetLiquidMax(stream);

                        if (this.LiquidCount == liquidCount
                            && stream.Y < (this.Screen.Point.Y + this.Screen.Height) / 16)
                        {
                            continue;
                        }

                        this.LiquidCount = liquidCount;

                        break;
                    }
                }

                this.Frames++;

                this.Screen.Scroll();

                if (biomeIndex < heights.Count
                    && this.Screen.Point.Y + this.Screen.Height >= heights[biomeIndex].Y + 100)
                {
                    seconds.Add(this.Frames / WaterFallRenderer.Fps);
                    biomeIndex++;
                }

                if (frame == WaterFallRenderer.Fps)
                {
                    frame = 0;
                }
            }
        }

        private void PlaylistTiming(
            Dictionary<BiomeType, Biome> biomes,
            List<int> seconds)
        {
            BiomeType biomeType = BiomeType.Overworld;

            this.Playlist.Add(biomeType, new(biomes.ElementAt(0).Value.Audio, 0, seconds[0]));

            for (int i = 1; i < biomes.Count - 1; i++)
            {
                biomeType = biomes.ElementAt(i).Key;

                this.Playlist.Add(biomeType, new(biomes[biomeType].Audio, seconds[i - 1], seconds[i]));
            }

            biomeType = biomes.ElementAt(biomes.Count - 1).Key;

            this.Playlist.Add(biomeType, new(biomes[biomeType].Audio, seconds[biomes.Count - 2], 5000));

            Console.WriteLine($"StreamMax: {this.Screen.LiquidMax.X},{this.Screen.LiquidMax.Y}");
            Console.WriteLine($"Scroll: {this.Screen.Point.X},{this.Screen.Point.Y}");

            foreach (KeyValuePair<BiomeType, BiomeAudio> pair in this.Playlist)
            {
                Console.WriteLine($"Biome: {pair.Key} - Start: {pair.Value.Start}, End: {pair.Value.End}");
            }
        }
    }
}
