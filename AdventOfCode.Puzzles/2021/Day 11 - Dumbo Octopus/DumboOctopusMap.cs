namespace AdventOfCode.Puzzles._2021.Day_11___Dumbo_Octopus
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class DumboOctopusMap : VectorDictionary<int, DumboOctopus>
    {
        public DumboOctopusMap(Cavern cavern, string[] input)
        {
            this.Cavern = cavern;
            this.ParseInput(input);
        }

        public int Flashes { get; private set; }

        public Cavern Cavern { get; }

        public void IncrementFlashes() => this.Flashes++;

        private void ParseInput(string[] input)
        {
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[0].Length; x++)
                {
                    this.Add(new(x, y), new(new(x, y), int.Parse($"{input[y][x]}"), this.Cavern));
                }
            }
        }
    }
}
