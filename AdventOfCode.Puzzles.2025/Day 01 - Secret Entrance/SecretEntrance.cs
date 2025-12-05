namespace AdventOfCode.Puzzles._2025.Day_01
{
    using AdventOfCode.Core;

    public class SecretEntrance(string[] input)
    {
        private List<(Cardinal Direction, int Distance)> Turns { get; set; } = ParseTurns(input);

        private int Dial { get; set; } = 50;

        private static List<(Cardinal Directon, int Distance)> ParseTurns(string[] input)
        {
            List<(Cardinal Directon, int Distance)> result = new();

            foreach (string line in input)
            {
                result.Add((line[0] == 'L' ? Cardinal.West : Cardinal.East, int.Parse(line[1..])));
            }

            return result;
        }

        private static int Wrap(int value) => ((value % 100) + 100) % 100;

        public int Password()
        {
            int result = 0;

            foreach (var (Direction, Distance) in this.Turns)
            {
                if (Direction == Cardinal.West)
                {
                    this.Dial -= Distance;
                    this.Dial = Wrap(this.Dial);
                }
                else
                {
                    this.Dial += Distance;
                    this.Dial = Wrap(this.Dial);
                }

                if (this.Dial == 0)
                {
                    result++;
                }
            }

            return result;
        }

        public int AdvancedPassword()
        {
            int result = 0;

            foreach (var (Direction, Distance) in this.Turns)
            {
                int direction = Direction == Cardinal.West ? -1 : 1;

                for (int i = 0; i < Distance; i++)
                {
                    this.Dial += direction;

                    if (this.Dial < 0)
                    {
                        this.Dial = 99;
                    }
                    else if (this.Dial > 99)
                    {
                        this.Dial = 0;
                        result++;
                        continue;
                    }

                    if (this.Dial == 0)
                    {
                        result++;
                    }
                }
            }

            return result;
        }
    }
}
