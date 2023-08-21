namespace AdventOfCode.Puzzles._2016.Day_17___Two_Steps_Forward
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class TwoStepsForward
    {
        public TwoStepsForward()
        {
            this.Vault = CreateVault();
            this.Results = new();
        }

        public VectorArray<int, EntityType> Vault { get; }

        private static List<char> Open { get; } = new() { 'b', 'c', 'd', 'e', 'f' };

        private List<string> Results { get; }

        public TwoStepsForward AccessVault(string initial)
        {
            Queue<(Vector<int> Point, string Path)> queue = new();

            queue.Enqueue((new Vector<int>(1, 1), string.Empty));

            while (queue.Count > 0)
            {
                (Vector<int> Point, string Path) current = queue.Dequeue();

                if (this.Results.Contains(current.Path))
                {
                    continue;
                }

                string passcode = $"{initial}{current.Path}".ToMd5().ToLower()[..4];

                bool north = Open.Contains(passcode[0]);
                bool south = Open.Contains(passcode[1]);
                bool west = Open.Contains(passcode[2]);
                bool east = Open.Contains(passcode[3]);

                List<VectorCell<int, EntityType>> adjacent = this.Vault.AdjacentCardinal(current.Point)
                    .Where(x => x.Value == EntityType.DoorHorizontal || x.Value == EntityType.DoorVertical)
                    .Where(x => (x.Direction == Cardinal.North && north)
                    || (x.Direction == Cardinal.South && south)
                    || (x.Direction == Cardinal.West && west)
                    || (x.Direction == Cardinal.East && east))
                   .ToList();

                bool southFinished = false;
                bool eastFinished = false;

                if (adjacent.Any(cell => (cell.Point.Y == 7 && cell.Point.X == 6 && east) || (cell.Point.Y == 6 && cell.Point.X == 7 && south)))
                {
                    southFinished = south;
                    eastFinished = east;
                    this.Results.Add($"{current.Path}{(east ? 'R' : 'D')}");
                }

                foreach (VectorCell<int, EntityType> cell in adjacent)
                {
                    if (cell.Direction == Cardinal.North)
                    {
                        queue.Enqueue((new(cell.Point.X, cell.Point.Y - 1), $"{current.Path}{CardinalHelper.CardinalToLetterMap[cell.Direction]}"));
                        continue;
                    }

                    if (cell.Direction == Cardinal.South && !southFinished)
                    {
                        queue.Enqueue((new(cell.Point.X, cell.Point.Y + 1), $"{current.Path}{CardinalHelper.CardinalToLetterMap[cell.Direction]}"));
                        continue;
                    }

                    if (cell.Direction == Cardinal.West)
                    {
                        queue.Enqueue((new(cell.Point.X - 1, cell.Point.Y), $"{current.Path}{CardinalHelper.CardinalToLetterMap[cell.Direction]}"));
                        continue;
                    }

                    if (cell.Direction == Cardinal.East && !eastFinished)
                    {
                        queue.Enqueue((new(cell.Point.X + 1, cell.Point.Y), $"{current.Path}{CardinalHelper.CardinalToLetterMap[cell.Direction]}"));
                    }
                }
            }

            return this;
        }

        public string ShortestPath() => this.Results.OrderBy(x => x.Length).ElementAt(0);

        public int LongestPath() => this.Results.OrderByDescending(x => x.Length).ElementAt(0).Length;

        private static VectorArray<int, EntityType> CreateVault()
        {
            string[] vault = new string[9];
            vault[0] = "#########";
            vault[1] = "#S| | | #";
            vault[2] = "#-#-#-#-#";
            vault[3] = "# | | | #";
            vault[4] = "#-#-#-#-#";
            vault[5] = "# | | | #";
            vault[6] = "#-#-#-#-#";
            vault[7] = "# | | |  ";
            vault[8] = "####### V";

            return new(vault, "#-|SV ");
        }
    }
}
