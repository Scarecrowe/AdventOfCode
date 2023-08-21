namespace AdventOfCode.Core.Extensions
{
    using System.Reflection;

    public static class AssemblyExtensions
    {
        public static string ExecutingDirectory(this Assembly assembly) => CodeBaseToPath(Assembly.GetExecutingAssembly().Location);

        public static string CodeBaseToPath(string codeBase) => Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(codeBase).Path)) ?? string.Empty;
    }
}
