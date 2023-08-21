namespace AdventOfCode.Puzzles._2015.Day_04___The_Ideal_Stocking_Stuffer
{
    using AdventOfCode.Core.Extensions;

    public class TheIdealStockingStuffer
    {
        public TheIdealStockingStuffer(string secret, int leadingZeros)
        {
            while (true)
            {
                if ($"{secret}{this.Number}".ToMd5().TakeWhile(c => c == '0').Count() == leadingZeros)
                {
                    break;
                }

                this.Number++;
            }
        }

        public int Number { get; }
    }
}
