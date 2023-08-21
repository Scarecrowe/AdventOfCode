namespace AdventOfCode.Puzzles._2022.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2022.Day_16___Proboscidea_Volcanium;

    public class Day16 : Puzzle, IPuzzle
    {
        public Day16(string file)
        {
            this.DayTitle = "Proboscidea Volcanium";
            this.GetPuzzleData(file);

            // this.Input = new string[10];
            // this.Input[0] = "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB";
            // this.Input[1] = "Valve BB has flow rate=13; tunnels lead to valves CC, AA";
            // this.Input[2] = "Valve CC has flow rate=2; tunnels lead to valves DD, BB";
            // this.Input[3] = "Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE";
            // this.Input[4] = "Valve EE has flow rate=3; tunnels lead to valves FF, DD";
            // this.Input[5] = "Valve FF has flow rate=0; tunnels lead to valves EE, GG";
            // this.Input[6] = "Valve GG has flow rate=0; tunnels lead to valves FF, HH";
            // this.Input[7] = "Valve HH has flow rate=22; tunnel leads to valve GG";
            // this.Input[8] = "Valve II has flow rate=0; tunnels lead to valves AA, JJ";
            // this.Input[9] = "Valve JJ has flow rate=21; tunnel leads to valve II";
        }

        public Day16(string[] input) => this.Input = input;

        public string Silver() => $"{new ProboscideaVolcanium(this.Input).Single()}";

        public string Gold() => $"{new ProboscideaVolcanium(this.Input).Pair()}";
    }
}
