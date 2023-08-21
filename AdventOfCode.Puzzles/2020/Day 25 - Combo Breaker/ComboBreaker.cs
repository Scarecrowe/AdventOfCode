namespace AdventOfCode.Puzzles._2020.Day_25___Combo_Breaker
{
    using AdventOfCode.Core.Extensions;

    public class ComboBreaker
    {
        public static long EncryptiongKey(string[] input)
        {
            long[] keys = input.ToLong();

            return PrivateKey(keys[0], FindLoopSize(7, keys[1]));
        }

        private static long FindLoopSize(int subject, long publicKey)
        {
            long value = 1;
            int loopSize = 1;

            while (true)
            {
                value *= subject;
                value %= 20201227;

                if (value == publicKey)
                {
                    return loopSize;
                }

                loopSize++;
            }
        }

        private static long PrivateKey(long subject, long loopSize)
        {
            long key = 1;

            for (int i = 1; i <= loopSize; i++)
            {
                key *= subject;
                key %= 20201227;
            }

            return key;
        }
    }
}
