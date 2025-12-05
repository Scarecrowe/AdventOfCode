namespace AdventOfCode.Puzzles._2024.Day_08___Resonant_Collinearity
{
    using AdventOfCode.Core;

    public class ResonantCollinearity
    {
        public VectorArray<int, char> Map { get; private set; }

        public Dictionary<char, HashSet<Vector<int>>> Antennas { get; private set; }

        public ResonantCollinearity(string[] input)
        {
            this.Map = new(input, c => c);
            this.Antennas = this.GetAntennas();
        }

        private Dictionary<char, HashSet<Vector<int>>> GetAntennas()
        {
            Dictionary<char, HashSet<Vector<int>>> result = [];

            foreach (var cell in this.Map.AxisEnumerator().Where(x => x.Value != '.'))
            {
                if (!result.ContainsKey(cell.Value))
                {
                    result[cell.Value] = new();
                }

                result[cell.Value].Add(cell.Point);
            }

            return result;
        }

        private static List<Vector<int>> PathToAntenna(Vector<int> start, Vector<int> end)
        {
            List<Vector<int>> path = new();

            int currentX = start.X;
            int currentY = start.Y;

            while (currentX != end.X)
            {
                path.Add(new(currentX, currentY));
                currentX += (end.X > currentX) ? 1 : -1;
            }

            while (currentY != end.Y)
            {
                path.Add(new(currentX, currentY));
                currentY += (end.Y > currentY) ? 1 : -1;
            }

            path.Add(new(currentX, currentY));

            return path;
        }

        private static List<Vector<int>> PathToNode(List<Vector<int>> path)
        {
            var result = new List<Vector<int>>();
            Vector<int> current = new(path[0].X, path[0].Y);

            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector<int> start = new(path[i].X, path[i].Y);
                Vector<int> end = new(path[i + 1].X, path[i + 1].Y);
                Vector<int> delta = new(start - end);

                current += delta;
                result.Add(current);
            }

            return result;
        }

        private static List<Vector<int>> PathDeltas(List<Vector<int>> path)
        {
            List<Vector<int>> result = [];

            for (int i = 0; i < path.Count - 1; i++)
            {
                result.Add(new(path[i] - path[i + 1]));
            }

            return result;
        }

        private void UniqueNodes(HashSet<Vector<int>> result, Vector<int> pointA, Vector<int> pointB)
        {
            List<Vector<int>> path = PathToAntenna(pointA, pointB);
            Vector<int> nodePoint = PathToNode(path).Last();

            if (!result.Contains(nodePoint)
                && this.Map.IsVectorInRange(nodePoint))
            {
                result.Add(nodePoint);
            }

            path.Reverse();
            nodePoint = PathToNode(path).Last();

            if (!result.Contains(nodePoint)
                && this.Map.IsVectorInRange(nodePoint))
            {
                result.Add(nodePoint);
            }
        }

        private void Resonate(HashSet<Vector<int>> result, Vector<int> pointA, Vector<int> pointB)
        {
            List<Vector<int>> path = PathToAntenna(pointA, pointB);
            List<Vector<int>> deltas = PathDeltas(path);
            Vector<int> current = pointB;

            while (true)
            {
                foreach (var delta in deltas)
                {
                    current += delta;
                }

                if (this.Map.IsVectorInRange(current))
                {
                    if (!result.Contains(current))
                    {
                        result.Add(current);
                    }
                }
                else
                {
                    break;
                }
            }
        }

        public HashSet<Vector<int>> AntiNodes(bool resonate = false)
        {
            HashSet<Vector<int>> result = new();

            foreach (var antenna in this.Antennas)
            {
                foreach (var pointA in antenna.Value)
                {
                    foreach (var pointB in antenna.Value)
                    {
                        if (pointA != pointB)
                        {
                            if (!resonate)
                            {
                                this.UniqueNodes(result, pointA, pointB);
                            }
                            else
                            {
                                this.Resonate(result, pointA, pointB);
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
