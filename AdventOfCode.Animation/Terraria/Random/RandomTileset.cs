namespace AdventOfCode.Animation.Terraria.Random
{
    public class RandomTileset
    {
        public RandomTileset(RandomAssetType randomType, Tileset tileset, int startIndex, int endIndex)
        {
            this.RandomType = randomType;
            this.Tileset = tileset;
            this.StartIndex = startIndex;
            this.EndIndex = endIndex;
        }

        public RandomAssetType RandomType { get; }

        public Tileset Tileset { get; }

        public int StartIndex { get; }

        public int EndIndex { get; }
    }
}
