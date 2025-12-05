namespace AdventOfCode.Puzzles._2024.Day_21___Keypad_Conundrum
{
    using AdventOfCode.Puzzles._2024.Days;

    public class KeypadConundrum(string[] input)
    {
        private static readonly Dictionary<string, int> SequenceScoreCache = new();
        private static readonly Dictionary<(object, char, char, int), long> SequenceLengthCache = new();

        private readonly KeypadExpander numericExpander = new KeypadExpander(NumericKeypad);
        private readonly KeypadExpander directionalExpander = new KeypadExpander(DirectionalKeypad);

        private static readonly char[,] NumericKeypad =
        {
            { '7','8','9' },
            { '4','5','6' },
            { '1','2','3' },
            { ' ','0','A' }
        };

        private static readonly char[,] DirectionalKeypad =
        {
            { ' ','^','A' },
            { '<','v','>' }
        };

        public string Complexity(int robots)
        {
            long totalScore = 0;

            foreach (var code in input)
            {
                var expandedSequences = numericExpander.ExpandSequence(code);
                var minLength = expandedSequences.Min(seq =>
                    ComputeExpandedSequenceLength(directionalExpander, seq, robots));

                int numericValue = int.Parse(code.Substring(0, 3));
                totalScore += minLength * numericValue;
            }

            return totalScore.ToString();
        }

        private static long ComputeExpandedSequenceLength(KeypadExpander expander, string sequence, int count)
        {
            if (count == 0)
            {
                return sequence.Length;
            }

            char previousButton = 'A';
            long totalLength = 0;

            foreach (var nextButton in sequence)
            {
                if (!SequenceLengthCache.TryGetValue((expander, previousButton, nextButton, count), out var length))
                {
                    var key = (previousButton, nextButton);

                    if (!expander.Expansions.TryGetValue(key, out var expandedSequences))
                    {
                        throw new InvalidOperationException($"No sequence found for key: {key}");
                    }

                    length = expandedSequences.Min(seq => ComputeExpandedSequenceLength(expander, seq, count - 1));
                    SequenceLengthCache[(expander, previousButton, nextButton, count)] = length;
                }

                totalLength += length;
                previousButton = nextButton;
            }

            return totalLength;
        }

        public static int ComputeSequenceScore(string sequence)
        {
            if (SequenceScoreCache.TryGetValue(sequence, out var score))
            {
                return score;
            }

            score = 0;
            char prev = 'A';

            foreach (var ch in sequence)
            {
                if (ch != prev) score++;
            }

            SequenceScoreCache[sequence] = score;

            return score;
        }
    }
}
