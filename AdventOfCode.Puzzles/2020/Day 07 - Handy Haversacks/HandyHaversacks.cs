namespace AdventOfCode.Puzzles._2020.Day_07___Handy_Haversacks
{
    using AdventOfCode.Core.Extensions;

    public class HandyHaversacks : Dictionary<string, Bag>
    {
        public const string ShinyGold = "shiny gold";

        public const string NoBag = "no other bags.";

        private List<string> available = new() { ShinyGold };

        public HandyHaversacks(string[] input) => this.Parse(input);

        public int ShinyGoldCount()
        {
            int count = 0;

            foreach (KeyValuePair<string, Bag> outer in this)
            {
                List<string> counted = new();

                bool hasBag = false;

                this.HasShinyGold(outer.Value, this, ref this.available, ref hasBag, ref counted);

                if (hasBag)
                {
                    count++;
                }
            }

            return count;
        }

        public int RequiredBags()
        {
            Bag shinyGold = this[ShinyGold];

            int count = 0;

            foreach (Bag child in shinyGold.Children)
            {
                count += child.Count * this.RequiredCount(this[child.Name], this);
            }

            return count;
        }

        private void Parse(string[] input)
        {
            foreach (string value in input)
            {
                string[] tokens = value.Split(" contain ");
                string colour = tokens[0].Replace(" bags");

                if (!this.ContainsKey(colour))
                {
                    this.Add(colour, new(colour, true));
                }

                Bag bag = this[colour];

                tokens = tokens[1].Split(",");

                foreach (string part in tokens)
                {
                    if (tokens[0] == NoBag)
                    {
                        continue;
                    }

                    string childColour = part.Replace(" bags").Replace(" bag").Replace(".").Trim();

                    int count = childColour[..childColour.IndexOf(" ")].ToInt();

                    childColour = childColour[(childColour.IndexOf(" ") + 1) ..];

                    if (childColour == ShinyGold)
                    {
                        this.available.Add(colour);
                    }

                    bag.Children.Add(new(childColour, false, count));
                }
            }
        }

        private int RequiredCount(Bag current, Dictionary<string, Bag> bags)
        {
            int count = 1;

            foreach (Bag child in current.Children)
            {
                if (bags.ContainsKey(child.Name))
                {
                    count += child.Count * this.RequiredCount(bags[child.Name], bags);
                }
            }

            return count;
        }

        private void HasShinyGold(Bag current, Dictionary<string, Bag> bags, ref List<string> available, ref bool hasBag, ref List<string> counted)
        {
            if (hasBag)
            {
                return;
            }

            foreach (Bag inner in current.Children)
            {
                if (hasBag)
                {
                    return;
                }

                if (available.Contains(inner.Name))
                {
                    hasBag = true;
                    return;
                }

                if (bags.ContainsKey(inner.Name) && !inner.Outer)
                {
                    this.HasShinyGold(bags[inner.Name], bags, ref available, ref hasBag, ref counted);
                }
            }
        }
    }
}
