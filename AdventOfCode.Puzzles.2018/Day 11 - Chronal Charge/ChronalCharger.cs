namespace AdventOfCode.Puzzles._2018.Day_11___Chronal_Charge
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class ChronalCharger
    {
        public ChronalCharger(string input) => this.SerialNumber = input.ToInt();

        public int SerialNumber { get; }

        public int PowerLevel(int x, int y)
        {
            int rackId = x + 10;
            int powerLevel = rackId * y;
            powerLevel += this.SerialNumber;
            powerLevel *= rackId;
            powerLevel = powerLevel.ToString().Reverse().ToArray()[2].ToString().ToInt();
            powerLevel -= 5;

            return powerLevel;
        }

        public string MaxPowerLevel3x3()
        {
            int size = 297;
            Vector<int> position = new(0, 0);
            int total = 0;

            for (int y = 1; y <= size; y++)
            {
                for (int x = 1; x <= size; x++)
                {
                    int powerLevel = 0;

                    for (int yy = 0; yy < 3; yy++)
                    {
                        for (int xx = 0; xx < 3; xx++)
                        {
                            powerLevel += this.PowerLevel(x + xx, y + yy);
                        }
                    }

                    if (powerLevel > total)
                    {
                        total = powerLevel;
                        position = new(x, y);
                    }
                }
            }

            return $"{position.X},{position.Y}";
        }

        public string MaxPowerLevel()
        {
            int mapSize = 301;
            int[,] powerLevels = new int[mapSize, mapSize];
            int max = int.MinValue;
            Vector<int> largest = new(0, 0, 0);

            for (int x = 1; x < mapSize; x++)
            {
                for (int y = 1; y < mapSize; y++)
                {
                    powerLevels[x, y] = this.PowerLevel(x, y);
                }
            }

            for (int x = 1; x <= mapSize; x++)
            {
                for (int y = 1; y <= mapSize; y++)
                {
                    int powerLevel = 0;

                    for (int size = 1; size < mapSize - Math.Max(x, y); size++)
                    {
                        for (int i = y; i < y + size + 1; i++)
                        {
                            powerLevel += powerLevels[x + size, i];
                        }

                        for (int i = x; i < x + size; i++)
                        {
                            powerLevel += powerLevels[i, y + size];
                        }

                        if (powerLevel > max)
                        {
                            max = powerLevel;
                            largest = new(x, y, size + 1);
                        }
                    }
                }
            }

            return $"{largest.X},{largest.Y},{largest.Z}";
        }
    }
}
