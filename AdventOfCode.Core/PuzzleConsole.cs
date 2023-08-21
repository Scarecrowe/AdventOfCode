namespace AdventOfCode.Core
{
    public static class PuzzleConsole
    {
        public static bool Log { get; set; } = true;

        public static List<string> Buffer { get; private set; } = new();

        public static int Position { get; private set; } = 0;

        public static void Write(char value, int indent = 1) => Write(value.ToString(), indent);

        public static void Write(string value, int indent = 1)
        {
            if (Log)
            {
                if (Position == 0)
                {
                    Buffer.Add($"{new string(' ', indent)}{value}");
                }
                else
                {
                    Buffer.Add(value);
                }

                Position++;
            }
        }

        public static void WriteLine(int value, int indent = 1) => WriteLine(value.ToString(), indent);

        public static void WriteLine(string value = "", int indent = 1)
        {
            if (Log)
            {
                Write($"{value}{Environment.NewLine}", indent);
                Position = 0;
            }
        }

        public static void Flush()
        {
            if (!Buffer.Any())
            {
                return;
            }

            Console.Write($"{string.Join(string.Empty, Buffer)}");
            Buffer.Clear();
            Position = 0;
        }

        public static void Flush(string path)
        {
            if (!Buffer.Any())
            {
                return;
            }

            using (StreamWriter sw = new(path))
            {
                sw.Write(string.Join(string.Empty, Buffer));
            }

            Buffer.Clear();
            Position = 0;
        }

        public static void Clear()
        {
            Buffer.Clear();
            Position = 0;
        }
    }
}
