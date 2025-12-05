namespace AdventOfCode.Puzzles._2022.Day_02___Rock_Paper_Scissors
{
    using AdventOfCode.Core.Extensions;

    public class RoundWithResult : IRound
    {
        private const int Win = 6;

        private const int Draw = 3;

        public RoundWithResult(string input)
        {
            string[] tokens = input.SplitSpace();

            this.ValueA = (PlayerAShape)tokens[0][0];
            this.ValueB = (RoundResult)tokens[1][0];
        }

        public PlayerAShape ValueA { get; }

        public object ValueB { get; }

        public int Score()
        {
            switch (this.ValueB)
            {
                case RoundResult.Lose:
                    switch (this.ValueA)
                    {
                        case PlayerAShape.Rock:
                            return 3;
                        case PlayerAShape.Paper:
                            return 1;
                        case PlayerAShape.Scissor:
                            return 2;
                    }

                    break;
                case RoundResult.Draw:
                    switch (this.ValueA)
                    {
                        case PlayerAShape.Rock:
                            return Draw + 1;
                        case PlayerAShape.Paper:
                            return Draw + 2;
                        case PlayerAShape.Scissor:
                            return Draw + 3;
                    }

                    break;
                case RoundResult.Win:
                    switch (this.ValueA)
                    {
                        case PlayerAShape.Rock:
                            return Win + 2;
                        case PlayerAShape.Paper:
                            return Win + 3;
                        case PlayerAShape.Scissor:
                            return Win + 1;
                    }

                    break;
            }

            throw new InvalidOperationException();
        }
    }
}
