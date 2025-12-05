namespace AdventOfCode.Puzzles._2024.Day_14___Restroom_Redoubt
{
    using AdventOfCode.Core;

    public class RestroomRedoubt
    {
        public HashSet<Robot> Robots { get; private set; }

        private int Width { get; set; }

        private int Height { get; set; }

        private int CenterX { get; set; }

        private int CenterY { get; set; }

        public RestroomRedoubt(string[] input)
        {
            this.Robots = input.Select(x => new Robot(x)).ToHashSet();
            this.Width = 101;
            this.Height = 103;
            this.CenterX = (int)Math.Ceiling((decimal)this.Width / 2) - 1;
            this.CenterY = (int)Math.Ceiling((decimal)this.Height / 2) - 1;
        }

        public int SafetyFactor(int seconds)
        {
            for (int i = 1; i <= seconds; i++)
            {
                this.MoveAll();
            }

            int q1 = 0, q2 = 0, q3 = 0, q4 = 0;

            foreach (Robot robot in this.Robots)
            {
                if (robot.Point.X < this.CenterX && robot.Point.Y < this.CenterY)
                {
                    q1++;
                }
                else if (robot.Point.X > this.CenterX && robot.Point.Y < this.CenterY)
                {
                    q2++;
                }
                else if (robot.Point.X < this.CenterX && robot.Point.Y > this.CenterY)
                {
                    q3++;
                }
                else if (robot.Point.X > this.CenterX && robot.Point.Y > this.CenterY)
                {
                    q4++;
                }
            }

            return q1 * q2 * q3 * q4;
        }

        public int XmasTree()
        {
            int result = 0;

            Vector<int>[] offsets =
            {
                new(-1, 1), new(1, 1),
                new(-2, 2), new(2, 2),
                new(-3, 3), new(3, 3),
                new(-4, 4), new(4, 4)
            };

            while (true)
            {
                this.MoveAll();
                result++;

                HashSet<Vector<int>> positions = this.Robots.Select(x => x.Point).ToHashSet();

                foreach (var robot in this.Robots)
                {
                    bool allOffsetsMatched = true;

                    foreach (Vector<int> offset in offsets)
                    {
                        if (!positions.Contains(robot.Point + offset))
                        {
                            allOffsetsMatched = false;
                            break;
                        }
                    }

                    if (allOffsetsMatched)
                    {
                        return result;
                    }
                }
            }
        }

        private void MoveAll()
        {
            foreach (Robot robot in this.Robots)
            {
                robot.Move(this.Width, this.Height);
            }
        }
    }
}
