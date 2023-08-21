namespace AdventOfCode.Puzzles._2022.Day_10___Cathode_Ray_Tube
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class CathodeRayTube
    {
        public CathodeRayTube(string[] input)
        {
            this.Strength = 0;
            this.Display = string.Empty;
            this.Process(input);
        }

        public int Strength { get; private set; }

        public string Display { get; private set; }

        private static int GetScore(int cycle, int x)
        {
            return cycle switch
            {
                20 or 60 or 100 or 140 or 180 or 220 => cycle * x,
                _ => 0,
            };
        }

        private void Process(string[] input)
        {
            VectorArray<int, int> crt = new(40, 6);
            int crtX = 0;
            int crtY = 0;
            int x = 1;
            int cycle = 0;
            int strength = 0;
            int index = 0;
            int value = 0;
            string opCode = string.Empty;

            while (true)
            {
                cycle++;
                strength += GetScore(cycle, x);

                if (crtX == x || crtX - 1 == x || crtX + 1 == x)
                {
                    crt[crtY, crtX] = 1;
                }

                crtX++;

                if (crtX > 39)
                {
                    crtX = 0;
                    crtY++;
                }

                if (!string.IsNullOrEmpty(opCode))
                {
                    x += value;
                    opCode = string.Empty;
                    continue;
                }

                if (string.IsNullOrEmpty(opCode))
                {
                    string[] tokens = input[index].SplitSpace();

                    if (tokens[0] == "addx")
                    {
                        opCode = tokens[0];
                        value = tokens[1].ToInt();
                    }

                    index++;

                    if (index >= input.Length)
                    {
                        break;
                    }
                }
            }

            this.Display = crt.Print((value) => value == 1 ? '#' : ' ');
            this.Strength = strength;
        }
    }
}
