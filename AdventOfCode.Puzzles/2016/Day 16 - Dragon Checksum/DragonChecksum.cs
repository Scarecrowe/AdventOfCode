namespace AdventOfCode.Puzzles._2016.Day_16___Dragon_Checksum
{
    using System.Text;

    public class DragonChecksum
    {
        public DragonChecksum(string input) => this.Input = input;

        public string Input { get; }

        public static string DragonCurve(string a)
        {
            StringBuilder b = new();

            string reversed = new(a.Reverse().ToArray());

            for (int i = 0; i < a.Length; i++)
            {
                b.Append(reversed[i] == '1' ? '0' : '1');
            }

            return $"{a}0{b}";
        }

        public static string Checksum(string value)
        {
            string result = value;

            do
            {
                result = MatchPairs(result);
            }
            while (result.Length % 2 == 0);

            return result;
        }

        public string Fill(int size)
        {
            string data = this.Input;

            while (data.Length < size)
            {
                data = DragonCurve(data);
            }

            return Checksum(data[..size]);
        }

        private static string MatchPairs(string value)
        {
            StringBuilder result = new();

            for (int i = 0; i < value.Length; i += 2)
            {
                result.Append(value[i] == value[i + 1] ? 1 : 0);
            }

            return result.ToString();
        }
    }
}