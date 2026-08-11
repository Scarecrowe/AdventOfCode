namespace AdventOfCode.Puzzles._2017.Day_19___A_Series_of_Tubes
{
    using System.Text;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class ASeriesOfTubes
    {
        public ASeriesOfTubes(string[] input)
        {
            this.Start = new(0, 0);
            this.Diagram = this.ParseDiagram(input);
        }

        public ASeriesOfTubes(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public VectorDictionary<int, char> Diagram { get; }

        public Vector<int> Start { get; private set; }

        public IFrameRenderer? Renderer { get; }

        private readonly record struct TubeStep(
            Vector<int> Point,
            Cardinal Direction,
            char Value,
            string Letters,
            int Steps);

        public string Move(bool returnSteps = false)
        {
            StringBuilder result = new();
            HashSet<Vector<int>> visited = new();
            Vector<int> point = new(this.Start);
            Cardinal direction = Cardinal.South;
            int steps = 0;

            while (true)
            {
                steps++;
                var cardinal = this.Diagram.AdjacentCardinal(point);
                char value = this.Diagram[point];
                visited.Add(new(point));

                if (value == '+')
                {
                    direction = cardinal.First(c => c.Point != point && c.Value != ' ' && !visited.Contains(c.Point)).Direction;
                }

                if (char.IsLetter(value))
                {
                    result.Append(value);
                }

                if (cardinal.First(c => c.Direction == direction).Value == ' ')
                {
                    return returnSteps ? $"{steps}" : $"{result}";
                }

                point += CardinalHelper.CardinalTransform<int>()[direction];
            }

            throw new InvalidOperationException();
        }

        public ASeriesOfTubes RenderSilver(int renderEvery = 12)
        {
            return this.RenderPath(renderEvery, showLetters: true);
        }

        public ASeriesOfTubes RenderGold(int renderEvery = 12)
        {
            return this.RenderPath(renderEvery, showLetters: false);
        }

        private ASeriesOfTubes RenderPath(int renderEvery, bool showLetters)
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<TubeStep> path = this.TracePath();
            HashSet<Vector<int>> trail = [];

            const int viewportWidth = 100;
            const int viewportHeight = 42;

            for (int i = 0; i < path.Count; i++)
            {
                TubeStep step = path[i];
                trail.Add(step.Point);

                if (i % renderEvery == 0 || i == path.Count - 1)
                {
                    string title = showLetters
                        ? $"A SERIES OF TUBES // LETTERS {step.Letters} // STEPS {step.Steps:00000}"
                        : $"A SERIES OF TUBES // STEPS {step.Steps:00000} // LETTERS {step.Letters}";

                    this.Renderer.RenderFrame(new Frame(this.BuildViewportFrame(
                        step,
                        trail,
                        title,
                        viewportWidth,
                        viewportHeight)));
                }
            }

            TubeStep last = path.Last();

            for (int i = 0; i < 24; i++)
            {
                this.Renderer.RenderFrame(new Frame(this.BuildViewportFrame(
                    last,
                    trail,
                    $"PACKET DELIVERED // LETTERS {last.Letters} // STEPS {last.Steps:00000}",
                    viewportWidth,
                    viewportHeight)));
            }

            return this;
        }

        private List<TubeStep> TracePath()
        {
            List<TubeStep> path = [];
            StringBuilder letters = new();
            HashSet<Vector<int>> visited = [];
            Vector<int> point = new(this.Start);
            Cardinal direction = Cardinal.South;
            int steps = 0;

            while (true)
            {
                steps++;
                List<VectorCell<int, char>> cardinal = this.Diagram.AdjacentCardinal(point).ToList();
                char value = this.Diagram[point];
                visited.Add(new(point));

                if (value == '+')
                {
                    direction = cardinal
                        .First(c => c.Point != point && c.Value != ' ' && !visited.Contains(c.Point))
                        .Direction;
                }

                if (char.IsLetter(value))
                {
                    letters.Append(value);
                }

                path.Add(new(new(point), direction, value, letters.ToString(), steps));

                if (cardinal.First(c => c.Direction == direction).Value == ' ')
                {
                    return path;
                }

                point += CardinalHelper.CardinalTransform<int>()[direction];
            }
        }

        private string[] BuildViewportFrame(
            TubeStep current,
            HashSet<Vector<int>> trail,
            string title,
            int viewportWidth,
            int viewportHeight)
        {
            List<string> result = [];
            result.Add(title.PadRight(viewportWidth, ' '));
            result.Add(new string('─', viewportWidth));

            int minX = this.Diagram.Min(c => c.Key.X);
            int maxX = this.Diagram.Max(c => c.Key.X);
            int minY = this.Diagram.Min(c => c.Key.Y);
            int maxY = this.Diagram.Max(c => c.Key.Y);

            int left = Math.Clamp(current.Point.X - (viewportWidth / 2), minX, Math.Max(minX, maxX - viewportWidth + 1));
            int top = Math.Clamp(current.Point.Y - (viewportHeight / 2), minY, Math.Max(minY, maxY - viewportHeight + 1));

            for (int y = top; y < top + viewportHeight; y++)
            {
                StringBuilder sb = new();

                for (int x = left; x < left + viewportWidth; x++)
                {
                    Vector<int> point = new(x, y);
                    char value = this.Diagram[point];

                    if (point == current.Point)
                    {
                        sb.Append('@');
                    }
                    else if (trail.Contains(point))
                    {
                        sb.Append(char.IsLetter(value) ? value : '~');
                    }
                    else
                    {
                        sb.Append(value);
                    }
                }

                result.Add(sb.ToString());
            }

            result.Add(new string('─', viewportWidth));
            result.Add($"VIEW X {left:000}-{left + viewportWidth - 1:000} // Y {top:000}-{top + viewportHeight - 1:000}".PadRight(viewportWidth, ' '));

            return [.. result];
        }

        private VectorDictionary<int, char> ParseDiagram(string[] input)
        {
            return new(input, (c, x, y) =>
            {
                if (this.Start == new Vector<int>(0, 0) && c == '|')
                {
                    this.Start = new(x, y);
                }

                return c;
            });
        }
    }
}
