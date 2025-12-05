namespace AdventOfCode.Puzzles._2021.Day_10___Syntax_Scoring
{
    public class SyntaxScoring
    {
        private static readonly List<char> OpeningChars = new() { (char)Opening.Round, (char)Opening.Square, (char)Opening.Curly, (char)Opening.Tag };
        private static readonly List<char> ClosingChars = new() { (char)Closing.Round, (char)Closing.Square, (char)Closing.Curly, (char)Closing.Tag };
        private static readonly Dictionary<char, int> ErrorPoints = new()
        {
            { (char)Closing.Round, 3 },
            { (char)Closing.Square, 57 },
            { (char)Closing.Curly, 1197 },
            { (char)Closing.Tag, 25137 }
        };

        private static readonly Dictionary<char, int> MiddlePoints = new()
        {
            { (char)Closing.Round, 1 },
            { (char)Closing.Square, 2 },
            { (char)Closing.Curly, 3 },
            { (char)Closing.Tag, 4 }
        };

        public SyntaxScoring(string[] input)
        {
            this.Incomplete = new();

            this.BuildIncomplete(input);
        }

        private List<Node> Incomplete { get; set; }

        private int CorruptedRound { get; set; }

        private int CorruptedSquare { get; set; }

        private int CorruptedCurly { get; set; }

        private int CorruptedTag { get; set; }

        public static bool IsOpening(char value) => OpeningChars.Contains(value);

        public static bool IsClosing(char value) => ClosingChars.Contains(value);

        public static char Invert(char value)
        {
            return value switch
            {
                (char)Opening.Round => (char)Closing.Round,
                (char)Opening.Square => (char)Closing.Square,
                (char)Opening.Curly => (char)Closing.Curly,
                (char)Opening.Tag => (char)Closing.Tag,
                (char)Closing.Round => (char)Opening.Round,
                (char)Closing.Square => (char)Opening.Square,
                (char)Closing.Curly => (char)Opening.Curly,
                (char)Closing.Tag => (char)Opening.Tag,
                _ => throw new ArgumentException($"Illegal value {value}"),
            };
        }

        public static List<char> Not(char value)
            => IsClosing(value)
                 ? ClosingChars.Where(x => x != value).ToList()
                 : OpeningChars.Where(x => x != value).ToList();

        public long ErrorScore()
            => (ErrorPoints[')'] * this.CorruptedRound)
                + (ErrorPoints[']'] * this.CorruptedSquare)
                + (ErrorPoints['}'] * this.CorruptedCurly)
                + (ErrorPoints['>'] * this.CorruptedTag);

        public long MiddleScore()
        {
            List<long> totals = new();

            foreach (Node tree in this.Incomplete)
            {
                long total = 0;
                Node current = tree;

                string closing = string.Empty;

                while (current.Parent != null)
                {
                    closing += Invert(current.Value);

                    current = current.Parent;
                }

                closing += Invert(current.Value);

                foreach (char value in closing)
                {
                    total = (total * 5) + MiddlePoints[value];
                }

                totals.Add(total);
            }

            return totals.OrderBy(x => x).ElementAt(totals.Count / 2);
        }

        private void BuildIncomplete(string[] input)
        {
            this.CorruptedRound = 0;
            this.CorruptedSquare = 0;
            this.CorruptedCurly = 0;
            this.CorruptedTag = 0;

            for (int i = 0; i < input.Length; i++)
            {
                string line = input[i];
                bool isCorrupt = false;
                Node? current = null;

                for (int j = 0; j < line.Length; j++)
                {
                    if (current == null)
                    {
                        current = new(line[j], null);
                        continue;
                    }

                    if (ClosingChars.Contains(line[j]))
                    {
                        if (current.Illegalvalues.Contains(line[j]))
                        {
                            isCorrupt = true;

                            switch (line[j])
                            {
                                case (char)Closing.Round:
                                    this.CorruptedRound++;

                                    break;
                                case (char)Closing.Square:
                                    this.CorruptedSquare++;

                                    break;
                                case (char)Closing.Curly:
                                    this.CorruptedCurly++;

                                    break;
                                case (char)Closing.Tag:
                                    this.CorruptedTag++;

                                    break;
                            }

                            break;
                        }
                        else
                        {
                            current = current.Parent;

                            if (current != null)
                            {
                                current.RemoveChild();
                            }
                        }

                        continue;
                    }

                    if (OpeningChars.Contains(line[j]))
                    {
                        current = current.AddChild(line[j]);
                    }
                }

                if (current != null && !isCorrupt)
                {
                    this.Incomplete.Add(current);
                }
            }
        }
    }
}
