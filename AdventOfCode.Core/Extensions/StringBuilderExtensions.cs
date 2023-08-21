namespace AdventOfCode.Core.Extensions
{
    using System.Text;

    public static class StringBuilderExtensions
    {
        public static StringBuilder Replace(this StringBuilder sb, string search) => sb.Replace(search, string.Empty);
    }
}
