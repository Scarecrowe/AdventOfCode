namespace AdventOfCode.Puzzles._2019.Day_16___Flawed_Frequency_Transmission
{
    using AdventOfCode.Core.Extensions;

    public class FlawedFrequencyTransmission
    {
        private static readonly int[] Modifier = new int[] { 1, 2, 1, 0 };

        public static string Single(string[] input)
        {
            int[] phasedData;
            int[] data = input[0].ToCharArray().Select(x => int.Parse(x.ToString())).ToArray();
            int[,] transform = Transform();

            phasedData = new int[data.Length];

            for (int phase = 0; phase < 100; phase++)
            {
                int d;
                for (d = 0; d < data.Length / 2; d++)
                {
                    int m = 0;
                    for (int i = d; i < data.Length; i++)
                    {
                        if ((i + 1) % (d + 1) == 0)
                        {
                            m = (m == 3) ? 0 : m + 1;
                        }

                        phasedData[d] += transform[data[i], Modifier[m]];
                    }

                    phasedData[d] = (phasedData[d] < 0) ? -phasedData[d] % 10 : phasedData[d] % 10;
                }

                CalculatePhasedData(d, data, phasedData);

                data = phasedData;
                phasedData = new int[phasedData.Length];
            }

            return string.Concat(data)[..8];
        }

        public static string Multiple(string[] input)
        {
            int offset = input[0][..7].ToInt();
            int[] raw = input[0].ToCharArray().Select(c => int.Parse(c.ToString())).ToArray();
            int[,] transform = Transform();
            int[] data = new int[raw.Length * 10_000];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = raw[i % raw.Length];
            }

            int[] phasedData = new int[data.Length];

            for (int phase = 0; phase < 100; phase++)
            {
                int d = 0;

                for (d = offset; d < data.Length / 2; d++)
                {
                    int m = 0;

                    for (int i = d; i < data.Length; i++)
                    {
                        if ((i + 1) % (d + 1) == 0)
                        {
                            m = (m == 3) ? 0 : m + 1;
                        }

                        phasedData[d] += transform[data[i], Modifier[m]];
                    }

                    phasedData[d] = (phasedData[d] < 0) ? -phasedData[d] % 10 : phasedData[d] % 10;
                }

                CalculatePhasedData(d, data, phasedData);

                data = phasedData;
                phasedData = new int[phasedData.Length];
            }

            return data.Join().Substring(offset, 8);
        }

        private static int[,] Transform()
        {
            int[,] result = new int[10, 3];

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    result[i, j] = (i * (j - 1)) % 10;
                }
            }

            return result;
        }

        private static void CalculatePhasedData(int i, int[] data, int[] phasedData)
        {
            int current = 0;

            for (int e = data.Length - 1; e >= i; e--)
            {
                current += data[e];
                phasedData[e] = (current < 0) ? (-current % 10) : current % 10;
            }
        }
    }
}
