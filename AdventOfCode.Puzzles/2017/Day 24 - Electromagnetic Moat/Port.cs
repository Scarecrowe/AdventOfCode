namespace AdventOfCode.Puzzles._2017.Day_24___Electromagnetic_Moat
{
    using AdventOfCode.Core.Extensions;

    public class Port
    {
        public Port(string line)
        {
            List<int> tokens = line.Split("/").ToIntList();

            this.Left = tokens[0];
            this.Right = tokens[1];
        }

        public int Left { get; }

        public int Right { get; }
    }
}
