namespace AdventOfCode.Puzzles._2024.Day_10___Hoof_It
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using System.Linq;

    public class HoofIt
    {
        public VectorArray<int, int> Map { get; private set; }

        public HoofIt(string[] input)
        {
            this.Map = new(input, x => x == '.' ? -1 : int.Parse($"{x}"));
        }

        public long TrailHeads(bool rating)
        {
            Queue<(Vector<int> Point, int Distance, List<Vector<int>> Visited)> queue = new();
            Dictionary<Vector<int>, HashSet<Vector<int>>> trails = new();
            Dictionary<Vector<int>, int> trailCounts = new();

            long result = 0;

            foreach(var cell in this.Map.AxisEnumerator())
            {
                if (cell.Value == 0)
                {
                    trails.Add(cell.Point, new());
                    trailCounts.Add(cell.Point, 0);

                    queue.Clear();
                    queue.Enqueue((cell.Point, 0, new List<Vector<int>>() {  }));

                    while(queue.Any())
                    {
                        var state = queue.Dequeue();

                        foreach(var adjacent in this.Map.AdjacentCardinal(state.Point))
                        {
                            if (!rating && state.Visited.Contains(adjacent.Point))
                            {
                                continue;
                            }

                            if (adjacent.Value == state.Distance + 1)
                            {
                                if (adjacent.Value == 9)
                                {
                                    result++;

                                    if (trails[cell.Point].Contains(adjacent.Point))
                                    {
                                        continue;
                                    }

                                    trails[cell.Point].Add(adjacent.Point);                                    
                                }
                                else
                                {
                                    state.Visited.Add(adjacent.Point);
                                    queue.Enqueue((adjacent.Point, adjacent.Value, state.Visited));
                                }
                            }
                        }
                    }
                }
            }

            return rating ? result : trails.Sum(pair => pair.Value.Count());
        }
    }
}
