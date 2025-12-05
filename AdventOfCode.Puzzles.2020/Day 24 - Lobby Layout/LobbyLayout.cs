namespace AdventOfCode.Puzzles._2020.Day_24___Lobby_Layout
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class LobbyLayout
    {
        public static Dictionary<Cardinal, Vector<int>> Directions => new()
        {
            { Cardinal.East, new(1, 0, -1) },
            { Cardinal.SouthEast, new(0, 1, -1) },
            { Cardinal.SouthWest, new(-1, 1, 0) },
            { Cardinal.West, new(-1, 0, 1) },
            { Cardinal.NorthWest, new(0, -1, 1) },
            { Cardinal.NorthEast, new(1, -1, 0) },
        };

        public static int BlackSideUp(string[] input)
        {
            VectorDictionary<int, bool> points = new() { { new(0, 0, 0), false } };

            foreach (string line in input)
            {
                Vector<int> point = Move(line, points);

                if (!points.ContainsKey(point))
                {
                    points.Add(point, true);
                }
                else
                {
                    points[point] = !points[point];
                }
            }

            return points.Values.Where(t => t).Count();
        }

        public static int BlackCount(string[] input)
        {
            VectorDictionary<int, bool> points = new() { { new(0, 0, 0), false } };

            foreach (string line in input)
            {
                Vector<int> point = Move(line, points);

                if (!points.ContainsKey(point))
                {
                    points.Add(point, true);
                }
                else
                {
                    points[point] = !points[point];
                }
            }

            for (int j = 0; j < 100; j++)
            {
                points = Iterate(points);
            }

            return points.Values.Where(t => t).Count();
        }

        private static Vector<int> Move(string path, VectorDictionary<int, bool> tiles)
        {
            Vector<int> hex = new(0, 0, 0);
            Stack<char> stack = path.ToStack();

            while (stack.Count > 0)
            {
                string direction = string.Empty;
                char @char = stack.Pop();
                direction += @char;

                if (@char == 'n' || @char == 's')
                {
                    direction += stack.Pop();
                }

                hex += Directions[CardinalHelper.CompassToCardinalMap[direction.ToUpper()]];

                Fill(hex, tiles);
            }

            return hex;
        }

        private static int Black(Vector<int> point, VectorDictionary<int, bool> points)
            => Directions.Select(a => a.Value)
                .Select(b => point + b)
                .Where(c => points.ContainsKey(c) && points[c])
                .Count();

        private static void Fill(Vector<int> point, VectorDictionary<int, bool> points)
        {
            foreach (KeyValuePair<Cardinal, Vector<int>> direction in Directions)
            {
                Vector<int> adjacent = point + direction.Value;

                if (!points.ContainsKey(adjacent))
                {
                    points.Add(adjacent, false);
                }
            }
        }

        private static VectorDictionary<int, bool> Iterate(VectorDictionary<int, bool> tiles)
        {
            VectorDictionary<int, bool> next = new();

            foreach (Vector<int> key in tiles.Keys)
            {
                int adjacent = Black(key, tiles);

                if (tiles[key])
                {
                    next.Add(key, adjacent == 1 || adjacent == 2);
                }
                else
                {
                    next.Add(key, adjacent == 2);
                }
            }

            foreach (Vector<int> key in next.Keys.Select(k => k).ToArray())
            {
                Fill(key, next);
            }

            return next;
        }
    }
}
