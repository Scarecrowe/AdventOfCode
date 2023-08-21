namespace AdventOfCode.Puzzles._2017.Day_16___Permutation_Promenade
{
    using AdventOfCode.Core.Extensions;

    public class PromenadeMove
    {
        public PromenadeMove(string move)
        {
            this.Type = ParseMoveType(move);

            move = move[1..];
            string[] tokens = move.Split("/");

            this.ValueA = char.IsNumber(tokens[0][0]) ? tokens[0].ToInt() : tokens[0][0];

            if (this.Type == PromenadeMoveType.Spin)
            {
                return;
            }

            this.ValueB = char.IsNumber(tokens[1][0]) ? tokens[1].ToInt() : tokens[1][0];
        }

        public PromenadeMoveType Type { get; }

        public int ValueA { get; }

        public int ValueB { get; }

        public string Run(string value)
            => this.Type switch
            {
                PromenadeMoveType.Spin => value.RotateRight(this.ValueA),
                PromenadeMoveType.Exchange => value.SwapPosition(this.ValueA, this.ValueB),
                PromenadeMoveType.Partner => value.SwapLetter((char)this.ValueA, (char)this.ValueB),
                _ => throw new InvalidOperationException(),
            };

        private static PromenadeMoveType ParseMoveType(string move)
            => move[0] switch
            {
                's' => PromenadeMoveType.Spin,
                'x' => PromenadeMoveType.Exchange,
                'p' => PromenadeMoveType.Partner,
                _ => throw new InvalidOperationException(),
            };
    }
}
