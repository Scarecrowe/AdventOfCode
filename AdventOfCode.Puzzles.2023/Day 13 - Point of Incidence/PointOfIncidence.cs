namespace AdventOfCode.Puzzles._2023.Day_13___Point_of_Incidence
{
    using AdventOfCode.Core;

    public class PointOfIncidence
    {
        public PointOfIncidence(string[] input)
        {
            List<string> lines = input.ToList();
            lines.Add(string.Empty);
            input = [.. lines];
            lines.Clear();
            long result = 0;

            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    int a = Horizontal([.. lines]);
                    int b = Vertical([.. lines]);
                    result += a + b;

                    lines.Clear();
                    continue;
                }

                lines.Add(line);
            }
        }

        public static int Diff(string a, string b)
        {
            int result = 0;
            int i = 0;
            while(i < a.Length)
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
    }
}
