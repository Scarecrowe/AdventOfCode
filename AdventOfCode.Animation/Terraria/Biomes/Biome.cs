namespace AdventOfCode.Animation.Terraria.Biomes
{
    using System.Drawing;
    using AdventOfCode.Animation.Terraria.Basins;
    using AdventOfCode.Animation.Terraria.Liquids;
    using AdventOfCode.Animation.Terraria.Npcs;
    using AdventOfCode.Animation.Terraria.Random;

    public class Biome
    {
        public Biome(BiomeType biomeType)
        {
            this.BiomeType = biomeType;
            this.Backgrounds = new();
            this.BasinTiles = new();
            this.BasinWalls = new();
            this.RandomAssets = new();
            this.WalkingNpcs = new();
            this.FlyingNpcs = new();
            this.SwimmingNpcs = new();
            this.Audio = string.Empty;
            this.Liquids = new();
        }

        public BiomeType BiomeType { get; }

        public List<(Image Image, double Weight)> Backgrounds { get; }

        public List<IBlockTileset> BasinTiles { get; }

        public List<WallTileset> BasinWalls { get; }

        public List<IRandomAsset> RandomAssets { get; }

        public string Audio { get; private set; }

        public List<Liquid> Liquids { get; private set; }

        public List<INpc> WalkingNpcs { get; private set; }

        public List<INpc> SwimmingNpcs { get; private set; }

        public List<INpc> FlyingNpcs { get; private set; }

        public static Biome GetBiomeByType(BiomeType biomeType)
        {
            return biomeType switch
            {
                BiomeType.Overworld => Overworld(),
                BiomeType.Underground => Underground(),
                BiomeType.Mushroom => Mushroom(),
                BiomeType.Ice => Ice(),
                BiomeType.Hallowed => Hallowed(),
                _ => Hell(),
            };
        }

        public static List<(BiomeType BiomeType, long Y)> GetBiomeHeights(
            Dictionary<BiomeType, Biome> biomes,
            long introLength,
            long height)
        {
            List<(BiomeType BiomeType, long Y)> result = new() { (biomes.ElementAt(1).Key, introLength) };

            long biomeLength = ((height * 16) - introLength) / 5;

            for (int i = 2; i < biomes.Count; i++)
            {
                introLength += biomeLength;
                result.Add((biomes.ElementAt(i).Key, introLength));
            }

            return result;
        }

        public static BiomeType GetBiomeType(
            Dictionary<BiomeType, Biome> biomes,
            long y,
            long introLength,
            long height)
        {
            List<(BiomeType BiomeType, long Y)> heights = GetBiomeHeights(biomes, introLength, height);

            y *= 16;
            ////y += introLength;

            for (int i = 0; i < heights.Count - 1; i++)
            {
                if (y >= heights[i].Y && y < heights[i + 1].Y)
                {
                    return heights[i].BiomeType;
                }
            }

            if (y >= heights[^1].Y)
            {
                return BiomeType.Hell;
            }

            return BiomeType.Overworld;
        }

        public static Biome Overworld()
        {
            Biome result = new(BiomeType.Overworld);

            result.AddBasinTile(new BlockTileSetC(Terraria.GetImage("Blocks\\Tiles_2.png")));
            result.AddBasinTile(new BlockTilesetA(Terraria.GetImage("Blocks\\Tiles_2_Beach.png")));
            result.AddBasinTile(new BlockTilesetA(Terraria.GetImage("Blocks\\Tiles_60.png")));
            result.AddBasinTile(new BlockTilesetB(Terraria.GetImage("Blocks\\Tiles_191.png")));

            result.AddBasinWall("Walls\\Wall_15.png");
            result.AddBasinWall("Walls\\Wall_2.png");
            result.AddBasinWall("Walls\\Wall_3.png");
            result.AddBasinWall("Walls\\Wall_60.png");
            result.AddBasinWall("Walls\\Wall_68.png");
            result.AddBasinWall("Walls\\Wall_70.png");
            result.AddBasinWall("Walls\\Wall_72.png");

            result.AddGrass("Random\\Tiles_3.png", new(0, 0.49), 0, 5);
            result.AddGrave("Random\\Tiles_85.png", new(0.5, 0.51));
            result.AddPumpkin("Random\\Tiles_35.png", new(0.51, 0.515));
            result.AddCaptured("Random\\NPC_105.png", 28, 32, new(0.515, 0.516));
            result.AddCaptured("Random\\NPC_106.png", 34, 48, new(0.516, 0.517));
            result.AddCaptured("Random\\NPC_123.png", 36, 34, new(0.517, 0.518));
            result.AddCaptured("Random\\NPC_354.png", 35, 40, new(0.518, 0.519));
            result.AddFlower("Random\\Tiles_3.png", new(0.519, 1.0), 6, 35);

            result.SetAudio(Terraria.GetAssetPath("audio\\overworld.mp3"));
            result.SetLiquid("Liquids\\water_0.png", "Liquids\\liquid_0.png", LiquidType.Water, 0.6f);

            result.AddWalkingNpcs(new() { NpcType.Guide, NpcType.Wizard, NpcType.WoodenMimic, NpcType.WhiteRabit, NpcType.Squirel });
            result.AddSwimmingNpcs(new() { NpcType.YellowFish });
            result.AddFlyingNpcs(new() { NpcType.WhiteBird, NpcType.BlueBird, NpcType.RedBird });

            return result;
        }

        public static Biome Underground()
        {
            Biome result = new(BiomeType.Underground);

            result.AddBackround("Backgrounds\\Background_3.png", 0.3);
            result.AddBackround("Backgrounds\\Background_67.png", 0.5);
            result.AddBackround("Backgrounds\\Background_74.png", 0.5);
            result.AddBackround("Backgrounds\\Background_87.png", 0.7);

            result.AddBasinTile(new BlockTilesetB(Terraria.GetImage("Blocks\\Tiles_0.png")));
            result.AddBasinTile(new BlockTilesetB(Terraria.GetImage("Blocks\\Tiles_1.png")));
            result.AddBasinTile(new BlockTilesetB(Terraria.GetImage("Blocks\\Tiles_40.png")));
            result.AddBasinTile(new BlockTilesetB(Terraria.GetImage("Blocks\\Tiles_59.png")));

            result.AddBasinWall("Walls\\Wall_1.png");
            result.AddBasinWall("Walls\\Wall_2.png");
            result.AddBasinWall("Walls\\Wall_15.png");
            result.AddBasinWall("Walls\\Wall_16.png");
            result.AddBasinWall("Walls\\Wall_24.png");

            result.AddGrass("Random\\Tiles_3.png", new(0, 0.49), 0, 5);
            result.AddGrave("Random\\Tiles_85.png", new(0.49, 0.51));
            result.AddPumpkin("Random\\Tiles_35.png", new(0.51, 0.515));
            result.AddCaptured("Random\\NPC_105.png", 28, 32, new(0.515, 0.516));
            result.AddCaptured("Random\\NPC_106.png", 34, 48, new(0.516, 0.517));
            result.AddCaptured("Random\\NPC_123.png", 36, 34, new(0.517, 0.518));
            result.AddCaptured("Random\\NPC_354.png", 35, 40, new(0.518, 0.519));
            result.AddFlower("Random\\Tiles_3.png", new(0.519, 0.7), 6, 35);
            result.AddFlower("Random\\Tiles_71.png", new(0.7, 1.0), 0, 4);

            result.SetAudio(Terraria.GetAssetPath("audio\\underground.mp3"));
            result.SetLiquid("Liquids\\water_0.png", "Liquids\\liquid_0.png", LiquidType.Water, 0.6f);

            result.AddWalkingNpcs(new() { NpcType.Guide, NpcType.Wizard, NpcType.WoodenMimic, NpcType.SkelletonMiner, NpcType.SkeletonMerchant, NpcType.SkeletonMerchantNoHat, NpcType.WhiteRabit, NpcType.Squirel });
            result.AddSwimmingNpcs(new() { NpcType.YellowFish });
            result.AddFlyingNpcs(new() { NpcType.WhiteBird, NpcType.BlueBird, NpcType.RedBird });

            return result;
        }

        public static Biome Mushroom()
        {
            Biome result = new(BiomeType.Mushroom);

            result.AddBackround("Backgrounds\\Background_76.png", 0.5);
            result.AddBackround("Backgrounds\\Background_63.png", 0.5);
            result.AddBackround("Backgrounds\\Background_65.png", 0.5);

            result.AddBasinTile(new BlockTilesetA(Terraria.GetImage("Blocks\\Tiles_70.png")));
            result.AddBasinTile(new BlockTilesetA(Terraria.GetImage("Blocks\\Tiles_109.png")));
            result.AddBasinTile(new BlockTilesetA(Terraria.GetImage("Blocks\\Tiles_492.png")));
            result.AddBasinTile(new BlockTilesetB(Terraria.GetImage("Blocks\\Tiles_190.png")));

            result.AddBasinWall("Walls\\Wall_74.png");
            result.AddBasinWall("Walls\\Wall_79.png");
            result.AddBasinWall("Walls\\Wall_80.png");
            result.AddBasinWall("Walls\\Wall_110.png");
            result.AddBasinWall("Walls\\Wall_133.png");

            result.AddGrass("Random\\Tiles_24.png", new(0, 0.49), 0, 5);
            result.AddGrave("Random\\Tiles_85.png", new(0.49, 0.51));
            result.AddPumpkin("Random\\Tiles_35.png", new(0.51, 0.515));
            result.AddCaptured("Random\\NPC_105.png", 28, 32, new(0.515, 0.516));
            result.AddCaptured("Random\\NPC_106.png", 34, 48, new(0.516, 0.517));
            result.AddCaptured("Random\\NPC_123.png", 36, 34, new(0.517, 0.518));
            result.AddCaptured("Random\\NPC_354.png", 35, 40, new(0.518, 0.519));
            result.AddFlower("Random\\Tiles_24.png", new(0.519, 1.0), 6, 22);

            result.SetAudio(Terraria.GetAssetPath("audio\\mushroom.mp3"));
            result.SetLiquid("Liquids\\water_5.png", "Liquids\\liquid_5.png", LiquidType.Water, 0.6f);

            result.AddWalkingNpcs(new() { NpcType.Mushroom, NpcType.Goblin, NpcType.GoldenMimic, NpcType.Gnome, NpcType.SkelletonMiner, NpcType.GlowingSnail, NpcType.Dracula });
            result.AddSwimmingNpcs(new() { NpcType.PurpleFish, NpcType.BlueFish });
            result.AddFlyingNpcs(new() { NpcType.BlueBat, NpcType.BrownBat, NpcType.RedBat });

            return result;
        }

        public static Biome Ice()
        {
            Biome result = new(BiomeType.Ice);

            result.AddBackround("Backgrounds\\Background_200.png", 0.5);
            result.AddBackround("Backgrounds\\Background_118.png", 0.5);
            result.AddBackround("Backgrounds\\Background_119.png", 0.5);
            result.AddBackround("Backgrounds\\Background_120.png", 0.7);

            result.AddBasinTile(new BlockTilesetB(Terraria.GetImage("Blocks\\Tiles_116.png")));
            result.AddBasinTile(new BlockTilesetB(Terraria.GetImage("Blocks\\Tiles_121.png")));
            result.AddBasinTile(new BlockTilesetB(Terraria.GetImage("Blocks\\Tiles_147.png")));
            result.AddBasinTile(new BlockTilesetB(Terraria.GetImage("Blocks\\Tiles_161.png")));
            result.AddBasinTile(new BlockTilesetB(Terraria.GetImage("Blocks\\Tiles_163.png")));
            result.AddBasinTile(new BlockTilesetB(Terraria.GetImage("Blocks\\Tiles_206.png")));

            result.AddBasinWall("Walls\\Wall_40.png");
            result.AddBasinWall("Walls\\Wall_71.png");
            result.AddBasinWall("Walls\\Wall_73.png");
            result.AddBasinWall("Walls\\Wall_84.png");
            result.AddBasinWall("Walls\\Wall_127.png");

            result.AddGrass("Random\\Tiles_84.png", new(0, 0.2), 6, 6);
            result.AddGrass("Random\\Tiles_84.png", new(0.2, 0.3), 1, 1);
            result.AddGrass("Random\\Tiles_83.png", new(0.3, 0.4), 6, 6);
            result.AddGrass("Random\\Tiles_83.png", new(0.4, 0.5), 1, 1);

            result.AddGrave("Random\\Tiles_85.png", new(0.5, 0.51));
            result.AddPumpkin("Random\\Tiles_35.png", new(0.53, 0.535));
            result.AddCaptured("Random\\NPC_105.png", 28, 32, new(0.515, 0.516));
            result.AddCaptured("Random\\NPC_106.png", 34, 48, new(0.516, 0.517));
            result.AddCaptured("Random\\NPC_123.png", 36, 34, new(0.517, 0.518));
            result.AddCaptured("Random\\NPC_354.png", 35, 40, new(0.518, 0.519));
            result.AddGrass("Random\\Tiles_84.png", new(0.519, 1.0), 1, 1);

            result.SetAudio(Terraria.GetAssetPath("audio\\ice.mp3"));
            result.SetLiquid("Liquids\\water_0.png", "Liquids\\liquid_0.png", LiquidType.Water, 0.6f);

            result.AddWalkingNpcs(new() { NpcType.BlackPengiun, NpcType.BluePengiun, NpcType.GoldenMimic, NpcType.Gnome, NpcType.Santa, NpcType.SkelletonMiner, NpcType.Mummy, NpcType.Dracula });
            result.AddSwimmingNpcs(new() { NpcType.RedFish, NpcType.BlueFish, NpcType.YellowFish });
            result.AddFlyingNpcs(new() { NpcType.BlueBat, NpcType.BrownBat, NpcType.RedBat });

            return result;
        }

        public static Biome Hallowed()
        {
            Biome result = new(BiomeType.Hallowed);

            result.AddBackround("Backgrounds\\Background_197.png", 0.5);
            result.AddBackround("Backgrounds\\Background_199.png", 0.5);
            result.AddBackround("Backgrounds\\Background_156.png", 0.7);

            result.AddBasinTile(new BlockTilesetB(Terraria.GetImage("Blocks\\Tiles_63.png")));
            result.AddBasinTile(new BlockTilesetB(Terraria.GetImage("Blocks\\Tiles_64.png")));
            result.AddBasinTile(new BlockTilesetB(Terraria.GetImage("Blocks\\Tiles_65.png")));
            result.AddBasinTile(new BlockTilesetB(Terraria.GetImage("Blocks\\Tiles_107.png")));
            result.AddBasinTile(new BlockTilesetB(Terraria.GetImage("Blocks\\Tiles_108.png")));
            result.AddBasinTile(new BlockTilesetB(Terraria.GetImage("Blocks\\Tiles_111.png")));

            result.AddBasinWall("Walls\\Wall_88.png");
            result.AddBasinWall("Walls\\Wall_90.png");
            result.AddBasinWall("Walls\\Wall_91.png");
            result.AddBasinWall("Walls\\Wall_92.png");
            result.AddBasinWall("Walls\\Wall_202.png");
            result.AddBasinWall("Walls\\Wall_288.png");
            result.AddBasinWall("Walls\\Wall_290.png");
            result.AddBasinWall("Walls\\Wall_291.png");

            result.AddGrass("Random\\Tiles_110.png", new(0, 0.49), 0, 5);
            result.AddGrave("Random\\Tiles_85.png", new(0.49, 0.495));
            result.AddPumpkin("Random\\Tiles_35.png", new(0.5, 0.515));
            result.AddCaptured("Random\\NPC_105.png", 28, 32, new(0.515, 0.516));
            result.AddCaptured("Random\\NPC_106.png", 34, 48, new(0.516, 0.517));
            result.AddCaptured("Random\\NPC_123.png", 36, 34, new(0.517, 0.518));
            result.AddCaptured("Random\\NPC_354.png", 35, 40, new(0.518, 0.519));
            result.AddFlower("Random\\Tiles_110.png", new(0.519, 1.0), 6, 22);

            result.SetAudio(Terraria.GetAssetPath("audio\\hallowed.mp3"));
            result.SetLiquid("Liquids\\water_4.png", "Liquids\\liquid_4.png", LiquidType.Water, 0.6f);

            result.AddWalkingNpcs(new() { NpcType.SkelletonMiner, NpcType.Elf, NpcType.Werewolf, NpcType.Dracula, NpcType.GoldenMimic, NpcType.ShadowMimic, NpcType.Wizard, NpcType.PurpleRabit, NpcType.Mummy });
            result.AddSwimmingNpcs(new() { NpcType.RedFish, NpcType.BlueFish, NpcType.GreenFish });
            result.AddFlyingNpcs(new() { NpcType.BlueBat, NpcType.BrownBat, NpcType.RedBat });

            return result;
        }

        public static Biome Hell()
        {
            Biome result = new(BiomeType.Hell);

            result.AddBackround("Backgrounds\\Background_125.png", 0.5);
            result.AddBackround("Backgrounds\\Background_127.png", 0.5);
            result.AddBackround("Backgrounds\\Background_149.png", 0.4);

            result.AddBasinTile(new BlockTilesetB(Terraria.GetImage("Blocks\\Tiles_39.png")));
            result.AddBasinTile(new BlockTilesetB(Terraria.GetImage("Blocks\\Tiles_58.png")));
            result.AddBasinTile(new BlockTilesetB(Terraria.GetImage("Blocks\\Tiles_76.png")));

            result.AddBasinWall("Walls\\Wall_96.png");
            result.AddBasinWall("Walls\\Wall_83.png");
            result.AddBasinWall("Walls\\Wall_209.png");
            result.AddBasinWall("Walls\\Wall_210.png");
            result.AddBasinWall("Walls\\Wall_269.png");

            result.AddGrass("Random\\Tiles_201.png", new(0, 0.49), 0, 5);
            result.AddGrave("Random\\Tiles_85.png", new(0.49, 0.495));
            result.AddPumpkin("Random\\Tiles_35.png", new(0.495, 0.515));
            result.AddCaptured("Random\\NPC_105.png", 28, 32, new(0.515, 0.516));
            result.AddCaptured("Random\\NPC_106.png", 34, 48, new(0.516, 0.517));
            result.AddCaptured("Random\\NPC_123.png", 36, 34, new(0.517, 0.518));
            result.AddCaptured("Random\\NPC_354.png", 35, 40, new(0.518, 0.519));

            result.AddFlower("Random\\Tiles_201.png", new(0.519, 1.0), 6, 22);

            result.SetAudio(Terraria.GetAssetPath("audio\\hell.mp3"));
            result.SetLiquid("Liquids\\water_1.png", "Liquids\\liquid_1.png", LiquidType.Lava, 1.0f);

            result.AddWalkingNpcs(new() { NpcType.Scarecrow, NpcType.Imp, NpcType.Werewolf, NpcType.Dracula, NpcType.ShadowMimic, NpcType.PurpleRabit });
            result.AddSwimmingNpcs(new() { NpcType.RedFish, NpcType.BrownFish, NpcType.GreenFish });
            result.AddFlyingNpcs(new() { NpcType.BlueBat, NpcType.BrownBat, NpcType.RedBat });

            return result;
        }

        public static Biome AdventOfCode()
        {
            Biome result = new(BiomeType.Ice);

            return result;
        }

        public static Dictionary<BiomeType, Biome> LoadOrderedBiomes()
        {
            return new()
            {
                { BiomeType.Overworld, Overworld() },
                { BiomeType.Underground, Underground() },
                { BiomeType.Mushroom, Mushroom() },
                { BiomeType.Ice, Ice() },
                { BiomeType.Hallowed, Hallowed() },
                { BiomeType.Hell, Hell() }
            };
        }

        public static Dictionary<BiomeType, Biome> LoadRandomBiomes()
        {
            Dictionary<BiomeType, Biome> result = new()
            {
                { BiomeType.Overworld, Overworld() },
                { BiomeType.Underground, Underground() }
            };

            List<BiomeType> biomeTypes = new()
            {
                BiomeType.Mushroom,
                BiomeType.Ice,
                BiomeType.Hallowed
            };

            for(int i = 0; i < biomeTypes.Count; i++)
            {
                int index = RandomGenerator.Next(0, biomeTypes.Count);

                result.Add(biomeTypes[index], GetBiomeByType(biomeTypes[index]));
                biomeTypes.RemoveAt(index);
            }

            result.Add(BiomeType.Hell, Hell());

            return result;
        }

        public void AddBackround(string path, double weight)
            => this.Backgrounds.Add((Terraria.GetImage(path), weight));

        public void AddBasinTile(IBlockTileset basinTilset)
            => this.BasinTiles.Add(basinTilset);

        public void AddBasinWall(string path)
            => this.BasinWalls.Add(new(new Tileset(Terraria.GetImage(path), 16, 16)));

        public void AddGrass(string path, Frequency frequency, int startIndex, int endIndex)
            => this.RandomAssets.Add(new SingleRowAsset(RandomAssetType.Grass, path, frequency, startIndex, endIndex));

        public void AddFlower(string path, Frequency frequency, int startIndex, int endIndex)
            => this.RandomAssets.Add(new SingleRowAsset(RandomAssetType.Flower, path, frequency, startIndex, endIndex));

        public void AddGrave(string path, Frequency frequency)
            => this.RandomAssets.Add(new GraveAsset(path, frequency));

        public void AddPumpkin(string path, Frequency frequency)
            => this.RandomAssets.Add(new PumpkinAsset(path, frequency));

        public void AddCaptured(string path, int tileWidth, int tileHeight, Frequency frequency)
            => this.RandomAssets.Add(new CapturedAsset(RandomAssetType.Captured, path, frequency, tileWidth, tileHeight));

        public void SetAudio(string path)
            => this.Audio = path;

        public void SetLiquid(string flowPath, string topPath, LiquidType liquidType, float opacity)
            => this.Liquids.Add(new(liquidType, new(Terraria.GetImage(flowPath), 16, 16), new(Terraria.GetImage(topPath), 16, 16, new(2, 0)), opacity));

        public IRandomAsset? GetRandomAssetBy(double frequency)
            => this.RandomAssets.FirstOrDefault(x => frequency >= x.Frequency.Min && frequency < x.Frequency.Max);

        public void AddWalkingNpcs(List<NpcType> npcs)
        {
            double frequency = 1.0 / npcs.Count;
            double min = 0.0;
            double max = frequency;

            for (int i = 0; i < npcs.Count; i++)
            {
                this.WalkingNpcs.Add(NpcSpawner.Create(npcs[i], new(min, max), new()));
                min += frequency;
                max += frequency;
            }
        }

        public void AddSwimmingNpcs(List<NpcType> npcs)
        {
            double frequency = 1.0 / npcs.Count;
            double min = 0.0;
            double max = frequency;

            for (int i = 0; i < npcs.Count; i++)
            {
                this.SwimmingNpcs.Add(NpcSpawner.Create(npcs[i], new(min, max), new()));
                min += frequency;
                max += frequency;
            }
        }

        public void AddFlyingNpcs(List<NpcType> npcs)
        {
            double frequency = 1.0 / npcs.Count;
            double min = 0.0;
            double max = frequency;

            for (int i = 0; i < npcs.Count; i++)
            {
                this.FlyingNpcs.Add(NpcSpawner.Create(npcs[i], new(min, max), new()));
                min += frequency;
                max += frequency;
            }
        }

        public INpc? GetWalkingNpc(double frequency, Basin basin)
        {
            long minY = Math.Min(basin.Top.Left.Y, basin.Top.Right.Y);

            if (basin.Bottom.Left.Y - minY == 2)
            {
                return this.WalkingNpcs.FirstOrDefault(
                    x => frequency >= x.SpawnFrequency.Min
                    && frequency < x.SpawnFrequency.Max
                    && x.Type != NpcType.WoodenMimic
                    && x.Type != NpcType.GoldenMimic
                    && x.Type != NpcType.ShadowMimic);
            }

            return this.WalkingNpcs.FirstOrDefault(x => frequency >= x.SpawnFrequency.Min && frequency < x.SpawnFrequency.Max);
        }

        public INpc? GetSwimmingNpc(double frequency)
            => this.SwimmingNpcs.FirstOrDefault(x => frequency >= x.SpawnFrequency.Min && frequency < x.SpawnFrequency.Max);

        public INpc? GetFlyingNpc(double frequency)
            => this.FlyingNpcs.FirstOrDefault(x => frequency >= x.SpawnFrequency.Min && frequency < x.SpawnFrequency.Max);

        public IRandomAsset? GetRandomAsset(RandomAssetType randomAssetType)
            => this.RandomAssets.FirstOrDefault(x => randomAssetType >= x.RandomAssetType);
    }
}
