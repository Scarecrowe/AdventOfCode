namespace AdventOfCode.Puzzles._2025.Day_06___Trash_Compactor
{
    public class TrashCompactor(string[] input)
    {
        private string[] Input { get; set; } = input;

        public long Sum()
        {
            List<List<string>> rows = this.Input
            .Select(line =>
                line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList()
            )
            .ToList();

            int max = rows.Max(r => r.Count);
            List<List<string>> columns = [];

            for (int i = 0; i < max; i++)
            {
                columns.Add(rows
                    .Select(x => x.Count > i ? x[i] : string.Empty)
                    .ToList());
            }

            long result = 0;

            for (int i = 0; i < columns.Count; i++)
            {
                List<string> column = columns[i];               
                List<long> numbers = column.Take(column.Count - 1).Select(long.Parse).ToList();

                result += column.Last() == "*"
                    ? numbers.Aggregate(1L, (a, b) => a * b)
                    : numbers.Sum();
            }

            return result;
        }

        public long RightToLeftSum()
        {
            long result = 0;
            int columns = this.Input.Max(l => l.Length);
            int rows = this.Input.Length;
            this.PadInput(columns);

            foreach (var (start, end) in this.Seperators(columns, rows))
            {
                List<long> numbers = this.Numbers(start, end, rows);

                if (numbers.Count == 0)
                {
                    continue;
                }

                result += this.Operator(start, end, rows) == '+'
                    ? numbers.Aggregate(0L, (a, b) => a + b)
                    : numbers.Aggregate(1L, (a, b) => a * b);
            }

            return result;
        }

        private bool IsSeparatorColumn(int x, int rows)
        {
            for (int y = 0; y < rows; y++)
            {
                if (this.Input[y][x] != ' ')
                {
                    return false;
                }                    
            }
                
            return true;
        }

        private void PadInput(int width)
        {
            for (int i = 0; i < this.Input.Length; i++)
            {
                if (this.Input[i].Length < width)
                {
                    this.Input[i] = this.Input[i].PadRight(width, ' ');
                }
            }
        }

        private List<(int start, int end)> Seperators(int columns, int rows)
        {
            List<(int start, int end)> blocks = [];
            int index = 0;

            while (index < columns)
            {
                while (index < columns && this.IsSeparatorColumn(index, rows))
                {
                    index++;
                }

                if (index >= columns)
                {
                    break;
                }

                int start = index;

                while (index < columns && !this.IsSeparatorColumn(index, rows))
                {
                    index++;
                }

                blocks.Add((start, index - 1));
            }

            return blocks;
        }

        private char Operator(int start, int end, int rows)
        {
            char result = ' ';

            for (int i = start; i <= end; i++)
            {
                char ch = this.Input[rows - 1][i];

                if (ch != ' ')
                {
                   return ch;
                }
            }

            return result;
        }

        private List<long> Numbers(int start, int end, int rows)
        {
            List<long> result = [];

            for (int x = start; x <= end; x++)
            {
                List<char> chars = [];

                for (int y = 0; y < rows - 1; y++)
                {
                    char ch = this.Input[y][x];

                    if (ch != ' ')
                    {
                        chars.Add(ch);
                    }
                }

                if (chars.Count == 0)
                {
                    continue;
                }

                string number = new(chars.ToArray().Where(char.IsDigit).ToArray());

                if (number.Length == 0)
                {
                    continue;
                }

                result.Add(long.Parse(number));
            }

            return result;
        }
    }
}
