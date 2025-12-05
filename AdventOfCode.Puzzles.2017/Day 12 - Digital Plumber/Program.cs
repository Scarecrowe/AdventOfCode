namespace AdventOfCode.Puzzles._2017.Day_12___Digital_Plumber
{
    using AdventOfCode.Core.Extensions;

    public class Program
    {
        public Program(string input)
        {
            int[] tokens = input.Tokens().ToInt();

            this.Id = tokens[0];
            this.Children = new();

            for (var i = 1; i < tokens.Length; i++)
            {
                this.Children.Add(tokens[i]);
            }
        }

        public int Id { get; }

        public HashSet<int> Children { get; }

        public HashSet<int> Group(Dictionary<int, Program> programs, HashSet<int>? group = null)
        {
            if (group == null)
            {
                group = new();
            }

            group.Add(this.Id);

            foreach (var program in this.Children)
            {
                if (!group.Contains(program))
                {
                    programs[program].Group(programs, group);
                }
            }

            return group;
        }
    }
}
