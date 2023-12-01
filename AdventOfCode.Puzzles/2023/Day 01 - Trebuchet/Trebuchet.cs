namespace AdventOfCode.Puzzles._2023.Day_01___Trebuchet
{
    public class Trebuchet
    {
        public Trebuchet(string[] input, bool includeWords = true)
        {
            this.IncludeWords = includeWords;
            this.Values = new();
            this.Map = new();

            foreach(string value in input)
            {
                this.Values.Add(this.Parse(value));
            }
        }

        private bool IncludeWords { get; }

        private List<int> Values { get; }

        private Dictionary<int, int> Map { get; set; }

        public int Sum() => this.Values.Sum();

        private int Parse(string input)
        {
            this.Map.Clear();
            this.GetDigits(input);

            if (this.IncludeWords)
            {
                this.GetWord(input, "one", 1);
                this.GetWord(input, "two", 2);
                this.GetWord(input, "three", 3);
                this.GetWord(input, "four", 4);
                this.GetWord(input, "five", 5);
                this.GetWord(input, "six", 6);
                this.GetWord(input, "seven", 7);
                this.GetWord(input, "eight", 8);
                this.GetWord(input, "nine", 9);
            }

            this.Map = this.Map.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            int first = this.Map.ElementAt(0).Value;
            return int.Parse($"{first}{(this.Map.Count == 1 ? first : this.Map.Last().Value)}");
        }

        private void GetDigits(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsDigit(input[i]))
                {
                    this.Map.Add(i, int.Parse($"{input[i]}"));
                }
            }
        }

        private void GetWord(string input, string word, int value)
        {
            if (input.Contains(word))
            {
                int i = input.IndexOf(word, 0);

                while (i != -1)
                {
                    this.Map.Add(i, value);

                    i = input.IndexOf(word, i + 1);
                }
            }
        }
    }
}
