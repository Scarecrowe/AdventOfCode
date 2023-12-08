namespace AdventOfCode.Puzzles._2023.Day_08___Haunted_Wasteland
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class HauntedWasteland
    {
        public HauntedWasteland(string[] input)
        {
            this.Nodes = this.Parse(input);
            this.Instructions = input[0];
        }

        public string Instructions { get; }

        public Dictionary<string, (string Left, string Right)> Nodes { get; }

        public int Move() => this.Move("AAA", (x) => x != "ZZZ");

        public long MoveGhost()
            => this.Nodes
                .Where(x => x.Key.EndsWith("A"))
                .Aggregate(1L, (total, next) => MathHelper.LeastCommonMultiple(total, this.Move(next.Key, (x) => !x.EndsWith("Z"))));

        private int Move(string name, Func<string, bool> iterator)
        {
            string node = name;
            int result = 0;

            while(iterator(node))
            {
                char instruction = this.Instructions[result % this.Instructions.Length];
                node = instruction == 'L' ? this.Nodes[node].Left : this.Nodes[node].Right;
                result++;
            }

            return result++;
        }

        private Dictionary<string, (string Left, string Right)> Parse(string[] input)
        {
            Dictionary<string, (string Left, string Right)> result = new();

            for (int i = 2; i < input.Length; i++)
            {
                string[] tokens = input[i].Replace("=").Replace("(").Replace(",").Replace(")").Split(" ", StringSplitOptions.RemoveEmptyEntries);
                result.Add(tokens[0], (tokens[1], tokens[2]));
            }

            return result;
        }
    }
}
