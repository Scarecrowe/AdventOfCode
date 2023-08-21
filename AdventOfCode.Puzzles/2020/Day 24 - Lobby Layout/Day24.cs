namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_24___Lobby_Layout;

    public class Day24 : Puzzle, IPuzzle
    {
        public Day24(string file)
        {
            this.DayTitle = "Lobby Layout";
            this.GetPuzzleData(file);
        }

        public Day24(string[] input) => this.Input = input;

        public string Silver() => $"{LobbyLayout.BlackSideUp(this.Input)}";

        public string Gold() => $"{LobbyLayout.BlackCount(this.Input)}";
    }
}
