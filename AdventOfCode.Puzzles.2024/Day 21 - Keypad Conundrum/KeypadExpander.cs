namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Puzzles._2024.Day_21___Keypad_Conundrum;

    public class KeypadExpander
    {
        private readonly char[,] keypad;
        public readonly Dictionary<(char, char), string[]> Expansions = new();
        private readonly Dictionary<(int, int, char, char), string[]> GeneratedSequenceCache = new();

        public KeypadExpander(char[,] keypad)
        {
            this.keypad = keypad;
            this.BuildExpander();
        }

        private void BuildExpander()
        {
            int rows = keypad.GetLength(0);
            int cols = keypad.GetLength(1);

            for (int rStart = 0; rStart < rows; rStart++)
            {
                for (int cStart = 0; cStart < cols; cStart++)
                {
                    if (keypad[rStart, cStart] == ' ')
                    {
                        continue;
                    }

                    for (int rEnd = 0; rEnd < rows; rEnd++)
                    {
                        for (int cEnd = 0; cEnd < cols; cEnd++)
                        {
                            if (keypad[rEnd, cEnd] == ' ')
                            {
                                continue;
                            }

                            var startButton = keypad[rStart, cStart];
                            var endButton = keypad[rEnd, cEnd];
                            var key = (startButton, endButton);

                            if (!this.Expansions.ContainsKey(key))
                            {
                                int hDist = Math.Abs(cStart - cEnd);
                                int vDist = Math.Abs(rStart - rEnd);
                                char hDir = cStart < cEnd ? '>' : '<';
                                char vDir = rStart < rEnd ? 'v' : '^';

                                var sequences = GenerateAllSequences(hDist, vDist, hDir, vDir);
                                sequences = RemoveInvalidSequences(rStart, cStart, sequences);
                                sequences = RemoveNonoptimalSequences(sequences);

                                this.Expansions[key] = sequences;
                            }
                        }
                    }
                }
            }
        }

        public string[] ExpandSequence(string sequence)
        {
            var result = new List<string>();
            char previousButton = 'A';

            foreach (var nextButton in sequence)
            {
                if (!Expansions.TryGetValue((previousButton, nextButton), out var subSequences))
                {
                    continue;
                }

                var newSequences = result.Count == 0 ? subSequences.ToList() :
                    result.SelectMany(existing => subSequences.Select(sub => existing + sub)).ToList();

                result = newSequences;
                previousButton = nextButton;
            }

            return RemoveNonoptimalSequences(result.ToArray());
        }

        private string[] GenerateAllSequences(int hDist, int vDist, char hDir, char vDir)
        {
            if (hDist == 0 && vDist == 0)
            {
                return ["A"];
            }

            var key = (hDist, vDist, hDir, vDir);

            if (this.GeneratedSequenceCache.TryGetValue(key, out var cached))
            {
                return cached;
            }

            var sequences = new List<string>();

            if (hDist > 0)
            {
                sequences.AddRange(GenerateAllSequences(hDist - 1, vDist, hDir, vDir).Select(s => hDir + s));
            }

            if (vDist > 0)
            {
                sequences.AddRange(GenerateAllSequences(hDist, vDist - 1, hDir, vDir).Select(s => vDir + s));
            }

            var result = sequences.ToArray();

            this.GeneratedSequenceCache[key] = result;

            return result;
        }

        private string[] RemoveInvalidSequences(int startRow, int startCol, string[] sequences)
        {
            return sequences.Where(seq => IsValidSequence(startRow, startCol, seq)).ToArray();
        }

        private bool IsValidSequence(int startRow, int startCol, string sequence)
        {
            int r = startRow, c = startCol;

            foreach (var dir in sequence)
            {
                switch (dir)
                {
                    case '^': r--; break;
                    case 'v': r++; break;
                    case '<': c--; break;
                    case '>': c++; break;
                    case 'A': return true;
                }

                if (r < 0 || r >= keypad.GetLength(0) || c < 0 || c >= keypad.GetLength(1))
                {
                    return false;
                }

                if (keypad[r, c] == ' ')
                {
                    return false;
                    }
            }

            return true;
        }

        private static string[] RemoveNonoptimalSequences(string[] sequences)
        {
            if (sequences.Length == 0)
            {
                return sequences;
            }

            int minScore = KeypadConundrum.ComputeSequenceScore(sequences.Min());

            return sequences.Where(s => KeypadConundrum.ComputeSequenceScore(s) == minScore).ToArray();
        }
    }
}
