namespace AdventOfCode.Puzzles._2023.Day_02___Cube_Conundrum
{
    public class CubeConundrum
    {
        public CubeConundrum(string[] input)
        {
            this.Games = new();

            foreach (string line in input)
            {
                this.Games.Add(new Game(line));
            }
        }

        public List<Game> Games { get; }

        public int SumOfIds(int red, int green, int blue)
        {
            List<int> possible = new();

            foreach (Game game in this.Games)
            {
                bool isPossible = true;

                foreach (Round round in game.Rounds)
                {
                    if (round.Red > red
                        || round.Blue > blue
                        || round.Green > green)
                    {
                        isPossible = false;
                        break;
                    }
                }

                if (!isPossible)
                {
                    continue;
                }

                possible.Add(game.Id);
            }

            return possible.Sum();
        }

        public int SumOfPower()
        {
            List<int> possible = new();

            foreach (Game game in this.Games)
            {
                int red = 0;
                int green = 0;
                int blue = 0;

                foreach (Round round in game.Rounds)
                {
                    if (round.Red > red)
                    {
                        red = round.Red;
                    }

                    if (round.Green > green)
                    {
                        green = round.Green;
                    }

                    if (round.Blue > blue)
                    {
                        blue = round.Blue;
                    }
                }

                possible.Add(red * green * blue);
            }

            return possible.Sum();
        }
    }
}
