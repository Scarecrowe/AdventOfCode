namespace AdventOfCode.Core.Extensions
{
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using AdventOfCode.Core.Contracts;

    public static class StringExtensions
    {
        public static string ToMd5(this string value, string format = "X2")
        {
            MD5 md5 = MD5.Create();

            byte[] result = md5.ComputeHash(Encoding.UTF8.GetBytes(value));

            StringBuilder sb = new();

            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString(format));
            }

            return sb.ToString();
        }

        public static string[] SplitSpace(this string value, StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries) => value.Split(" ", stringSplitOptions);

        public static string[] SplitComma(this string value, StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries) => value.Split(",", stringSplitOptions);

        public static string SwapPosition(this string value, int x, int y)
        {
            char[] result = value.ToCharArray();
            char buffer = value[x];

            result[x] = result[y];
            result[y] = buffer;

            return new string(result);
        }

        public static string SwapLetter(this string value, char x, char y) => value.SwapPosition(value.IndexOf(x), value.IndexOf(y));

        public static string RotateLeft(this string value, int count) => string.Concat(value[count..], value.AsSpan(0, count));

        public static string RotateRight(this string value, int count) => RotateLeft(value, value.Length - count);

        public static string RotateAroundChar(this string value, char c)
        {
            int count = value.IndexOf(c);
            count = count >= 4 ? count + 2 : count + 1;

            for (int i = 1; i <= count; i++)
            {
                value = value.RotateRight(1);
            }

            return value;
        }

        public static string Reverse(this string value, int x, int y)
        {
            string str = value.Substring(x, (y - x) + 1);

            return value.Replace(str, string.Join(string.Empty, str.Reverse()));
        }

        public static string Move(this string value, int x, int y)
        {
            char first = value[x];

            value = value.Remove(x, 1);

            return value.Insert(y, first.ToString());
        }

        public static string Remove(this string value, string remove) => value.Replace(remove, string.Empty);

        public static string Strip(this string value, char strip)
        {
            while (value[0] == strip)
            {
                value = value.Remove(0, 1);
            }

            while (value[value.Length - 1] == strip)
            {
                value = value.Remove(value.Length - 1, 1);
            }

            return value;
        }

        public static string In(this string value, string contains)
        {
            StringBuilder result = new();
            HashSet<char> @in = contains.Distinct().ToHashSet();

            foreach (char c in value)
            {
                if (@in.Contains(c))
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        public static string Repeat(this string value, int count)
        {
            StringBuilder result = new();

            for (int i = 0; i < count; i++)
            {
                result.Append(value);
            }

            return result.ToString();
        }

        public static List<int> ToAscii(this string value)
        {
            List<int> result = value.Select(x => int.Parse(x.ToString())).ToList();

            return result;
        }

        public static string[] Tokens(this string value)
        {
            List<string> result = new();
            StringBuilder sb = new();

            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];

                if ((c >= 'a' && c <= 'z')
                   || (c >= 'A' && c <= 'Z')
                   || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                }
                else
                {
                    if (sb.Length == 0)
                    {
                        continue;
                    }

                    result.Add(sb.ToString());
                    sb.Clear();
                }
            }

            if (sb.Length > 0)
            {
                result.Add(sb.ToString());
            }

            return result.ToArray();
        }

        public static long ToLong(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            return long.Parse(value);
        }

        public static ulong ToULong(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            return ulong.Parse(value);
        }

        public static int ToInt(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            return int.Parse(value);
        }

        public static int ToInt(this string value, int fromBase)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            return Convert.ToInt32(value, fromBase);
        }

        public static uint ToUInt(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            return uint.Parse(value);
        }

        public static float ToFloat(this string value) => float.Parse(value);

        public static double ToDouble(this string value) => double.Parse(value);

        public static decimal ToDecimal(this string value) => decimal.Parse(value);

        public static byte ToByte(this string value) => byte.Parse(value);

        public static DateTime ToDateTime(this string value) => DateTime.Parse(value);

        public static string Replace(this string value, string search) => value.Replace(search, string.Empty);

        public static string Numbers(this string value)
        {
            StringBuilder result = new();

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] >= '0' && value[i] <= '9')
                {
                    result.Append(value[i]);
                }
            }

            return result.ToString();
        }

        public static Stack<char> ToStack(this string value)
        {
            Stack<char> stack = new();

            foreach (char c in value.ToCharArray().Reverse())
            {
                stack.Push(c);
            }

            return stack;
        }

        public static bool ContainsSpecialCharacters(this string value)
        {
            value.Should().Not().BeNullOrEmpty(paramName: nameof(value));

            Regex expression = new("^[a-zA-Z0-9 ]*$");

            return !expression.IsMatch(value);
        }
    }
}
