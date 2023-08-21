namespace AdventOfCode.Puzzles._2015.Day_10___Elves_Look__Elves_Say
{
    using System.Text;

    public class ElvesLookElvesSay
    {
        public static string Play(string input, int rounds)
        {
            string value = input;
            StringBuilder sb = new();

            for (int round = 1; round <= rounds; round++)
            {
                int count = 1;
                char last = value[0];

                for (int i = 1; i < value.Length; i++)
                {
                    if (last == value[i])
                    {
                        count++;
                        continue;
                    }

                    sb.Append($"{count}{last}");

                    last = value[i];
                    count = 1;
                }

                sb.Append($"{count}{last}");

                value = sb.ToString();
                sb.Clear();
            }

            return value;
        }
    }
}
