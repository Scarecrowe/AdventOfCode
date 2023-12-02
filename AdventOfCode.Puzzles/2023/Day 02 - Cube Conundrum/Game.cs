namespace AdventOfCode.Puzzles._2023.Day_02___Cube_Conundrum
{
    using AdventOfCode.Core.Extensions;

    public class Game
    {
        public Game(string line)
        {
            this.Rounds = new();

            string[] tokens = line.Split(":");

            this.Id = int.Parse(tokens[0].Replace("Game "));

            tokens = tokens[1].Split(";");

            foreach (string cube in tokens)
            {
                string[] colours = cube.Trim().Split(",");

                Round round = new();

                foreach (string colour in colours)
                {
                    string[] values = colour.Trim().Split(" ");

                    switch (values[1])
                    {
                        case "red":
                            round.Red = int.Parse(values[0]);
                            break;
                        case "green":
                            round.Green = int.Parse(values[0]);
                            break;
                        case "blue":
                            round.Blue = int.Parse(values[0]);
                            break;
                        default:
                            break;
                    }
                }

                this.Rounds.Add(round);
            }
        }

        public int Id { get; }

        public List<Round> Rounds { get; }
    }
}
