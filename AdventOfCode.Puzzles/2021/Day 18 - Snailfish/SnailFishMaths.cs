namespace AdventOfCode.Puzzles._2021.Day_18___Snailfish
{
    using AdventOfCode.Core.Extensions;

    public class SnailFishMaths
    {
        public SnailFishMaths(string[] input)
        {
            this.Input = input;
            this.AllFish = this.Parse(this.Input);
        }

        public string[] Input { get; }

        public List<SnailFish> AllFish { get; set; }

        public SnailFish? ParseFish(SnailFish? current, int i, string line, bool rightMost)
        {
            if (current == null)
            {
                current = new(null, null);
            }

            if (i >= line.Length)
            {
                return current;
            }

            switch (line[i])
            {
                case '[':
                    if (!rightMost)
                    {
                        current?.SetA(new(null, current));
                        current = current?.A;
                    }
                    else
                    {
                        current?.SetB(new(null, current));
                        current = current?.B;
                        rightMost = false;
                    }

                    return this.ParseFish(current, i + 1, line, rightMost);
                case ']':
                    rightMost = false;
                    return this.ParseFish(current?.Parent, i + 1, line, rightMost);
                case ',':
                    rightMost = true;
                    return this.ParseFish(current, i + 1, line, rightMost);
                default:
                    if (!rightMost)
                    {
                        current?.SetA(new(line[i].ToString().ToInt(), current));
                    }
                    else
                    {
                        current?.SetB(new(line[i].ToString().ToInt(), current));
                    }

                    return this.ParseFish(current, i + 1, line, rightMost);
            }
        }

        public long FinalMagnitude()
        {
            SnailFish? fish = new SnailFish(null, null, this.AllFish[0], this.AllFish[1]).Addition();
            SnailFish? addition = null;

            for (int i = 2; i < this.AllFish.Count; i++)
            {
                addition = fish;
                fish = new SnailFish(null, null, addition, this.AllFish[i]).Addition();
            }

            return addition?.Parent?.Magnitude() ?? 0;
        }

        public long LargestMagnitude()
        {
            List<long> magnitudes = new();

            for (int i = 0; i < this.Input.Length; i++)
            {
                for (int j = 0; j < this.Input.Length; j++)
                {
                    if (i != j)
                    {
                        SnailFish fishA = new(null, null);
                        SnailFish fishB = new(null, null);

                        this.ParseFish(fishA, 1, this.Input[i], false);
                        this.ParseFish(fishB, 1, this.Input[j], false);

                        magnitudes.Add(new SnailFish(null, null, fishA, fishB)?.Addition()?.Magnitude() ?? 0);
                    }
                }
            }

            return magnitudes.Max();
        }

        private List<SnailFish> Parse(string[] input)
        {
            List<SnailFish> result = new();

            foreach (string line in input)
            {
                SnailFish fish = new(null, null);
                this.ParseFish(fish, 1, line, false);
                result.Add(fish);
            }

            return result;
        }
    }
}
