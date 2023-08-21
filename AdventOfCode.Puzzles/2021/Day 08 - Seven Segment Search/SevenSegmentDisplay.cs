namespace AdventOfCode.Puzzles._2021.Day_08___Seven_Segment_Search
{
    using AdventOfCode.Core.Extensions;

    public class SevenSegmentDisplay
    {
        public SevenSegmentDisplay(string[] input)
        {
            this.Entries = input.Select(x => new Entry(x)).ToList();
            this.Configuration = new();
        }

        public List<Entry> Entries { get; }

        public Dictionary<int, string> Configuration { get; }

        public static List<char> CalculateDisplay(List<string> signalInput)
        {
            List<int> result = new(10);
            List<char> display = new() { 'z', 'z', 'z', 'z', 'z', 'z', 'z' };

            string one = signalInput.First(x => x.Length == 2);
            string seven = signalInput.First(x => x.Length == 3);
            string remaining = RemoveChars(seven, one);

            display[0] = remaining[0];

            display[2] = one[0];
            display[5] = one[1];

            var temp = ((string[])signalInput.ToArray().Clone()).ToList();

            List<string> noOnes = signalInput.Where(x => !x.Contains(display[2])).ToList();

            if (noOnes.Count == 1)
            {
                display[2] = one[1];
                display[5] = one[0];

                noOnes = signalInput.Where(x => !x.Contains(display[2])).ToList();
            }

            string five = noOnes.First(x => x.Length == 5);
            string six = noOnes.First(x => x.Length == 6);

            string remaingFive = RemoveChars(RemoveChars(five, one), seven);
            string remaingSix = RemoveChars(RemoveChars(six, one), seven);

            string v = RemoveChars(remaingSix, remaingFive);

            display[4] = v[0];

            string r = string.Empty;

            for (int i = 0; i < temp.Count; i++)
            {
                temp[i] = RemoveChars(temp[i], one);
                temp[i] = RemoveChars(temp[i], seven);
                temp[i] = RemoveChars(temp[i], v);

                for (int j = 0; j < temp[i].Length; j++)
                {
                    if (!r.Contains(temp[i][j]))
                    {
                        r += temp[i][j];
                    }
                }
            }

            Dictionary<char, int> counts = new();

            foreach (char c in r)
            {
                counts.Add(c, 0);

                for (int i = 0; i < temp.Count; i++)
                {
                    for (int j = 0; j < temp[i].Length; j++)
                    {
                        if (temp[i][j] == c)
                        {
                            counts[c]++;
                        }
                    }
                }
            }

            var p = counts.Aggregate((l, t) => l.Value < t.Value ? l : t).Key;

            display[1] = p;

            for (int i = 0; i < temp.Count; i++)
            {
                temp[i] = RemoveChars(temp[i], p.ToString());
            }

            string four = signalInput.FirstOrDefault(x => x.Length == 4) ?? string.Empty;

            string fourRemaining = RemoveChars(four, one);
            fourRemaining = RemoveChars(fourRemaining, seven);
            fourRemaining = RemoveChars(fourRemaining, v);
            fourRemaining = RemoveChars(fourRemaining, p.ToString());

            display[3] = fourRemaining[0];

            var f = counts.FirstOrDefault(x => x.Key != p && x.Key != fourRemaining[0]).Key;

            display[6] = f;

            return display;
        }

        public int UniqueOutputValues()
        {
            int total = 0;
            List<int> counts = new() { 2, 4, 3, 7 };

            foreach (Entry entry in this.Entries)
            {
                foreach (string value in entry.OutputValues)
                {
                    if (counts.Contains(value.Length))
                    {
                        total++;
                    }
                }
            }

            return total;
        }

        public long DisplayValue()
        {
            long total = 0;

            foreach (Entry entry in this.Entries)
            {
                List<char> display = CalculateDisplay(entry.SignalPatterns);

                List<int> numbers = new() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                string zero = string.Concat($"{display[0]}{display[1]}{display[2]}{display[4]}{display[5]}{display[6]}".OrderBy(c => c));
                string one = string.Concat($"{display[2]}{display[5]}".OrderBy(c => c));
                string two = string.Concat($"{display[0]}{display[2]}{display[3]}{display[4]}{display[6]}".OrderBy(c => c));
                string three = string.Concat($"{display[0]}{display[2]}{display[3]}{display[5]}{display[6]}".OrderBy(c => c));
                string four = string.Concat($"{display[1]}{display[2]}{display[3]}{display[5]}".OrderBy(c => c));
                string five = string.Concat($"{display[0]}{display[1]}{display[3]}{display[5]}{display[6]}".OrderBy(c => c));
                string six = string.Concat($"{display[0]}{display[1]}{display[3]}{display[4]}{display[5]}{display[6]}".OrderBy(c => c));
                string seven = string.Concat($"{display[0]}{display[2]}{display[5]}".OrderBy(c => c));
                string eight = string.Concat($"{display[0]}{display[1]}{display[2]}{display[3]}{display[4]}{display[5]}{display[6]}".OrderBy(c => c));
                string nine = string.Concat($"{display[0]}{display[1]}{display[2]}{display[3]}{display[5]}{display[6]}".OrderBy(c => c));

                for (int i = 0; i < entry.SignalPatterns.Count; i++)
                {
                    string patternOrdered = entry.SignalPatterns[i].OrderBy(c => c).Join();

                    if (patternOrdered == zero)
                    {
                        numbers[i] = 0;
                        continue;
                    }

                    if (patternOrdered == one)
                    {
                        numbers[i] = 1;
                        continue;
                    }

                    if (patternOrdered == two)
                    {
                        numbers[i] = 2;
                        continue;
                    }

                    if (patternOrdered == three)
                    {
                        numbers[i] = 3;
                        continue;
                    }

                    if (patternOrdered == four)
                    {
                        numbers[i] = 4;
                        continue;
                    }

                    if (patternOrdered == five)
                    {
                        numbers[i] = 5;
                        continue;
                    }

                    if (patternOrdered == six)
                    {
                        numbers[i] = 6;
                        continue;
                    }

                    if (patternOrdered == seven)
                    {
                        numbers[i] = 7;
                        continue;
                    }

                    if (patternOrdered == eight)
                    {
                        numbers[i] = 8;
                        continue;
                    }

                    if (patternOrdered == nine)
                    {
                        numbers[i] = 9;
                        continue;
                    }
                }

                string final = string.Empty;

                foreach (string value in entry.OutputValues)
                {
                    int index = -1;

                    for (int i = 0; i < entry.SignalPatterns.Count; i++)
                    {
                        var x = entry.SignalPatterns[i].OrderBy(c => c).Join();
                        var y = value.OrderBy(c => c).Join();

                        if (x == y)
                        {
                            index = i;
                            break;
                        }
                    }

                    final += numbers[index].ToString();
                }

                total += final.ToInt();
            }

            return total;
        }

        private static string RemoveChars(string one, string two)
        {
            string result = string.Empty;

            foreach (char chr in one)
            {
                if (!two.Contains(chr))
                {
                    result += chr;
                }
            }

            return result;
        }
    }
}
