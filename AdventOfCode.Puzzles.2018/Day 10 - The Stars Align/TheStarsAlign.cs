namespace AdventOfCode.Puzzles._2018.Day_10___The_Stars_Align
{
    using System.Text;
    using AdventOfCode.Core;

    public class TheStarsAlign
    {
        public TheStarsAlign(string[] input) => this.Map = Parse(input);

        public List<StarLight> Map { get; private set; }

        public TheStarsAlign Move()
        {
            this.Map.ForEach(light => light.Move());

            return this;
        }

        public string Print()
        {
            StringBuilder result = new();
            result.Append("\r\n");

            long minX = this.Map.Min(x => x.Location.X);
            long maxX = this.Map.Max(x => x.Location.X);
            long minY = this.Map.Min(y => y.Location.Y);
            long maxY = this.Map.Max(y => y.Location.Y);

            for (long y = minY; y <= maxY; y++)
            {
                result.Append(' ');

                for (long x = minX; x <= maxX; x++)
                {
                    StarLight? light = this.Map.FirstOrDefault(c => c.Location == new Vector<int>((int)x, (int)y));

                    result.Append(light == null ? " " : "#");
                }

                if (y < maxY)
                {
                    result.Append("\r\n");
                }
            }

            return result.ToString();
        }

        public string Align(bool print = true)
        {
            bool canLoop = true;
            int seconds = 0;

            while (canLoop)
            {
                this.Move();

                seconds++;

                if (Math.Abs(this.Map.Max(y => y.Location.Y) - this.Map.Min(y => y.Location.Y)) <= 9)
                {
                    canLoop = false;
                }
            }

            return print ? this.Print() : $"{seconds}";
        }

        private static List<StarLight> Parse(string[] input) => input.Select(x => new StarLight(x)).ToList();
    }
}
