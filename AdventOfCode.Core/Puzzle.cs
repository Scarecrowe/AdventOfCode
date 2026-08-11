namespace AdventOfCode.Core
{
    using System.Reflection;
    using System.Text.RegularExpressions;
    using AdventOfCode.Core.Extensions;

    public abstract class Puzzle
    {
        protected Puzzle(
         int year,
         int day,
         string title)
        {
            this.Year = year;
            this.Day = day;
            this.DayTitle = title;
            this.FilePath = string.Empty;
            this.FilePath = GetInputPath(day, title);
            this.Input = this.GetPuzzleData(this.FilePath);
        }

        protected Puzzle(
            int year,
            int day,
            string title,
            string[] input)
            : this(year, day, title)
        {
            this.Input = input;
        }

        protected Puzzle(
            int year,
            int day,
            string title,
            StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries,
            bool splitOnCarriageReturn = true)
            : this(year, day, title)
        {
            this.FilePath = GetInputPath(day, title);
            this.Input = this.GetPuzzleData(this.FilePath, splitOptions, splitOnCarriageReturn);
        }

        public string FilePath { get; protected set; }

        public string[] Input { get; protected set; }

        public string DayTitle { get; protected set; }

        public int Year { get; protected set; }

        public int Day { get; protected set; }

        public static IPuzzle? GetPuzzle(int year, int day, string input, bool split = true, bool trim = true, StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries)
        {
            if (split)
            {
                return GetPuzzle(year, day, input.Split("\r\n", splitOptions).Select(x => trim ? x.Trim() : x).ToList());
            }

            return GetPuzzle(year, day, new List<string> { trim ? input.Trim() : input });
        }

        public static string GetTitle(int year, int day)
        {
            Type? puzzle = Assembly
                .Load($"AdventOfCode.Puzzles.{year}")
                .GetType($"AdventOfCode.Puzzles._{year}.Days.Day{day}");

            if (puzzle == null)
            {
                throw new ArgumentException();
            }

            IPuzzle? instance = Activator.CreateInstance(puzzle) as IPuzzle;

            if (instance == null)
            {
                throw new InvalidOperationException();
            }

            return instance?.DayTitle ?? string.Empty;
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
                throw new ArgumentException($"Year: {year}, Day: {day}");
            }

            if (input != null)
            {
                return Activator.CreateInstance(puzzle) as IPuzzle;
            }

            return Activator.CreateInstance(puzzle) as IPuzzle;
        }

        public static T? GetAnimation<T>(int year, int day)
            where T : class
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

            return Activator.CreateInstance(puzzle) as T;
        }

        public static List<Type> GetRenderers<T>(int year)
        {
            Assembly assembly = Puzzle.GetPuzzleAssembly(year);

            return assembly
                .GetTypes()
                .Where(t =>
                    typeof(T).IsAssignableFrom(t) &&
                    t.IsClass &&
                    !t.IsAbstract)
                .ToList();
        }

        public static int GetDayFromType(Type type)
            => int.Parse(type.Name.Replace("Day", string.Empty) ?? string.Empty);

        public static int GetYearFromType(Type type)
            => int.Parse(type.Namespace?.Replace("AdventOfCode.Puzzles._", string.Empty).Replace(".Days", string.Empty) ?? string.Empty);

        public static Assembly GetPuzzleAssembly(int year)
        {
            return Assembly.LoadFrom($"AdventOfCode.Puzzles.{year}.dll");
        }

        public static List<string> GetPuzzleTitles(int year)
        {
            List<string> result = new();

            IOrderedEnumerable<string?> namespaces = GetPuzzleAssembly(year)
                .GetTypes()
                .Select(t => t.Namespace)
                .Where(ns => !string.IsNullOrEmpty(ns))
                .Distinct()
                .OrderBy(@namespace => @namespace);

            foreach (string? @namespace in namespaces)
            {
                string current = @namespace?.Replace($"AdventOfCode.Puzzles._{year}.", string.Empty) ?? string.Empty;
                int day = GetDayFromNamespace(@namespace ?? string.Empty);

                if (day == -1)
                {
                    continue;
                }

                for (int i = 1; i <= 25; i++)
                {
                    current = current.Replace($"Day_{i:D2}", string.Empty);
                }

                current = current.Replace("_", " ");

                result.Add(current.Trim());
            }

            return result;
        }

        public static int GetDayFromNamespace(string @namespace)
        {
            Match? match = Regex.Match(@namespace, @"Day_(\d{2})");

            if (!match.Success)
            {
                return -1;
            }

            return int.Parse(match.Groups[1].Value);
        }

        public static string[] GetInput(int day, string title, StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries)
        {
            return File.ReadAllText(GetInputPath(day, title)).Split(
                new[] { "\r\n", "\r", "\n" },
                splitOptions);
        }

        protected static string GetInputPath(int day, string title)
        {
            string dayPath = $"{Assembly.GetCallingAssembly().ExecutingDirectory()}\\Day {(day < 10 ? $"0{day}" : $"{day}")} - {title}";

            return $"{dayPath}\\input.txt";
        }

        protected string[] GetPuzzleData(string path, StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries, bool splitOnCarriageReturn = true)
        {
            if (!File.Exists(path))
            {
                return [];
            }

            if (splitOnCarriageReturn)
            {
                return File.ReadAllText(path).Split(
                    new[] { "\r\n", "\r", "\n" },
                    splitOptions);
            }

            return new string[] { File.ReadAllText(path) };
        }
    }
}
