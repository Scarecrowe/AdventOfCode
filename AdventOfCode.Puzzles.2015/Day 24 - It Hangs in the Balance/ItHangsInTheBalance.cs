namespace AdventOfCode.Puzzles._2015.Day_24___It_Hangs_in_the_Balance
{
    using AdventOfCode.Core.Extensions;

    public class ItHangsInTheBalance
    {
        public ItHangsInTheBalance(string[] input)
        {
            this.Weights = input.ToLongList();
            this.TotalWeight = this.Weights.Sum();
        }

        public List<long> Weights { get; }

        public long TotalWeight { get; }

        public long IdealConfiguration(int groupCount)
        {
            long groupWeight = this.TotalWeight / groupCount;

            for (int i = 1; i <= this.Weights.Count; i++)
            {
                foreach (IEnumerable<long> combination in this.Weights.Combinations(i))
                {
                    if (combination.Sum() == groupWeight)
                    {
                        return combination.ToList().Product();
                    }
                }
            }

            return 0;
        }
    }
}
