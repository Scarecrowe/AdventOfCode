namespace AdventOfCode.Puzzles._2018.Day_12___Subterranean_Sustainability
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class SubterraneanSustainability
    {
        public SubterraneanSustainability(string[] input)
        {
            this.Rules = new();
            string state = input[0].Replace("initial state: ");

            this.IntialState = new NegativeIndexArray<bool>(state.Length + 4, -2);

            for (int i = 0; i < state.Length; i++)
            {
                this.IntialState[i] = state[i] == '#';
            }

            for (int i = 2; i < input.Length; i++)
            {
                string[] tokens = input[i].Split(" => ");
                this.Rules.Add(tokens[0], tokens[1][0] == '#');
            }
        }

        public NegativeIndexArray<bool> IntialState { get; private set; }

        public Dictionary<string, bool> Rules { get; }

        public long Grow(long generations)
        {
            int min = this.IntialState.Min;
            int max = this.IntialState.Max + 1;

            NegativeIndexArray<bool> last = this.IntialState;
            HashSet<string> states = new();

            for (int i = 1; i <= generations; i++)
            {
                NegativeIndexArray<bool> current = new(max, min);

                for (int j = min; j <= max; j++)
                {
                    string rule = PotToRule(j, ref last);

                    if (this.Rules.ContainsKey(rule))
                    {
                        if (this.Rules[rule])
                        {
                            current[j] = this.Rules[rule];
                        }
                    }
                    else
                    {
                        current[j] = true;
                    }
                }

                string value = current.Values.Select(x => x ? '#' : '.').Join().Strip('.');

                if (!states.Contains(value))
                {
                    states.Add(value);
                }
                else
                {
                    return SumOfPlants(ref current, generations, i);
                }

                if (current[current.Length - (min * -1) - 1])
                {
                    max += 2;
                }

                if (current[min])
                {
                    min -= 2;
                }

                last = current;
            }

            return SumOfPlants(ref last, 0, 0);
        }

        private static string PotToRule(int index, ref NegativeIndexArray<bool> state)
        {
            char ll = !state.HasIndex(index - 2) ? '.' : state[index - 2] ? '#' : '.';
            char l = !state.HasIndex(index - 1) ? '.' : state[index - 1] ? '#' : '.';
            char c = !state.HasIndex(index) ? '.' : state[index] ? '#' : '.';
            char r = !state.HasIndex(index + 1) ? '.' : state[index + 1] ? '#' : '.';
            char rr = !state.HasIndex(index + 2) ? '.' : state[index + 2] ? '#' : '.';

            return $"{ll}{l}{c}{r}{rr}";
        }

        private static long SumOfPlants(ref NegativeIndexArray<bool> state, long generations, long stable)
        {
            long result = 0;

            for (int j = state.Min; j < state.Length; j++)
            {
                if (state.HasIndex(j) && state[j])
                {
                    result += j + (generations - stable);
                }
            }

            return result;
        }
    }
}
