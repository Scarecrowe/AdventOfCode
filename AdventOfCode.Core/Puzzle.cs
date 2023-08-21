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
                .Load("AdventOfCode.Puzzles")
                .GetType($"AdventOfCode.Puzzles._{year}.Days.Day{day}");

            if ((year < 2015 || year > 2022)
                || (day < 1 || day > 25)
                || puzzle == null)
            {
                throw new ArgumentException();
            }

            if (input != null)
            {
                return Activator.CreateInstance(puzzle, new List<string[]> { input.ToArray() }.ToArray()) as IPuzzle;
            }

            return Activator.CreateInstance(puzzle, GetInputPath(year, day)) as IPuzzle;
        }

        public static string[] GetInput(int year, int day, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            return File.ReadAllText(GetInputPath(year, day)).Split(
                new[] { "\r\n", "\r", "\n" },
                options);
        }

        protected static string GetInputPath(int year, int day)
        {
            string yearPath = $"{Assembly.GetCallingAssembly().ExecutingDirectory()}\\{year}";
            string dayPath = day < 10 ? $"0{day}" : $"{day}";
            string path = Directory.GetDirectories(yearPath).ToList().FirstOrDefault(x => Path.GetFileName(x).StartsWith($"Day {dayPath}")) ?? string.Empty;
            return $"{path}\\input.txt";
        }

        protected void GetPuzzleData(string file, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries, bool split = true)
        {
            file.Should().Not().BeNullOrEmpty(file, paramName: nameof(file));

            this.FilePath = file;

            if (split)
            {
                this.Input = File.ReadAllText(file).Split(
                new[] { "\r\n", "\r", "\n" },
                options);

                return;
            }

            this.Input = new string[] { File.ReadAllText(file) };
        }
    }
}
