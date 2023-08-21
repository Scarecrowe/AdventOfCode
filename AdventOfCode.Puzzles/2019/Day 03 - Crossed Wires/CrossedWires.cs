namespace AdventOfCode.Puzzles._2019.Day_03___Crossed_Wires
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class CrossedWires
    {
        public static long Nearest(string[] input)
        {
            Dictionary<string, int> wireA = TraceWire(input[0]);
            Dictionary<string, int> wireB = TraceWire(input[1]);

            long result = int.MaxValue;

            foreach (KeyValuePair<string, int> item in wireA)
            {
                int[] coordinate = item.Key.Split(":").ToInt();

                if (item.Value >= 1 && coordinate[0] != 0 && coordinate[1] != 0)
                {
                    if (wireB.ContainsKey(item.Key))
                    {
                        long distance = new Vector<int>(0, 0).Distance(new(coordinate));

                        if (distance < result)
                        {
                            result = distance;
                        }
                    }
                }
            }

            return result;
        }

        public static int StepsToIntersection(string[] input)
        {
            Dictionary<string, int> wireA = TraceWire(input[0]);
            Dictionary<string, int> wireB = TraceWire(input[1]);

            SortedDictionary<string, int> intersectionsA = TraceIntersections(wireA, wireB);
            SortedDictionary<string, int> intersectionsB = TraceIntersections(wireB, wireA);

            int result = int.MaxValue;

            for (int i = 0; i < intersectionsA.Count; i++)
            {
                int steps = intersectionsA.ElementAt(i).Value + intersectionsB.ElementAt(i).Value;

                if (steps < result)
                {
                    result = steps;
                }
            }

            return result;
        }

        private static void AddJoin(ref Dictionary<string, int> map, Vector<int> point)
        {
            if (!map.ContainsKey(point.ToKey2D()))
            {
                map.Add(point.ToKey2D(), 1);
            }
            else
            {
                map.Add($"{point.ToKey2D()}:1", 1);
            }
        }

        private static Dictionary<string, int> TraceWire(string input)
        {
            Dictionary<string, int> map = new();

            string[] wire = input.Split(",", StringSplitOptions.RemoveEmptyEntries);

            int x = 0;
            int y = 0;

            foreach (string join in wire)
            {
                int length = join[1..].ToInt();

                switch (join[0])
                {
                    case 'U':
                        for (int i = y - 1; i >= y - length; i--)
                        {
                            AddJoin(ref map, new(x, i));
                        }

                        y -= length;

                        break;
                    case 'D':
                        for (int i = y + 1; i <= y + length; i++)
                        {
                            AddJoin(ref map, new(x, i));
                        }

                        y += length;

                        break;
                    case 'L':
                        for (int i = x - 1; i >= x - length; i--)
                        {
                            AddJoin(ref map, new(i, y));
                        }

                        x -= length;

                        break;
                    case 'R':
                        for (int i = x + 1; i <= x + length; i++)
                        {
                            AddJoin(ref map, new(i, y));
                        }

                        x += length;
                        break;
                }
            }

            return map;
        }

        private static SortedDictionary<string, int> TraceIntersections(Dictionary<string, int> mapA, Dictionary<string, int> mapB)
        {
            SortedDictionary<string, int> intersections = new();
            int index = 1;

            Dictionary<string, int> visited = new();

            foreach (KeyValuePair<string, int> wire in mapA)
            {
                string key = wire.Key.Contains('#')
                     ? wire.Key[..wire.Key.IndexOf("#")]
                     : wire.Key;

                if (!visited.ContainsKey(wire.Key))
                {
                    visited.Add(wire.Key, index);
                }
                else
                {
                    index = visited[key];
                }

                if (mapB.ContainsKey(wire.Key))
                {
                    intersections.Add(wire.Key, index);
                }

                index++;
            }

            return intersections;
        }
    }
}
