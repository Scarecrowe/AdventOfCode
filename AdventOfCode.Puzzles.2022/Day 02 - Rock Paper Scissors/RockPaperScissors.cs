namespace AdventOfCode.Puzzles._2022.Day_02___Rock_Paper_Scissors
{
    public class RockPaperScissors<T>
    {
        public RockPaperScissors(string[] input) => this.Rounds = RockPaperScissors<T>.Parse(input);

        public List<IRound> Rounds { get; }

        public long Play() => this.Rounds.Sum(x => x.Score());

        private static List<IRound> Parse(string[] input) => typeof(T) == typeof(PlayerBShape)
            ? RockPaperScissors<T>.ParsePlayerB(input)
            : RockPaperScissors<T>.ParseRoundResult(input);

        private static List<IRound> ParsePlayerB(string[] input) => input.Select(x => new RoundWithShape(x)).ToList<IRound>();

        private static List<IRound> ParseRoundResult(string[] input) => input.Select(x => new RoundWithResult(x)).ToList<IRound>();
    }
}
