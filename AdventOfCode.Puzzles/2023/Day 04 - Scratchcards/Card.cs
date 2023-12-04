namespace AdventOfCode.Puzzles._2023.Day_04___Scratchcards
{
    using AdventOfCode.Core.Extensions;

    public class Card
    {
        public Card(string line)
        {
            this.Winning = new();
            this.Numbers = new();

            string[] tokens = line.Split(":");

            this.Number = int.Parse(tokens[0].Replace("Card "));

            tokens = tokens[1].Split("|");

            foreach (string value in tokens[0].Split(" "))
            {
                if (string.IsNullOrEmpty(value.Trim()))
                {
                    continue;
                }

                this.Winning.Add(int.Parse(value));
            }

            foreach (string value in tokens[1].Split(" "))
            {
                if (string.IsNullOrEmpty(value.Trim()))
                {
                    continue;
                }

                this.Numbers.Add(int.Parse(value));
            }
        }

        public List<int> Winning { get; }

        public List<int> Numbers { get; }

        public int Number { get; }
    }
}
