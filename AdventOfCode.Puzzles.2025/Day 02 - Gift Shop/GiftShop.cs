namespace AdventOfCode.Puzzles._2025.Day_02___Gift_Shop
{
    public class GiftShop(string[] input)
    {
        public string Input { get; set; } = input[0];

        static bool IsInvalidId(string id, bool single)
        {
            int len = id.Length;

            for (int subLen = 1; subLen <= len / 2; subLen++)
            {
                if (len % subLen != 0)
                {
                    continue;
                }

                int repetitions = len / subLen;

                ReadOnlySpan<char> sub = id.AsSpan(0, subLen);

                bool match = true;

                for (int i = subLen; i < len; i += subLen)
                {
                    if (!id.AsSpan(i, subLen).SequenceEqual(sub))
                    {
                        match = false;
                        break;
                    }
                }

                if (!match)
                {
                    continue;
                }

                if (!single)
                {
                    if (repetitions >= 2)
                    {
                        return true;
                    }
                }
                else
                {
                    if (repetitions == 2)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public long SumInvalidIds(bool single = true)
        {
            long result = 0;

            foreach (string product in this.Input.Split(','))
            {
                string[] range = product.Split('-');
                long start = long.Parse(range[0]);
                long end = long.Parse(range[1]);

                for (long i = start; i <= end; i++)
                {
                    string s = i.ToString();

                    if (IsInvalidId(s, single))
                    {
                        result += i;
                    }
                }
            }

            return result;
        }
    }
}
