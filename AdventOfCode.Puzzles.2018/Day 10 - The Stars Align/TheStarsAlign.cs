namespace AdventOfCode.Puzzles._2018.Day_10___The_Stars_Align
{
    using System.Text;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;

    public class TheStarsAlign
    {
        public TheStarsAlign(string[] input) => this.Map = Parse(input);

        public TheStarsAlign(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public List<StarLight> Map { get; private set; }

        public IFrameRenderer? Renderer { get; }

        private readonly record struct SkyPoint(Vector<int> Location, Vector<int> Velocity);

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

        public TheStarsAlign RenderSilver(int secondsBefore = 80, int secondsAfter = 24, int renderEvery = 1)
            => this.RenderAlignment(secondsBefore, secondsAfter, renderEvery, "THE STARS ALIGN");

        public TheStarsAlign RenderGold(int secondsBefore = 120, int secondsAfter = 40, int renderEvery = 1)
            => this.RenderAlignment(secondsBefore, secondsAfter, renderEvery, "MESSAGE APPEARS");

       
        private List<SkyPoint> ToSkyPoints()
            => this.Map.Select(light => new SkyPoint(light.Location, light.Velocity)).ToList();

        private static int FindMessageSecond(List<SkyPoint> initial)
        {
            int seconds = 0;

            while (true)
            {
                List<Vector<int>> points = PositionsAt(initial, seconds);
                long height = points.Max(p => p.Y) - points.Min(p => p.Y);

                if (height <= 9)
                {
                    return seconds;
                }

                seconds++;
            }
        }

        private TheStarsAlign RenderAlignment(
            int secondsBefore,
            int secondsAfter,
            int renderEvery,
            string title)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<SkyPoint> initial = this.ToSkyPoints();
            int messageSecond = FindMessageSecond(initial);
            int firstSecond = Math.Max(0, messageSecond - secondsBefore);
            int lastSecond = messageSecond + secondsAfter;

            List<Vector<int>> messagePoints = PositionsAt(initial, messageSecond);

            int minX = messagePoints.Min(p => p.X) - 20;
            int maxX = messagePoints.Max(p => p.X) + 20;
            int minY = messagePoints.Min(p => p.Y) - 8;
            int maxY = messagePoints.Max(p => p.Y) + 8;

            for (int second = firstSecond; second <= lastSecond; second++)
            {
                if ((second - firstSecond) % renderEvery != 0 && second != messageSecond && second != lastSecond)
                {
                    continue;
                }

                this.Renderer.RenderFrame(
                    new Frame(this.BuildFrame(
                        initial,
                        second,
                        messageSecond,
                        title,
                        minX,
                        maxX,
                        minY,
                        maxY)));
            }

            return this;
        }

        private string[] BuildFrame(
            List<SkyPoint> initial,
            int second,
            int messageSecond,
            string title,
            int minX,
            int maxX,
            int minY,
            int maxY)
        {
            int width = maxX - minX + 1;
            int height = maxY - minY + 1;
            int delta = second - messageSecond;

            char[][] sky = new char[height][];

            for (int y = 0; y < height; y++)
            {
                sky[y] = new string(' ', width).ToCharArray();
            }

            foreach (SkyPoint point in initial)
            {
                int x = point.Location.X + (point.Velocity.X * second);
                int y = point.Location.Y + (point.Velocity.Y * second);

                if (x < minX || x > maxX || y < minY || y > maxY)
                {
                    continue;
                }

                sky[y - minY][x - minX] = '#';
            }

            string[] result = new string[height + 3];

            result[0] = $"{title} // SECOND {second:00000} // T{delta:+000;-000;000}".PadRight(width);
            result[1] = $"SKY WINDOW {width:000} x {height:000} // LIGHTS {initial.Count:0000}".PadRight(width);
            result[2] = new string(' ', width);

            for (int y = 0; y < height; y++)
            {
                result[y + 3] = new string(sky[y]);
            }

            return result;
        }

        private static List<Vector<int>> PositionsAt(List<SkyPoint> initial, int second)
        {
            List<Vector<int>> result = [];

            foreach (SkyPoint point in initial)
            {
                result.Add(new Vector<int>(
                    point.Location.X + (point.Velocity.X * second),
                    point.Location.Y + (point.Velocity.Y * second)));
            }

            return result;
        }

        private void RenderPaddedFrames(List<string[]> frames)
        {
            int width = frames.SelectMany(frame => frame).Max(row => row.Length);
            int height = frames.Max(frame => frame.Length);

            foreach (string[] frame in frames)
            {
                this.Renderer?.RenderFrame(
                    new Frame(PadFrame(frame, width, height)));
            }
        }

        private static string[] PadFrame(string[] frame, int width, int height)
        {
            List<string> result = [];

            foreach (string row in frame)
            {
                result.Add(row.PadRight(width, ' '));
            }

            while (result.Count < height)
            {
                result.Add(new string(' ', width));
            }

            return [.. result];
        }

        private static List<StarLight> Parse(string[] input) => input.Select(x => new StarLight(x)).ToList();
    }
}
