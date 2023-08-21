namespace AdventOfCode.Puzzles._2018.Day_05___Alchemical_Reduction
{
    using System.Text;
    using AdventOfCode.Core.Extensions;

    public class AlchemicalReduction
    {
        public AlchemicalReduction(string input)
        {
            this.Distinct = input.Distinct().Where(x => char.IsLower(x)).ToList();
            this.Units = new(input);
        }

        public StringBuilder Units { get; }

        public List<char> Distinct { get; }

        public string React(StringBuilder? sb = null)
        {
            sb ??= this.Units;

            while (true)
            {
                bool reacted = false;

                for (int i = 0; i < sb.Length - 1; i++)
                {
                    bool isLower = char.IsLower(sb[i]);

                    if (sb[i] + (isLower ? -32 : 32) == sb[i + 1])
                    {
                        reacted = true;
                        sb.Remove(i, 2);
                    }
                }

                if (!reacted)
                {
                    return sb.ToString();
                }
            }
        }

        public int ShortestPolymer()
        {
            Dictionary<char, int> reactions = new();

            foreach (char c in this.Distinct)
            {
                StringBuilder sb = new(this.Units.ToString());

                sb = sb.Replace(c.ToString()).Replace(((char)(c - 32)).ToString());

                reactions.Add(c, this.React(sb).Length);
            }

            return reactions.Min(x => x.Value);
        }
    }
}
