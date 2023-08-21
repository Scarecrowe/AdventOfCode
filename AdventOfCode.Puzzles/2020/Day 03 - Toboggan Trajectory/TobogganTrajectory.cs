namespace AdventOfCode.Puzzles._2020.Day_03___Toboggan_Trajectory
{
    public class TobogganTrajectory
    {
        public TobogganTrajectory(string[] input) => this.Input = input;

        public string[] Input { get; }

        public int Single() => this.TraverseSlope(3, 1);

        public long Multiple()
        {
            long trees = this.TraverseSlope(1, 1);
            trees *= this.TraverseSlope(3, 1);
            trees *= this.TraverseSlope(5, 1);
            trees *= this.TraverseSlope(7, 1);
            trees *= this.TraverseSlope(1, 2);

            return trees;
        }

        private int TraverseSlope(int right, int down)
        {
            int x = right;
            int trees = 0;

            for (int i = down; i < this.Input.Length; i += down)
            {
                if (this.Input[i][x] == '#')
                {
                    trees++;
                }

                x = x > this.Input[i].Length - (right + 1)
                    ? right - (this.Input[i].Length - x)
                    : x + right;
            }

            if (down == 2)
            {
                if (this.Input[^1][x + 1] == '#')
                {
                    trees++;
                }
            }

            return trees;
        }
    }
}
