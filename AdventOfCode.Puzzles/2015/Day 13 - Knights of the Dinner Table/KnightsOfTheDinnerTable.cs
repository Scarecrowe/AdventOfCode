namespace AdventOfCode.Puzzles._2015.Day_13___Knights_of_the_Dinner_Table
{
    using AdventOfCode.Core.Extensions;

    public class KnightsOfTheDinnerTable
    {
        public KnightsOfTheDinnerTable(string[] input) => this.People = ParseGuestList(input);

        public Dictionary<string, Dictionary<string, int>> People { get; private set; }

        public static Dictionary<string, Dictionary<string, int>> ParseGuestList(string[] input)
        {
            var result = new Dictionary<string, Dictionary<string, int>>();

            foreach (string line in input)
            {
                string[] tokens = line[..^1].Split(" ");

                if (!result.ContainsKey(tokens[0]))
                {
                    result.Add(tokens[0], new());
                }

                int value = tokens[3].ToInt();

                result[tokens[0]].Add(tokens[10], tokens[2] == "gain" ? value : value * -1);
            }

            return result;
        }

        public List<List<string>> PossibleSeating()
        {
            List<string> possible = this.People.Keys.ToList();

            return possible.Permutations(possible.Count);
        }

        public KnightsOfTheDinnerTable AddPerson(string name)
        {
            this.People.Add(name, new());

            List<string> people = this.People.Keys.ToList();
            people.Remove(name);

            foreach (string person in people)
            {
                this.People[name].Add(person, 0);
                this.People[person].Add(name, 0);
            }

            return this;
        }

        public int OptimalSeating()
        {
            Dictionary<string, int> arrangements = new();

            foreach (List<string> arrangement in this.PossibleSeating())
            {
                int result = 0;

                for (int i = 0; i < arrangement.Count; i++)
                {
                    int next = ((i + 1) + arrangement.Count) % arrangement.Count;

                    result += this.People[arrangement[i]][arrangement[next]]
                        + this.People[arrangement[next]][arrangement[i]];
                }

                arrangements.Add(arrangement.Join(" -> "), result);
            }

            return arrangements.Values.Max();
        }
    }
}
