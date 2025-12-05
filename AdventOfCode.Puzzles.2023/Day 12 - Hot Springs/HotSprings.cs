namespace AdventOfCode.Puzzles._2023.Day_12___Hot_Springs
{
    using AdventOfCode.Core.Extensions;

    public class HotSprings
    {
        public HotSprings(string[] input)
        {
            long result = 0;

            foreach(string line in input)
            {
                string[] tokens = line.Split(" ");
                List<string> possible = new();

                int[] damaged = tokens[1].Split(",").ToInt();
                int count = 0;

                this.Make(tokens[0], 0, possible);

                foreach(string pos in possible)
                {
                    var t = pos.Split(".", StringSplitOptions.RemoveEmptyEntries);
                    bool found = true;

                    if (t.Length != damaged.Length)
                    {
                        continue;
                    }

                    for(int i = 0; i < t.Length; i++)
                    {
                        if (t[i].Length != damaged[i])
                        {
                            found = false;
                            break;
                        }
                    }

                    if (found)
                    {
                        count++;
                    }
                }

                result += count;
            }
        }

        public void Make(string value, int i, List<string> values)
        {
            for (; i < value.Length; i++)
            {
                if (value[i] == '?')
                {
                    char[] ch = value.ToCharArray();

                    ch[i] = '#';
                    this.Make(new string(ch), i + 1, values);

                    ch[i] = '.';
                    this.Make(new string(ch), i + 1, values);
                    break;
                }
            }

            if (!value.Contains("?"))
            {
                values.Add(value);
            }
        }
    }
}
