namespace AdventOfCode.Puzzles._2020.Days
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2020.Day_16___Ticket_Translation;

    public class Day16 : Puzzle, IPuzzle
    {
        public Day16()
        {
            this.DayTitle = "Ticket Translation";
            this.GetPuzzleData(16, this.DayTitle, StringSplitOptions.None);
        }

        public Day16(string[] input) => this.Input = input;

        public string Silver() => $"{TicketTranslation.ErrorRate(this.Input)}";

        public string Gold() => $"{TicketTranslation.Departures(this.Input)}";
    }
}
