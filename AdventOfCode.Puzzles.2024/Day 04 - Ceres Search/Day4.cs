namespace AdventOfCode.Puzzles._2024.Days
{
    using AdventOfCode.Core;

    public class Day4 : Puzzle, IPuzzle
    {
        public Day4()
        {
            this.DayTitle = "Ceres Search";
            this.GetPuzzleData(4, this.DayTitle);
            this.Map = new(this.Input, c => c);
        }

        public string Silver()
        {
            VectorArray<int, char> map = new (this.Input, c => c);

            int result = 0;

            for(int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    foreach(var cardinal in Enum.GetValues<Cardinal>())
                    {
                        if (FindTerm(map, new Vector<int>(x, y), cardinal, "XMAS").Count == 4)
                        {
                            result++;
                        }
                    }
                }
            }

            return $"{result}";
        }

        private void Search(string term, List<Cardinal> directions, Vector<int> point)
        {
            foreach(Cardinal direction in directions)
            {
                List<Vector<int>> points = FindTerm(this.Map, point, direction, term);

                if (points.Count == term.Length)
                {
                    this.Locations.Add(points);
                }
            }
        }

        VectorArray<int, char> Map;

        HashSet<List<Vector<int>>> Locations = new();

        private void Filter(ref int result)
        {
            Dictionary<string, Vector<int>> processed = new ();
            List<Vector<int>> tree = new();

            foreach (var locationA in this.Locations)
            {
                if (processed.ContainsKey(locationA[1].ToKey2D()))
                {
                    continue;
                }

                foreach (var locationB in this.Locations)
                {
                    if (locationA != locationB
                        && locationA[1] == locationB[1])
                    {
                        tree.Clear();
                        tree.AddRange(locationA);
                        tree.AddRange(locationB);

                        if (new[]
                        {
                            locationA[1] + Vector<int>.NorthWest,
                            locationA[1] + Vector<int>.NorthEast,
                            locationA[1] + Vector<int>.SouthWest,
                            locationA[1] + Vector<int>.SouthEast
                        }.All(tree.Contains))
                        {
                            processed.Add(locationA[1].ToKey2D(), locationA[1]);
                            result++;
                        }                       
                        
                        break;
                    }
                }
            }
        }

        public string Gold()
        {
            int result = 0;

            foreach (VectorCell<int, char> cell in this.Map.AxisEnumerator())
            {
                this.Search("MAS", new List<Cardinal> { Cardinal.NorthEast, Cardinal.NorthWest, Cardinal.SouthEast, Cardinal.SouthWest}, cell.Point);
                this.Search("SAM", new List<Cardinal> { Cardinal.NorthEast, Cardinal.NorthWest, Cardinal.SouthEast, Cardinal.SouthWest }, cell.Point);
            }

            this.Filter(ref result);

            return $"{result}";
        }

        private static List<Vector<int>> FindTerm(VectorArray<int, char> map, Vector<int> point, Cardinal direction, string term)
        {
            List<Vector<int>> points = new ();

            if (map[point] != term[0])
            {
                return points;
            }

            points.Add(point);

            for(int i = 1; i <= term.Length - 1; i++)
            {
                var adjacent = map.AdjacentInterCardinal(point);

                if (!adjacent.Any(x => x.Direction == direction && x.Value == term[i]))
                {
                    return points;
                }

                point = adjacent.First(x => x.Direction == direction).Point;
                points.Add(point);
            }

            return points;
        }
    }
}
