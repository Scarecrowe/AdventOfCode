namespace AdventOfCode.Puzzles._2016.Day_07___Internet_Protocol_Version_7
{
    using System.Text;
    using AdventOfCode.Core.Extensions;

    public class InternetAddress
    {
        public InternetAddress(string input)
        {
            this.Hypernet = new();
            this.Supernet = new();

            StringBuilder sb = new();

            for (int i = 0; i < input.Length; i++)
            {
                char character = input[i];

                switch (character)
                {
                    case '[':
                        this.Supernet.Add(sb.ToString());
                        sb.Clear();
                        break;
                    case ']':
                        this.Hypernet.Add(sb.ToString());
                        sb.Clear();
                        break;
                    default:
                        sb.Append(character);
                        break;
                }
            }

            this.Supernet.Add(sb.ToString());
        }

        public List<string> Hypernet { get; }

        public List<string> Supernet { get; }

        public bool SupportsTls()
        {
            foreach (string hypernet in this.Hypernet)
            {
                if (GetReverse(hypernet, 4).Count > 0)
                {
                    return false;
                }
            }

            foreach (string supernet in this.Supernet)
            {
                if (GetReverse(supernet, 4).Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool SupportsSsl()
        {
            List<string> blocks = new();

            foreach (string hypernet in this.Hypernet)
            {
                List<string> block = GetReverse(hypernet, 3);

                if (block.Count == 0)
                {
                    continue;
                }

                foreach (string value in block)
                {
                    blocks.Add($"{value[1]}{value[0]}{value[1]}");
                }
            }

            if (blocks.Count == 0)
            {
                return false;
            }

            foreach (string supernet in this.Supernet)
            {
                List<string> accessor = GetReverse(supernet, 3);

                if (accessor.Count == 0)
                {
                    continue;
                }

                foreach (string value in accessor)
                {
                    if (blocks.Contains(value))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static List<string> GetReverse(string value, int count)
        {
            List<string> result = new();

            for (int i = 0; i < value.Length; i++)
            {
                if (i + count > value.Length)
                {
                    break;
                }

                string current = value.Substring(i, count);

                if (string.Equals(current, current.Reverse().Join()) && current[1] != current[0])
                {
                    result.Add(current);
                }
            }

            return result;
        }
    }
}
