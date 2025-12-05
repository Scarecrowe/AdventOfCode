namespace AdventOfCode.Core
{
    using System.Reflection;
    using AdventOfCode.Core.Contracts;
    using AdventOfCode.Core.Extensions;

    public abstract class Puzzle
    {
        protected Puzzle()
        {
            this.FilePath = string.Empty;
            this.Input = Array.Empty<string>();
            this.DayTitle = string.Empty;
        }

        public string FilePath { get; protected set; }

        public string[] Input { get; protected set; }

        public string DayTitle { get; protected set; }

        public static IPuzzle? GetPuzzle(int year, int day, string input, bool split = true, bool trim = true, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            if (split)
            {
                return GetPuzzle(year, day, input.Split("\r\n", options).Select(x => trim ? x.Trim() : x).ToList());
            }

            return GetPuzzle(year, day, new List<string> { trim ? input.Trim() : input });
        }

        public static IPuzzle? GetPuzzle(int year, int day, List<string>? input = null)
        {
            Type? puzzle = Assembly
                .Load($"AdventOfCode.Puzzles.{year}")
                .GetType($"AdventOfCode.Puzzles._{year}.Days.Day{day}");

            if ((year < 2015 || year > 2025)
                || (day < 1 || day > 25)
                || puzzle == null)
            {
                throw new ArgumentException();
            }

            if (input != null)
            {
                return Activator.CreateInstance(puzzle) as IPuzzle;
            }

            return Activator.CreateInstance(puzzle) as IPuzzle;
        }

        public static string[] GetInput(int day, string title, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            return File.ReadAllText(GetInputPath(day, title)).Split(
                new[] { "\r\n", "\r", "\n" },
                options);
        }

        protected static string GetInputPath(int day, string title)
        {
            string dayPath = $"{Assembly.GetCallingAssembly().ExecutingDirectory()}\\Day {(day < 10 ? $"0{day}" : $"{day}")} - {title}";

            return $"{dayPath}\\input.txt";
        }

        protected void GetPuzzleData(int day, string title, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries, bool split = true)
        {
            this.FilePath = GetInputPath(day, title);

            if (!File.Exists(this.FilePath))
            {
                return;
            }

            if (split)
            {
                this.Input = File.ReadAllText(this.FilePath).Split(
                new[] { "\r\n", "\r", "\n" },
                options);

                return;
            }

            this.Input = new string[] { File.ReadAllText(this.FilePath) };
        }
    }
}
