namespace AdventOfCode.Puzzles._2015.Day_15___Science_for_Hungry_People
{
    using AdventOfCode.Core.Extensions;

    public class ScienceForHungryPeople
    {
        public ScienceForHungryPeople(string[] input) => this.Ingridents = Parse(input);

        public Dictionary<string, Ingrident> Ingridents { get; }

        public static Dictionary<string, Ingrident> Parse(string[] input)
        {
            Dictionary<string, Ingrident> result = new();

            foreach (string line in input)
            {
                string[] name = line.Split(":");
                string[] properties = name[1].SplitSpace();

                Ingrident ingrident = new(
                    name[0],
                    properties[1][..^1].ToInt(),
                    properties[3][..^1].ToInt(),
                    properties[5][..^1].ToInt(),
                    properties[7][..^1].ToInt(),
                    properties[9].ToInt());

                result.Add(ingrident.Name, ingrident);
            }

            return result;
        }

        public int Mix(int[] mixture, int requiredCalories = -1)
        {
            int capacity = 0;
            int durability = 0;
            int flavor = 0;
            int texture = 0;
            int calories = 0;

            for (int i = 0; i < mixture.Length; i++)
            {
                Ingrident ingrident = this.Ingridents.ElementAt(i).Value;

                capacity += ingrident.Capacity * mixture[i];
                durability += ingrident.Durability * mixture[i];
                flavor += ingrident.Flavor * mixture[i];
                texture += ingrident.Texture * mixture[i];
                calories += ingrident.Calories * mixture[i];
            }

            if (requiredCalories == -1
                || (requiredCalories > -1 && calories == requiredCalories))
            {
                return Math.Max(0, capacity) * Math.Max(0, durability) * Math.Max(0, flavor) * Math.Max(0, texture);
            }

            return 0;
        }

        public long HighestRankingMixture(int teaspoons, int requiredCalories = -1)
        {
            int[] mixture = new int[this.Ingridents.Count];
            Dictionary<string, int> mixtures = new();

            while (true)
            {
                for (int i = 0; i < this.Ingridents.Count; i++)
                {
                    mixture[i]++;

                    if (mixture[i] > teaspoons)
                    {
                        mixture[i] = 0;
                    }
                    else
                    {
                        break;
                    }
                }

                if (mixture.Sum() != teaspoons)
                {
                    continue;
                }

                string key = mixture.Join(" ");

                if (mixtures.ContainsKey(key))
                {
                    break;
                }

                mixtures.Add(key, this.Mix(mixture, requiredCalories));
            }

            return mixtures.Values.Max();
        }
    }
}
