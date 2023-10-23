namespace AdventOfCode.Animation.Terraria.Npcs
{
    using AdventOfCode.Animation.Terraria.Basins;
    using AdventOfCode.Animation.Terraria.Biomes;
    using AdventOfCode.Animation.Terraria.Npcs.Actions;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using AdventOfCode.Puzzles._2018.Day_17___Reservoir_Research;

    public class NpcSpawner
    {
        public const double FishSpawnFrequency = 0.2;
        public const double BirdSpawnFrequency = 0.5;
        public const double BatSpawnFrequency = 0.5;
        public const int MinBasinWidth = 5;

        public static ActionTileset GetMoveTileset(
            string path,
            int tileWidth,
            int tileHeight,
            Vector<int> offset,
            int indexStart,
            int indexEnd)
        {
            return new(
                Terraria.GetImage(path),
                tileWidth,
                tileHeight,
                offset,
                indexStart,
                indexEnd);
        }

        public static INpc Create(NpcType npcName, Frequency frequency, Vector<long> point)
        {
            return npcName switch
            {
                NpcType.Guide => new Humaniod(npcName, frequency, GetMoveTileset("NPCs\\NPC_22.png", 40, 54, new(0, 2), 2, 15)),
                NpcType.Imp => new Humaniod(npcName, frequency, GetMoveTileset("NPCs\\NPC_24.png", 38, 54, new(0, 2), 0, 9)),
                NpcType.Goblin => new Humaniod(npcName, frequency, GetMoveTileset("NPCs\\NPC_26.png", 40, 54, new(0, 2), 2, 14)),
                NpcType.SkelletonMiner => new Humaniod(npcName, frequency, GetMoveTileset("NPCs\\NPC_44.png", 40, 54, new(0, 2), 2, 14)),
                NpcType.WhiteRabit => new Bunny(npcName, frequency, GetMoveTileset("NPCs\\NPC_46.png", 30, 38, new(0, 2), 0, 6)),
                NpcType.PurpleRabit => new Bunny(npcName, frequency, GetMoveTileset("NPCs\\NPC_46.png", 30, 38, new(0, 2), 0, 6)),
                NpcType.Mummy => new Humaniod(npcName, frequency, GetMoveTileset("NPCs\\NPC_78.png", 40, 54, new(0, 2), 2, 14)),
                NpcType.WoodenMimic => new Mimic(npcName, frequency, GetMoveTileset("NPCs\\NPC_85.png", 32, 44, new(0, 2), 1, 5), 0),
                NpcType.GoldenMimic => new Mimic(npcName, frequency, GetMoveTileset("NPCs\\NPC_85.png", 32, 44, new(0, 2), 7, 11), 6),
                NpcType.ShadowMimic => new Mimic(npcName, frequency, GetMoveTileset("NPCs\\NPC_85.png", 32, 44, new(0, 2), 13, 17), 12),
                NpcType.Werewolf => new Humaniod(npcName, frequency, GetMoveTileset("NPCs\\NPC_104.png", 40, 54, new(0, 2), 2, 14)),
                NpcType.Santa => new Humaniod(npcName, frequency, GetMoveTileset("NPCs\\NPC_142.png", 42, 54, new(0, 2), 2, 15)),
                NpcType.SantaWithoutHat => new Humaniod(npcName, frequency, GetMoveTileset("NPCs\\NPC_142_Alt_1.png", 42, 54, new(0, 2), 2, 15)),
                NpcType.BluePengiun => new Humaniod(npcName, frequency, GetMoveTileset("NPCs\\NPC_148.png", 28, 40, new(0, 2), 0, 8)),
                NpcType.BlackPengiun => new Humaniod(npcName, frequency, GetMoveTileset("NPCs\\NPC_149.png", 28, 40, new(0, 2), 0, 8)),
                NpcType.Mushroom => new Humaniod(npcName, frequency, GetMoveTileset("NPCs\\NPC_160.png", 32, 58, new(0, 2), 2, 15)),
                NpcType.Squirel => new Squirrel(npcName, frequency, GetMoveTileset("NPCs\\NPC_299.png", 44, 30, new(0, 2), 1, 5)),
                NpcType.Scarecrow => new Humaniod(npcName, frequency, GetMoveTileset("NPCs\\NPC_311.png", 42, 54, new(0, 2), 2, 14)),
                NpcType.Elf => new Humaniod(npcName, frequency, GetMoveTileset("NPCs\\NPC_338.png", 46, 46, new(0, 2), 2, 14)),
                NpcType.GlowingSnail => new Snail(npcName, frequency, GetMoveTileset("NPCs\\NPC_360.png", 38, 22, new(0, 2), 0, 5)),
                NpcType.Gnome => new Humaniod(npcName, frequency, GetMoveTileset("NPCs\\NPC_624.png", 18, 40, new(0, 2), 1, 8)),
                NpcType.SkeletonMerchant => new Humaniod(npcName, frequency, GetMoveTileset("NPCs\\SkeletonMerchant.png", 44, 54, new(0, 2), 2, 15)),
                NpcType.SkeletonMerchantNoHat => new Humaniod(npcName, frequency, GetMoveTileset("NPCs\\SkeletonMerchant_Default_Party.png", 44, 54, new(0, 2), 2, 15)),
                NpcType.Wizard => new Humaniod(npcName, frequency, GetMoveTileset("NPCs\\Wizard_Default.png", 40, 60, new(0, 4), 2, 15)),
                NpcType.Dracula => new Dracula(npcName, frequency, GetMoveTileset("NPCs\\NPC_159.png", 40, 54, new(0, 2), 2, 14), GetMoveTileset("NPCs\\NPC_158.png", 46, 48, new(0, 0), 0, 3)),
                NpcType.YellowFish => new Fish(npcName, frequency, GetMoveTileset("NPCs\\NPC_230.png", 25, 26, new(0, 4), 0, 5), GetMoveTileset("NPCs\\NPC_55.png", 26, 22, new(0, 6), 0, 5)),
                NpcType.PurpleFish => new Fish(npcName, frequency, GetMoveTileset("NPCs\\NPC_230_purple.png", 25, 26, new(0, 4), 0, 5), GetMoveTileset("NPCs\\NPC_57.png", 26, 26, new(0, 2), 0, 5)),
                NpcType.GreenFish => new Fish(npcName, frequency, GetMoveTileset("NPCs\\NPC_230_green.png", 25, 26, new(0, 4), 0, 5), GetMoveTileset("NPCs\\NPC_58.png", 28, 26, new(0, 2), 0, 5)),
                NpcType.RedFish => new Fish(npcName, frequency, GetMoveTileset("NPCs\\NPC_230_red.png", 25, 26, new(0, 4), 0, 5), GetMoveTileset("NPCs\\NPC_102.png", 38, 26, new(0, 2), 0, 5)),
                NpcType.BrownFish => new Fish(npcName, frequency, GetMoveTileset("NPCs\\NPC_230_brown.png", 25, 26, new(0, 4), 0, 5), GetMoveTileset("NPCs\\NPC_241.png", 32, 28, new(0, 2), 0, 5)),
                NpcType.BlueFish => new Fish(npcName, frequency, GetMoveTileset("NPCs\\NPC_230_blue.png", 25, 26, new(0, 4), 0, 5), GetMoveTileset("NPCs\\NPC_607.png", 26, 26, new(0, 2), 0, 5)),
                NpcType.WhiteBird => new Bird(npcName, frequency, GetMoveTileset("NPCs\\NPC_74.png", 28, 24, new(0, 0), 0, 4)),
                NpcType.BlueBird => new Bird(npcName, frequency, GetMoveTileset("NPCs\\NPC_297.png", 28, 24, new(0, 0), 0, 4)),
                NpcType.RedBird => new Bird(npcName, frequency, GetMoveTileset("NPCs\\NPC_298.png", 28, 24, new(0, 0), 0, 4)),
                NpcType.BlueBat => new Bird(npcName, frequency, GetMoveTileset("NPCs\\NPC_49.png", 28, 32, new(0, 0), 0, 4)),
                NpcType.BrownBat => new Bird(npcName, frequency, GetMoveTileset("NPCs\\NPC_51.png", 28, 32, new(0, 0), 0, 4)),
                NpcType.RedBat => new Bird(npcName, frequency, GetMoveTileset("NPCs\\NPC_60.png", 28, 32, new(0, 0), 0, 4)),
                _ => throw new InvalidOperationException(),
            };
        }

        public static List<INpc> Single(
            NpcType npcName,
            List<Basin> basins,
            Dictionary<BiomeType, Biome> biomes,
            HashSet<Vector<long>> collisions,
            VectorArray<long, EntityType> map)
        {
            foreach(KeyValuePair<BiomeType, Biome> biome in biomes)
            {
                biome.Value.WalkingNpcs.Clear();
                biome.Value.SwimmingNpcs.Clear();
                biome.Value.WalkingNpcs.Add(Create(npcName, new(0, 1.0), new()));
            }

            return Multiple(basins, biomes, collisions, map);
        }

        public static List<INpc> Multiple(
            List<Basin> basins,
            Dictionary<BiomeType, Biome> biomes,
            HashSet<Vector<long>> collisions,
            VectorArray<long, EntityType> map)
        {
            List<INpc> result = new();

            foreach (Basin basin in basins)
            {
                Biome biome = biomes[basin.BiomeType];

                if (basin.IsBasin
                    && basin.Width > MinBasinWidth)
                {
                    BasinNpc(result, biome, basin, collisions, map);
                }

                if ((biome.BiomeType == BiomeType.Overworld
                    || biome.BiomeType == BiomeType.Underground)
                    && basin.Blocks.ElementAt(0).Key.Y != 1)
                {
                    BirdNpc(result, biome, basin, map);
                    continue;
                }

                if (basin != basins[^1]
                    && basin != basins[^2]
                    && !basin.Intersects)
                {
                    BatNpc(result, biome, basin, map);
                }
            }

            return result;
        }

        private static INpc SpawnWithoutCollision(
            INpc npc,
            int minX,
            int maxX,
            int y,
            HashSet<Vector<long>> collisions,
            VectorArray<long, EntityType> map)
        {
            long x = RandomGenerator.Next(minX, maxX);

            npc.Point.X = x * 16;
            npc.Point.Y = (y * 16) - (npc.TileHeight + 1);

            if (npc.IsCollision(collisions, map) != CollisionType.None)
            {
                return SpawnWithoutCollision(npc, minX, maxX, y, collisions, map);
            }

            return npc;
        }

        private static void BasinNpc(
            List<INpc> npcs,
            Biome biome,
            Basin basin,
            HashSet<Vector<long>> collisions,
            VectorArray<long, EntityType> map)
        {
            double value = RandomGenerator.NextDouble();

            INpc? npc = value >= 0.0 && value < FishSpawnFrequency
                ? biome.GetSwimmingNpc(RandomGenerator.NextDouble())
                : biome.GetWalkingNpc(RandomGenerator.NextDouble(), basin);

            if (npc == null)
            {
                return;
            }

            int minX = basin.Bottom.Left.X.ToInt() + 1;
            int maxX = basin.Bottom.Right.X.ToInt() - 2;

            int y = basin.IsBasin
                ? basin.Bottom.Left.Y.ToInt()
                : basin.Top.Left.Y.ToInt();

            npcs.Add(SpawnWithoutCollision(npc.Clone(), minX, maxX, y, collisions, map));
        }

        private static void BirdNpc(
            List<INpc> npcs,
            Biome biome,
            Basin basin,
            VectorArray<long, EntityType> map)
        {
            if (RandomGenerator.NextDouble() < BirdSpawnFrequency)
            {
                Vector<long> point = RandomGenerator.TrueOrFalse() ? basin.Top.Left : basin.Top.Right;

                if (point.Y < 2
                    || map[point + Vector<long>.North] == EntityType.Clay
                    || map[point + (Vector<long>.North * 2)] == EntityType.Clay)
                {
                    return;
                }

                INpc? npc = biome.GetFlyingNpc(RandomGenerator.NextDouble());

                if (npc == null)
                {
                    return;
                }

                npc = npc.Clone();
                npc.Point.X = point.X * 16;
                npc.Point.Y = (point.Y * 16) - (npc.TileHeight + 1);

                npcs.Add(npc);
            }
        }

        private static void BatNpc(
            List<INpc> npcs,
            Biome biome,
            Basin basin,
            VectorArray<long, EntityType> map)
        {
            if (RandomGenerator.NextDouble() < BatSpawnFrequency)
            {
                long x = RandomGenerator.Next((int)basin.Bottom.Left.X, (int)basin.Bottom.Right.X);

                Vector<long> point = new(x, basin.Bottom.Left.Y + 3);

                if (map[point + Vector<long>.South] == EntityType.Clay
                    || map[point + (Vector<long>.South * 2)] == EntityType.Clay)
                {
                    return;
                }

                INpc? npc = biome.GetFlyingNpc(RandomGenerator.NextDouble());

                if (npc == null)
                {
                    return;
                }

                npc = npc.Clone();
                npc.Point.X = point.X * 16;
                npc.Point.Y = (point.Y * 16) - (npc.TileHeight + 1);

                npcs.Add(npc);
            }
        }
    }
}
