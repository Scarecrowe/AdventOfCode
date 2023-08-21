namespace AdventOfCode.Puzzles._2021.Day_03___Binary_Diagnostic
{
    using AdventOfCode.Core.Extensions;

    public class BinaryDiagnostic
    {
        public static int PowerConsumption(string[] input)
        {
            string gamma = string.Empty;
            string epilson = string.Empty;

            for (int i = 0; i < input[0].Length; i++)
            {
                int one = 0;

                foreach (string line in input)
                {
                    if (line[i] == '1')
                    {
                        one++;
                    }
                }

                int zero = input.Length - one;

                gamma += zero > one ? "0" : "1";
                epilson += zero > one ? "1" : "0";
            }

            return gamma.ToInt(2) * epilson.ToInt(2);
        }

        public static int LifeSupport(string[] input)
        {
            int counter = 0;

            List<string> remaining = new();

            foreach (string line in input)
            {
                remaining.Add(line);
            }

            while (remaining.Count > 1)
            {
                remaining = Find(remaining, counter, 0);

                counter++;
            }

            string oxygen = remaining[0];

            remaining = new();
            counter = 0;

            foreach (string line in input)
            {
                remaining.Add(line);
            }

            while (remaining.Count > 1)
            {
                remaining = Find(remaining, counter, 1);

                counter++;
            }

            string co2 = remaining[0];

            return oxygen.ToInt(2) * co2.ToInt(2);
        }

        private static List<string> Find(List<string> remaining, int counter, int mode)
        {
            List<string> result = new();
            int zero = 0;
            int one = 0;

            foreach (string line in remaining)
            {
                if (line[counter] == '1')
                {
                    one++;

                    continue;
                }

                zero++;
            }

            bool isOneMost = one > zero;
            bool isEqual = one == zero;

            if (mode == 0)
            {
                if (isEqual)
                {
                    isOneMost = true;
                }

                foreach (string line in remaining)
                {
                    if (line[counter] == (isOneMost ? '1' : '0'))
                    {
                        result.Add(line);
                    }
                }
            }
            else
            {
                if (isEqual)
                {
                    isOneMost = true;
                }

                foreach (string line in remaining)
                {
                    if (line[counter] == (isOneMost ? '0' : '1'))
                    {
                        result.Add(line);
                    }
                }
            }

            return result;
        }
    }
}
