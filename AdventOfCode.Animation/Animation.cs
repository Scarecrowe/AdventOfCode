namespace AdventOfCode.Animation
{
    using System.Drawing;
    using System.Reflection;
    using AdventOfCode.Core.Extensions;

    public class Animation
    {
        public static string[] GetInput(int year, int day, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            return File.ReadAllText(GetInputPath(year, day)).Split(
                new[] { "\r\n", "\r", "\n" },
                options);
        }

        public static string GetRenderPath()
        {
            string path = $"{Assembly.GetCallingAssembly().ExecutingDirectory()}";
            string result = $"{path}\\Output";

            if(!Directory.Exists(result))
            {
                Directory.CreateDirectory(result);
            }

            return result;
        }

        public static string GetInputPath(int year, int day)
        {
            string yearPath = $"{Assembly.GetCallingAssembly().ExecutingDirectory()}\\{year}";
            string dayPath = day < 10 ? $"0{day}" : $"{day}";
            string path = Directory.GetDirectories(yearPath).ToList().FirstOrDefault(x => Path.GetFileName(x).StartsWith($"Day {dayPath}")) ?? string.Empty;
            return $"{path}\\input.txt";
        }

        public static Image GetImage(string file) => Image.FromFile(file);
    }
}
