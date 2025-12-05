namespace AdventOfCode.Puzzles._2017.Day_19___A_Series_of_Tubes
{
    using System.Text;
    using AdventOfCode.Core;

    public class ASeriesOfTubes
    {
        public ASeriesOfTubes(string[] input)
        {
            this.Start = new(0, 0);
            this.Diagram = this.ParseDiagram(input);
        }

        public VectorDictionary<int, char> Diagram { get; }

        public Vector<int> Start { get; private set; }

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
