namespace AdventOfCode.Puzzles._2015.Day_09___All_in_a_Single_Night
{
    using AdventOfCode.Core.Extensions;

    public class Journey
    {
        public Journey(string source, string destination, string distance)
        {
            this.Source = source;
            this.Destination = destination;
            this.Distance = distance.ToInt();
        }

        public string Source { get; }

        public string Destination { get; }

        public int Distance { get; }
    }
}
