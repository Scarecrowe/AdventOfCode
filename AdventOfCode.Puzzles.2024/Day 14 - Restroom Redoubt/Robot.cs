namespace AdventOfCode.Puzzles._2024.Day_14___Restroom_Redoubt
{
    using AdventOfCode.Core;

    public class Robot
    {
        public Vector<int> Point { get; set; }

        public Vector<int> Velocity { get; private set; }

        public Robot(string line)
        {
            string[] points = line.Replace("p=", string.Empty).Replace("v=", string.Empty).Split(' ');

            this.Point = new(points[0].Split(',').Select(x => int.Parse(x)).ToArray());
            this.Velocity = new(points[1].Split(',').Select(x => int.Parse(x)).ToArray());
        }

        public void Move(int width, int height)
        {
            this.Point.X = (this.Point.X + this.Velocity.X) % width;
            this.Point.Y = (this.Point.Y + this.Velocity.Y) % height;

            if (this.Point.X < 0)
            {
                this.Point.X += width;
            }

            if (this.Point.Y < 0)
            {
                this.Point.Y += height;
            }
        }
    }
}
