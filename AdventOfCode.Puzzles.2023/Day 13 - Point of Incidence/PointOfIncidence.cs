namespace AdventOfCode.Puzzles._2023.Day_13___Point_of_Incidence
{
    using System.Text;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;

    public class PointOfIncidence
    {
        public PointOfIncidence(string[] input)
        {
            this.Input = input;
            this.Patterns = ParsePatterns(input);
            this.Result = Summarize(input, smudges: 1);
        }

        public PointOfIncidence(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public long Result { get; }

        public IFrameRenderer? Renderer { get; }

        private string[] Input { get; }

        private List<string[]> Patterns { get; }

        private enum ReflectionAxis
        {
            Vertical,
            Horizontal
        }

        private sealed record ReflectionCandidate(
            ReflectionAxis Axis,
            int Index,
            int Score,
            int Mismatch,
            List<(int A, int B)> Pairs,
            List<Vector<int>> Smudges);

        public static int Diff(string a, string b)
        {
            int result = 0;
            int i = 0;
            while (i < a.Length)
            {
                if (a[i] != b[i])
                {
                    result++;
                }

                i++;
            }

            return result;
        }

        public static int Horizontal(string[] input)
        {
            int count = input[0].Length;

            for (int i = 0; i < count - 1; i++)
            {
                int mismatch = 0;
                int l = i;
                int r = i + 1;

                while (l >= 0 && r < count)
                {
                    string lCol = string.Join(string.Empty, input.Select(x => x[l]));
                    string rCol = string.Join(string.Empty, input.Select(x => x[r]));
                    mismatch += Diff(lCol, rCol);
                    l--;
                    r++;
                }

                if (mismatch == 1)
                {
                    return i + 1;
                }
            }

            return 0;
        }

        public static int Vertical(string[] input)
        {
            int count = input.Length;

            for (int i = 0; i < count - 1; i++)
            {
                int mismatch = 0;
                int l = i;
                int r = i + 1;

                while (l >= 0 && r < count)
                {
                    string lRow = input[l];
                    string rRow = input[r];
                    mismatch += Diff(lRow, rRow);
                    l--;
                    r++;
                }

                if (mismatch == 1)
                {
                    return (i + 1) * 100;
                }
            }

            return 0;
        }

        public static long Silver(string[] input)
            => Summarize(input, smudges: 0);

        public static long Gold(string[] input)
            => Summarize(input, smudges: 1);

        public PointOfIncidence RenderSilver(int holdFrames = 10)
            => this.Render(smudges: 0, title: "POINT OF INCIDENCE", holdFrames);

        public PointOfIncidence RenderGold(int holdFrames = 10)
            => this.Render(smudges: 1, title: "POINT OF INCIDENCE // SMUDGE MODE", holdFrames);

        private static long Summarize(string[] input, int smudges)
        {
            long result = 0;

            foreach (string[] pattern in ParsePatterns(input))
            {
                ReflectionCandidate? reflection = FindReflection(pattern, smudges);

                if (reflection == null)
                {
                    throw new InvalidOperationException("No reflection line was found for one of the patterns.");
                }

                result += reflection.Score;
            }

            return result;
        }

        private PointOfIncidence Render(int smudges, string title, int holdFrames)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<string[]> frames = [];
            long total = 0;

            for (int patternIndex = 0; patternIndex < this.Patterns.Count; patternIndex++)
            {
                string[] pattern = this.Patterns[patternIndex];
                List<ReflectionCandidate> candidates = GetCandidates(pattern);
                ReflectionCandidate? selected = candidates.FirstOrDefault(c => c.Mismatch == smudges);

                if (selected == null)
                {
                    throw new InvalidOperationException($"No reflection line was found for pattern {patternIndex + 1}.");
                }

                foreach (ReflectionCandidate candidate in candidates)
                {
                    frames.Add(this.BuildFrame(
                        pattern,
                        candidate,
                        selected,
                        patternIndex + 1,
                        this.Patterns.Count,
                        total,
                        $"{title} // SCANNING"));
                }

                total += selected.Score;

                for (int i = 0; i < holdFrames; i++)
                {
                    frames.Add(this.BuildFrame(
                        pattern,
                        selected,
                        selected,
                        patternIndex + 1,
                        this.Patterns.Count,
                        total,
                        smudges == 0
                            ? $"{title} // REFLECTION FOUND"
                            : $"{title} // SMUDGE FOUND"));
                }
            }

            string[] finalFrame =
            [
                $"{title}",
                string.Empty,
                $"PATTERNS {this.Patterns.Count:000}",
                $"SUMMARY  {total:000000}",
                string.Empty,
                "ANIMATION COMPLETE"
            ];

            for (int i = 0; i < holdFrames * 2; i++)
            {
                frames.Add(finalFrame);
            }

            this.RenderPaddedFrames(frames);

            return this;
        }

        private string[] BuildFrame(
            string[] pattern,
            ReflectionCandidate candidate,
            ReflectionCandidate selected,
            int patternNumber,
            int patternCount,
            long currentTotal,
            string title)
        {
            List<string> result = [];
            string axis = candidate.Axis == ReflectionAxis.Vertical ? "VERTICAL" : "HORIZONTAL";
            string line = candidate.Axis == ReflectionAxis.Vertical
                ? $"COLUMNS {candidate.Index:000}/{pattern[0].Length - 1:000}"
                : $"ROWS    {candidate.Index:000}/{pattern.Length - 1:000}";

            result.Add(title);
            result.Add($"PATTERN {patternNumber:000}/{patternCount:000} // {axis} // {line} // DIFF {candidate.Mismatch:000} // TOTAL {currentTotal:000000}");
            result.Add(string.Empty);

            if (candidate.Axis == ReflectionAxis.Vertical)
            {
                result.Add(BuildColumnGuide(pattern[0].Length, candidate.Index));
            }

            for (int y = 0; y < pattern.Length; y++)
            {
                StringBuilder sb = new();

                if (candidate.Axis == ReflectionAxis.Horizontal)
                {
                    sb.Append(candidate.Pairs.Any(p => p.A == y) ? 'v' : ' ');
                    sb.Append(candidate.Pairs.Any(p => p.B == y) ? '^' : ' ');
                }

                for (int x = 0; x < pattern[y].Length; x++)
                {
                    Vector<int> point = new(x, y);

                    if (candidate.Smudges.Contains(point))
                    {
                        sb.Append('!');
                    }
                    else
                    {
                        sb.Append(pattern[y][x]);
                    }

                    if (candidate.Axis == ReflectionAxis.Vertical && x == candidate.Index - 1)
                    {
                        sb.Append('|');
                    }
                }

                result.Add(sb.ToString());

                if (candidate.Axis == ReflectionAxis.Horizontal && y == candidate.Index - 1)
                {
                    result.Add(new string('-', pattern[0].Length + 2));
                }
            }

            result.Add(string.Empty);

            if (candidate == selected)
            {
                result.Add($"SELECTED SCORE {candidate.Score:00000}");
            }
            else
            {
                result.Add("SEARCHING FOR REQUIRED REFLECTION");
            }

            return [.. result];
        }

        private static string BuildColumnGuide(int width, int index)
        {
            StringBuilder sb = new();

            for (int x = 0; x < width; x++)
            {
                if (x == index - 1)
                {
                    sb.Append('>');
                }
                else if (x == index)
                {
                    sb.Append('<');
                }
                else
                {
                    sb.Append(' ');
                }

                if (x == index - 1)
                {
                    sb.Append('|');
                }
            }

            return sb.ToString();
        }

        private static ReflectionCandidate? FindReflection(string[] pattern, int smudges)
            => GetCandidates(pattern).FirstOrDefault(c => c.Mismatch == smudges);

        private static List<ReflectionCandidate> GetCandidates(string[] pattern)
        {
            List<ReflectionCandidate> candidates = [];

            for (int i = 0; i < pattern[0].Length - 1; i++)
            {
                candidates.Add(BuildVerticalCandidate(pattern, i));
            }

            for (int i = 0; i < pattern.Length - 1; i++)
            {
                candidates.Add(BuildHorizontalCandidate(pattern, i));
            }

            return candidates;
        }

        private static ReflectionCandidate BuildVerticalCandidate(string[] pattern, int index)
        {
            int mismatch = 0;
            int l = index;
            int r = index + 1;
            List<(int A, int B)> pairs = [];
            List<Vector<int>> smudges = [];

            while (l >= 0 && r < pattern[0].Length)
            {
                pairs.Add((l, r));

                for (int y = 0; y < pattern.Length; y++)
                {
                    if (pattern[y][l] != pattern[y][r])
                    {
                        mismatch++;
                        smudges.Add(new(l, y));
                        smudges.Add(new(r, y));
                    }
                }

                l--;
                r++;
            }

            return new(
                ReflectionAxis.Vertical,
                index + 1,
                index + 1,
                mismatch,
                pairs,
                smudges);
        }

        private static ReflectionCandidate BuildHorizontalCandidate(string[] pattern, int index)
        {
            int mismatch = 0;
            int l = index;
            int r = index + 1;
            List<(int A, int B)> pairs = [];
            List<Vector<int>> smudges = [];

            while (l >= 0 && r < pattern.Length)
            {
                pairs.Add((l, r));

                for (int x = 0; x < pattern[l].Length; x++)
                {
                    if (pattern[l][x] != pattern[r][x])
                    {
                        mismatch++;
                        smudges.Add(new(x, l));
                        smudges.Add(new(x, r));
                    }
                }

                l--;
                r++;
            }

            return new(
                ReflectionAxis.Horizontal,
                index + 1,
                (index + 1) * 100,
                mismatch,
                pairs,
                smudges);
        }

        private static List<string[]> ParsePatterns(string[] input)
        {
            List<string[]> patterns = [];
            List<string> lines = [];

            foreach (string line in input.Append(string.Empty))
            {
                if (string.IsNullOrEmpty(line))
                {
                    if (lines.Count > 0)
                    {
                        patterns.Add([.. lines]);
                        lines.Clear();
                    }

                    continue;
                }

                lines.Add(line);
            }

            return patterns;
        }

        private void RenderPaddedFrames(List<string[]> frames)
        {
            int width = frames.SelectMany(frame => frame).Max(row => row.Length);
            int height = frames.Max(frame => frame.Length);

            foreach (string[] frame in frames)
            {
                this.Renderer?.RenderFrame(
                    new Frame(PadFrame(frame, width, height)));
            }
        }

        private static string[] PadFrame(string[] frame, int width, int height)
        {
            List<string> result = [];

            foreach (string row in frame)
            {
                result.Add(row.PadRight(width, ' '));
            }

            while (result.Count < height)
            {
                result.Add(new string(' ', width));
            }

            return [.. result];
        }
    }
}