using System.ComponentModel.DataAnnotations;

namespace AdventOfCode.Puzzles._2024.Day_22___Monkey_Market
{
    public class MonkeyMarket
    {
        private const long mod = 16777216;

        public List<long> Numbers { get; private set; } 

        public MonkeyMarket(string[] input)
        {
            this.Numbers = input.Select(x => long.Parse(x)).ToList();
        }

        private static long SecretNumber(long number)
        {
            long value = (number * 64) ^ number;
            value %= mod;
            value = ((value / 32) ^ value) % mod;
            value = ((value * 2048) ^ value) % mod;

            return value;
        }

        public long Sum()
        {
            long result = 0;

            foreach(long number in this.Numbers)
            {
                long value = number;
                for(int i = 0; i < 2000; i++)
                {
                    value = SecretNumber(value);
                }

                result += value;
            }

            return result;
        }

        public long Best()
        {
            long result = 0;
            var digits = this.Digits();
            var sequeneces = this.Sequeneces(digits);

            var commonSequences = FindCommonSubsequences(sequeneces, 4, 2);

            foreach (var commonSequence in commonSequences)
            {
                long value = 0;

                if (commonSequence.Sequence[0] == -2
                   && commonSequence.Sequence[1] == 1
                   && commonSequence.Sequence[2] == -1
                   && commonSequence.Sequence[3] == 3)
                {
                    var t = 1;
                }

                for (int i = 0; i < digits.Count; i++)
                {
                    var digit = digits[i];
                    var sequenece = sequeneces[i];

                    for (int j = 0; j < sequenece.Count - 3; j++)
                    {
                        if (sequenece[j] == commonSequence.Sequence[0]
                            && sequenece[j + 1] == commonSequence.Sequence[1]
                            && sequenece[j + 2] == commonSequence.Sequence[2]
                            && sequenece[j + 3] == commonSequence.Sequence[3])
                        {
                            value += digit[j + 4];
                            break;
                        }
                    }
                }

                if (value > result)
                {
                    result = value;
                    ////val = commonSequence;
                }
            }

            return result;
        }

        private List<List<int>> Sequeneces(List<List<int>> digits)
        {
            List<List<int>> results = [];

            foreach(var digit in digits)
            {
                List<int> result = [];

                for (int i = 0; i < digit.Count - 1; i++)
                {
                    result.Add(digit[i + 1] - digit[i]);
                }

                results.Add(result);
            }

            return results;
        }

        private List<List<int>> Digits()
        {
            List<List<int>> results = [];

            foreach(long number in this.Numbers)
            {
                List<int> result = [];
                
                long value = number;
                result.Add(int.Parse($"{value}"[^1].ToString()));

                for (int i = 0; i < 2000; i++)
                {
                    value = SecretNumber(value);
                    result.Add(int.Parse($"{value}"[^1].ToString()));
                }

                results.Add(result);
            }

            return results;
        }

        static List<(List<int> Sequence, int MatchCount)> FindCommonSubsequences(List<List<int>> lists, int k, int minMatchCount)
        {
            if (lists == null || lists.Count == 0)
                return new List<(List<int>, int)>();

            // Extract subsequences from each list
            var subsequencesWithCounts = new Dictionary<List<int>, int>(new ListComparer());

            foreach (var list in lists)
            {
                var subsequences = GetSubsequences(list, k);
                foreach (var subsequence in subsequences)
                {
                    if (!subsequencesWithCounts.ContainsKey(subsequence))
                        subsequencesWithCounts[subsequence] = 0;

                    subsequencesWithCounts[subsequence]++;
                }
            }

            // Filter sequences that meet the minimum match count
            return subsequencesWithCounts
                .Where(pair => pair.Value >= minMatchCount)
                .Select(pair => (pair.Key, pair.Value))
                .ToList();
        }

        static HashSet<List<int>> GetSubsequences(List<int> list, int k)
        {
            var subsequences = new HashSet<List<int>>(new ListComparer());
            if (list == null || list.Count < k)
                return subsequences;

            for (int i = 0; i <= list.Count - k; i++)
            {
                subsequences.Add(list.GetRange(i, k));
            }
            return subsequences;
        }
    }

    class ListComparer : IEqualityComparer<List<int>>
    {
        public bool Equals(List<int> x, List<int> y)
        {
            return x.SequenceEqual(y);
        }

        public int GetHashCode(List<int> obj)
        {
            return obj.Aggregate(17, (current, item) => current * 31 + item.GetHashCode());
        }
    }
}
