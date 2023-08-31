namespace AdventOfCode.Core.Extensions
{
    using System.Collections;

    public static class ListExtensions
    {
        public static void Fill(this List<long> list, long min, long max)
        {
            for (long i = min; i <= max; i++)
            {
                list.Add(i);
            }
        }

        public static void Fill(this List<int> list, int min, int max)
        {
            for (int i = min; i <= max; i++)
            {
                list.Add(i);
            }
        }

        public static void FillWith(this List<int> list, int value, int max)
        {
            if (max == 0)
            {
                return;
            }

            for (int i = 0; i <= max; i++)
            {
                list.Add(value);
            }
        }

        public static List<List<T>> Permutations<T>(this List<T> list, int length)
        {
            if (length == 0)
            {
                throw new ArgumentException();
            }

            if (length == 1)
            {
                return list
                    .Select(t => new List<T> { t })
                    .ToList();
            }

            return Permutations(list, length - 1)
                .SelectMany(
                    t => list.Where(o => !t.Contains(o)).ToList(),
                    (t1, t2) => t1.Concat(new List<T> { t2 }).ToList())
                .ToList();
        }

        public static IEnumerable<string> CombinationsWithRepetition(this IEnumerable<int> list, int length)
        {
            if (length <= 0)
            {
                yield return string.Empty;
            }
            else
            {
                foreach (var i in list)
                {
                    foreach (var c in CombinationsWithRepetition(list, length - 1))
                    {
                        yield return $"{i}{c}";
                    }
                }
            }
        }

        public static List<List<long>> CombinationsOfTotal(this List<long> list, long total)
        {
            {
                List<List<long>> result = new();

                for (int i = 0; i < (1 << list.Count); ++i)
                {
                    List<long> combination = new();

                    for (int j = 0; j < list.Count; ++j)
                    {
                        if ((i & (1 << j)) != 0)
                        {
                            combination.Add(list[j]);
                        }
                    }

                    if (combination.Sum() == total)
                    {
                        result.Add(combination);
                    }
                }

                return result;
            }
        }

        public static IEnumerable Combinations<T>(this List<T> elements, int length)
        {
            var elem = elements.ToArray();
            var size = elem.Length;

            if (length > size)
            {
                yield break;
            }

            var numbers = new int[length];

            for (var i = 0; i < length; i++)
            {
                numbers[i] = i;
            }

            do
            {
                yield return numbers.Select(n => elem[n]);
            }
            while (NextCombination(numbers, size, length));
        }

        public static List<List<T>> Combinations<T>(this List<T> list)
        {
            List<List<T>> result = new();

            if (list.Count == 0)
            {
                return result;
            }

            for (int i = 0; i < (1 << list.Count); ++i)
            {
                List<T> combination = new();

                for (int j = 0; j < list.Count; ++j)
                {
                    if ((i & (1 << j)) != 0)
                    {
                        combination.Add(list[j]);
                    }
                }

                result.Add(combination);
            }

            return result;
        }

        public static int Product(this List<int> list)
        {
            if (!list.Any())
            {
                return 0;
            }

            int product = list[0];

            for (int i = 1; i < list.Count; i++)
            {
                product *= list[i];
            }

            return product;
        }

        public static long Product(this List<long> list)
        {
            if (!list.Any())
            {
                return 0;
            }

            long product = list[0];

            for (int i = 1; i < list.Count; i++)
            {
                product *= list[i];
            }

            return product;
        }

        public static void RemoveRange<T>(this List<T> list, List<T> range)
        {
            foreach (T item in range)
            {
                list.Remove(item);
            }
        }

        private static bool NextCombination(IList<int> num, int n, int k)
        {
            bool finished;

            var changed = finished = false;

            for (var i = k - 1; !finished && !changed; i--)
            {
                if (num[i] < n - 1 - (k - 1) + i)
                {
                    num[i]++;

                    if (i < k - 1)
                    {
                        for (var j = i + 1; j < k; j++)
                        {
                            num[j] = num[j - 1] + 1;
                        }
                    }

                    changed = true;
                }

                finished = i == 0;
            }

            return changed;
        }
    }
}
