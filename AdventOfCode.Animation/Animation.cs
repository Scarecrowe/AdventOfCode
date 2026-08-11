namespace AdventOfCode.Animation
{
    using System.Drawing;
    using System.Reflection;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class Animation
    {
        public static string[] GetInput(int day, string title, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            return File.ReadAllText(GetInputPath(day, title)).Split(
                new[] { "\r\n", "\r", "\n" },
                options);
        }

        public static string GetInputPath(int day, string title)
        {
            string dayPath = $"{Assembly.GetCallingAssembly().ExecutingDirectory()}\\Day {(day < 10 ? $"0{day}" : $"{day}")} - {title}";

            return $"{dayPath}\\input.txt";
        }

        public static string GetRenderPath(int year, int day, string title)
        {
            string path = $"{Assembly.GetCallingAssembly().ExecutingDirectory()}";
            string animationsPath = $"{path}\\Animations";

            if (!Directory.Exists(animationsPath))
            {
                Directory.CreateDirectory(animationsPath);
            }

            string yearPath = $"{animationsPath}\\{year}";

            if (!Directory.Exists(yearPath))
            {
                Directory.CreateDirectory(yearPath);
            }

            string puzzlePath = $"{yearPath}\\Day {day} - {title}";

            if (!Directory.Exists(puzzlePath))
            {
                Directory.CreateDirectory(puzzlePath);
            }

            return puzzlePath;
        }

        public static List<Type> GetByType(AnimationType animationType)
        {
            List<Type> result = new();

            for (int year = 2015; year <= 2025; year++)
            {
                switch (animationType)
                {
                    case AnimationType.TwoDimension:
                        result.AddRange(Puzzle.GetRenderers<I2dAnimation>(year));
                        break;
                    case AnimationType.ThreeDimension:
                        result.AddRange(Puzzle.GetRenderers<I3dAnimation>(year));
                        break;
                    case AnimationType.Ascii:
                        result.AddRange(Puzzle.GetRenderers<IAsciiAnimation>(year));
                        break;
                    case AnimationType.Console:
                        result.AddRange(Puzzle.GetRenderers<IConsoleAnimation>(year));
                        break;
                    default:
                        throw new InvalidOperationException($"No animation type {animationType}");
                }
            }

            return result;
        }

        public static Image GetImage(string file) => Image.FromFile(file);
    }
}
