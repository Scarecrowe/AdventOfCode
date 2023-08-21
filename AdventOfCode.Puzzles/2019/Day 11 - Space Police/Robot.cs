namespace AdventOfCode.Puzzles._2019.Day_11___Space_Police
{
    using AdventOfCode.Core;
    using AdventOfCode.Puzzles._2019.IntCode;

    public class Robot
    {
        public Robot(string program)
        {
            this.Point = new(0, 0);
            this.Direction = Cardinal.North;
            this.Cpu = new(program);
        }

        public Vector<int> Point { get; private set; }

        public Cardinal Direction { get; private set; }

        public IntcodeCpu Cpu { get; }

        public Robot Turn(RobotTurn turn)
        {
            this.Direction = turn == RobotTurn.Left
                ? CardinalHelper.AntiClockwise[this.Direction]
                : CardinalHelper.Clockwise[this.Direction];
            this.Point += CardinalHelper.CardinalTransform<int>()[this.Direction];

            return this;
        }

        public Robot SetPosition(Vector<int> point)
        {
            this.Point = new(point);

            return this;
        }
    }
}
