namespace AdventOfCode.Puzzles._2016.Day_05___How_About_a_Nice_Game_of_Chess
{
    using System.Text;
    using AdventOfCode.Core.Extensions;

    public class HowAboutANiceGameOfChess
    {
        public static string Simple(string doorId)
        {
            StringBuilder password = new();

            int index = 0;

            while (true)
            {
                string hash = $"{doorId}{index}".ToMd5();

                if (hash.StartsWith("00000"))
                {
                    password.Append(hash[5]);

                    if (password.Length == 8)
                    {
                        break;
                    }
                }

                index++;
            }

            return password.ToString().ToLower();
        }

        public static string Advanced(string doorId)
        {
            char[] password = new char[8];

            int index = 0;
            int chars = 0;

            while (true)
            {
                string hash = $"{doorId}{index}".ToMd5();

                if (hash.StartsWith("00000"))
                {
                    if (char.IsNumber(hash[5]))
                    {
                        int charIndex = int.Parse($"{hash[5]}");

                        if (charIndex <= 7 && password[charIndex] == '\0')
                        {
                            password[charIndex] = hash[6];

                            chars++;

                            if (chars == 8)
                            {
                                break;
                            }
                        }
                    }
                }

                index++;
            }

            return string.Join(string.Empty, password).ToLower();
        }
    }
}
