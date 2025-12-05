namespace AdventOfCode.Puzzles._2024.Days
{
    // Keypad class handles layout and valid moves
    public class Keypad
    {
        private readonly char[,] layout;

        public Keypad(char[,] layout) => this.layout = layout;

        public bool IsValidPosition(int row, int col) =>
            row >= 0 && row < layout.GetLength(0) &&
            col >= 0 && col < layout.GetLength(1) &&
            layout[row, col] != ' ';

        public char GetButton(int row, int col) => layout[row, col];

        public (int Row, int Col) FindButton(char button)
        {
            for (int r = 0; r < layout.GetLength(0); r++)
            {
                for (int c = 0; c < layout.GetLength(1); c++)
                {
                    if (layout[r, c] == button) return (r, c);
                }
            }
            throw new InvalidOperationException($"Button {button} not found on keypad");
        }

        // Iterative BFS to generate all sequences from start -> target
        public IEnumerable<string> GenerateSequences((int Row, int Col) start, (int Row, int Col) target)
        {
            var queue = new Queue<(int Row, int Col, string Sequence)>();
            queue.Enqueue((start.Row, start.Col, ""));

            while (queue.Count > 0)
            {
                var (row, col, seq) = queue.Dequeue();

                if ((row, col) == target)
                {
                    yield return seq + "A";
                    continue;
                }

                foreach (var (dr, dc, dir) in new (int, int, char)[]
                {
                    (-1,0,'^'), (1,0,'v'), (0,-1,'<'), (0,1,'>')
                })
                {
                    int newRow = row + dr;
                    int newCol = col + dc;

                    if (IsValidPosition(newRow, newCol))
                    {
                        queue.Enqueue((newRow, newCol, seq + dir));
                    }
                }
            }
        }
    }
}