namespace AdventOfCode.Puzzles._2018.Day_13___Mine_Cart_Madness
{
    using AdventOfCode.Core;

    public class MineCart
    {
        public MineCart(Vector<int> point, char direction)
        {
            this.Point = point;
            this.Direction = direction;
        }

        public Vector<int> Point { get; }

        public char Direction { get; private set; }

        public bool Collided { get; private set; }

        public int TurnCount { get; private set; }

        private static Dictionary<(char, char), char> Turns { get; } = new()
        {
            { ('/', '<'), 'v' },
            { ('/', '>'), '^' },
            { ('/', '^'), '>' },
            { ('/', 'v'), '<' },
            { ('\\', '<'), '^' },
            { ('\\', '>'), 'v' },
            { ('\\', '^'), '<' },
            { ('\\', 'v'), '>' },
            { ('<', 'l'), 'v' },
            { ('>', 'l'), '^' },
            { ('^', 'l'), '<' },
            { ('v', 'l'), '>' },
            { ('<', 'r'), '^' },
            { ('>', 'r'), 'v' },
            { ('^', 'r'), '>' },
            { ('v', 'r'), '<' }
        };

        public Vector<int> PeekMove()
        {
            switch (this.Direction)
            {
                case '<':
                    return new(this.Point.X - 1, this.Point.Y);
                case '>':
                    return new(this.Point.X + 1, this.Point.Y);
                case '^':
                    return new(this.Point.X, this.Point.Y - 1);
                case 'v':
                    return new(this.Point.X, this.Point.Y + 1);
                default:
                    break;
            }

            throw new InvalidOperationException();
        }

        public void Move()
        {
            if (this.Direction == '<')
            {
                this.Point.X--;
            }
            else if (this.Direction == '>')
            {
                this.Point.X++;
            }
            else if (this.Direction == '^')
            {
                this.Point.Y--;
            }
            else
            {
                this.Point.Y++;
            }
        }

        public void IntersectionTurn()
        {
            if (this.TurnCount == 0)
            {
                this.TurnLeft();
            }
            else if (this.TurnCount == 2)
            {
                this.TurnRight();
            }

            this.TurnCount = (this.TurnCount + 1) % 3;
        }

        public void CornerTurn(char value) => this.Direction = Turns[(value, this.Direction)];

        public void MarkCollided() => this.Collided = true;

        private void TurnLeft() => this.Direction = Turns[(this.Direction, 'l')];

        private void TurnRight() => this.Direction = Turns[(this.Direction, 'r')];
    }
}
