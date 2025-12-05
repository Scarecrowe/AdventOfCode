namespace AdventOfCode.Puzzles._2022.Day_02___Rock_Paper_Scissors
{
    using AdventOfCode.Core.Extensions;

    public class RoundWithShape : IRound
    {
        public RoundWithShape(string input)
        {
            string[] tokens = input.SplitSpace();

            this.ValueA = (PlayerAShape)tokens[0][0];
            this.ValueB = (PlayerBShape)tokens[1][0];
        }

        public PlayerAShape ValueA { get; }

        public object ValueB { get; }

        private static int Win { get; } = 6;

        private static int Draw { get; } = 3;

        public int Score()
        {
            int shapeScore = this.ShapeScore();

            switch (this.ValueA)
            {
                case PlayerAShape.Rock:
                    switch (this.ValueB)
                    {
                        case PlayerBShape.Rock:
                            return Draw + shapeScore;
                        case PlayerBShape.Paper:
                            return Win + shapeScore;
                        case PlayerBShape.Scissor:
                            return shapeScore;
                    }

                    break;
                case PlayerAShape.Paper:
                    switch (this.ValueB)
                    {
                        case PlayerBShape.Rock:
                            return shapeScore;
                        case PlayerBShape.Paper:
                            return Draw + shapeScore;
                        case PlayerBShape.Scissor:
                            return Win + shapeScore;
                    }

                    break;
                case PlayerAShape.Scissor:
                    switch (this.ValueB)
                    {
                        case PlayerBShape.Rock:
                            return Win + shapeScore;
                        case PlayerBShape.Paper:
                            return shapeScore;
                        case PlayerBShape.Scissor:
                            return Draw + shapeScore;
                    }

                    break;
            }

            throw new InvalidOperationException();
        }

        private int ShapeScore()
        {
            switch (this.ValueB)
            {
                case PlayerBShape.Rock:
                    return 1;
                case PlayerBShape.Paper:
                    return 2;
                case PlayerBShape.Scissor:
                    return 3;
            }

            throw new InvalidOperationException();
        }
    }
}
