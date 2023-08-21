namespace AdventOfCode.Puzzles._2020.Day_12___Rain_Risk
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class RainRisk
    {
        public RainRisk(string[] input)
        {
            this.Input = input;
            this.Direction = 1;
            this.Point = new(0, 0);
            this.WayPoint = new(0, 0);
        }

        private int[] Directions { get; } = new int[] { 'N', 'E', 'S', 'W' };

        private Vector<int> Point { get; set; }

        private Vector<int> WayPoint { get; set; }

        private uint Direction { get; set; }

        private string[] Input { get; }

        public long Distance()
        {
            this.Point = new(0, 0);
            this.Direction = 1;
            this.Input.ForEach(this.Process);

            return this.Point.Absolute();
        }

        public long DistanceWithWaypoint()
        {
            this.Point = new(0, 0);
            this.WayPoint = new(10, 1);
            this.Direction = 1;
            this.Input.ForEach(this.ProcessWithWaypoint);

            return this.Point.Absolute();
        }

        private void Process(string instruction)
        {
            char mode = instruction[0];
            int value = instruction[1..].ToInt();

            switch (mode)
            {
                case 'N':
                    this.Point.Y += value;
                    break;
                case 'S':
                    this.Point.Y -= value;
                    break;
                case 'E':
                    this.Point.X += value;
                    break;
                case 'W':
                    this.Point.X -= value;
                    break;
                case 'L':
                    this.Direction = (this.Direction - ((uint)value / 90)) % 4;
                    break;
                case 'R':
                    this.Direction = (this.Direction + ((uint)value / 90)) % 4;
                    break;
                case 'F':
                    this.Process($"{(char)this.Directions[this.Direction]}{value}");
                    break;
            }
        }

        private void ProcessWithWaypoint(string instruction)
        {
            char mode = instruction[0];
            int value = instruction[1..].ToInt();

            switch (mode)
            {
                case 'N':
                    this.WayPoint.Y += value;
                    break;
                case 'S':
                    this.WayPoint.Y -= value;
                    break;
                case 'E':
                    this.WayPoint.X += value;
                    break;
                case 'W':
                    this.WayPoint.X -= value;
                    break;
                case 'L':
                    this.WayPoint = this.WayPoint.Rotate(value);
                    break;
                case 'R':
                    this.WayPoint = this.WayPoint.Rotate(-value);
                    break;
                case 'F':
                    this.Point += this.WayPoint * value;
                    break;
            }
        }
    }
}
