namespace AdventOfCode.Animation
{
    using System.Drawing;
    using System.Reflection;
    using AdventOfCode.Core.Extensions;

    public class Animation
    {
        public static string[] GetInput(int day, string title, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            return File.ReadAllText(GetInputPath(day, title)).Split(
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

        public static string GetInputPath(int day, string title)
        {
            string dayPath = $"{Assembly.GetCallingAssembly().ExecutingDirectory()}\\Day {(day < 10 ? $"0{day}" : $"{day}")} - {title}";

            return $"{dayPath}\\input.txt";
        }

        public static Image GetImage(string file) => Image.FromFile(file);
    }
}
