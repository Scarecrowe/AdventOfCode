namespace AdventOfCode.Puzzles._2022.Day_24___Blizzard_Basin
{
    using AdventOfCode.Core;

    public class BlizzardBasin
    {
        public BlizzardBasin(string[] input)
        {
            this.Start = new(0, 0);
            this.Finish = new(0, 0);
            this.Maps = new();
            this.Parse(input);
        }

        private Vector<int> Start { get; set; }

        private Vector<int> Finish { get; set; }

        private BlizzardBasinMaps Maps { get; set; }

        public int FewestMinutes()
            => new BlizzardBasinState(0, this.Start).Move(this.Finish, this.Maps).Minutes;

        public int FewestMinutesWithRoundTrip()
            => new BlizzardBasinState(0, this.Start).Move(this.Finish, this.Maps).Move(this.Start, this.Maps).Move(this.Finish, this.Maps).Minutes;

        private void Parse(string[] input)
        {
            List<VectorArray<int, BlizzardBasinType>> maps = new();
            List<Blizzard> blizzards = Blizzard.Parse(input);
            VectorArray<int, BlizzardBasinType> map = BlizzardBasinMaps.Build(input, blizzards);
            string initialKey = map.ToString((c) => (char)c);

            while (true)
            {
                maps.Add(map);
                blizzards = blizzards.Select(b => b.Move()).ToList();
                map = BlizzardBasinMaps.Build(input, blizzards);

                if (map.ToString((c) => (char)c) == initialKey)
                {
                    break;
                }
            }

            this.Maps = new BlizzardBasinMaps(maps);
            this.Start = new Vector<int>(1, 0);
            this.Finish = new Vector<int>(this.Maps.Point.X - 2, this.Maps.Point.Y - 1);
        }
    }
}
