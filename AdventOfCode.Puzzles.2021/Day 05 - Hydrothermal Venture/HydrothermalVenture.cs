namespace AdventOfCode.Puzzles._2021.Day_05___Hydrothermal_Venture
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class HydrothermalVenture
    {
        public HydrothermalVenture(string[] input, bool includeDiagional)
        {
            (List<HydrothermalVent> vents, Vector<int> point) = Parse(input);
            this.Map = new (point.X + 1, point.Y + 1);
            this.MapVents(vents, includeDiagional);
        }

        public VectorArray<int, int> Map { get; }

        public int TotalCrossOverVents() => this.Map.AxisEnumerator().Count(cell => cell.Value >= 2);

        public void MapHorizontally(HydrothermalVent vent)
        {
            for (int y = Math.Min(vent.Origin.Y, vent.Destination.Y); y <= Math.Max(vent.Origin.Y, vent.Destination.Y); y++)
            {
                this.Map[y, vent.Origin.X]++;
            }
        }

        public void MapVertically(HydrothermalVent vent)
        {
            for (int x = Math.Min(vent.Origin.X, vent.Destination.X); x <= Math.Max(vent.Origin.X, vent.Destination.X); x++)
            {
                this.Map[vent.Origin.Y, x]++;
            }
        }

        public void MapDiagionally(HydrothermalVent vent)
        {
            int x = vent.Origin.X;
            int y = vent.Origin.Y;

            this.Map[y, x]++;

            switch (vent.Direction)
            {
                case Cardinal.NorthEast:
                    this.MoveNorthEast(x, y, vent);
                    return;
                case Cardinal.SouthEast:
                    this.MoveSouthEast(x, y, vent);
                    return;

                case Cardinal.SouthWest:
                    this.MoveSouthWest(x, y, vent);
                    return;

                case Cardinal.NorthWest:
                    this.MoveNorthWest(x, y, vent);
                    return;
            }
        }

        private static (List<HydrothermalVent> Vents, Vector<int> Point) Parse(string[] input)
        {
            List<HydrothermalVent> result = new();
            Vector<int> point = new(0, 0);

            foreach (string line in input)
            {
                string[] tokens = line.Split(" -> ");
                HydrothermalVent vent = new(new(tokens[0].Split(",").ToInt()), new(tokens[1].Split(",").ToInt()));

                point.X = Math.Max(point.X, vent.Origin.X);
                point.X = Math.Max(point.X, vent.Destination.X);
                point.Y = Math.Max(point.Y, vent.Origin.Y);
                point.Y = Math.Max(point.Y, vent.Destination.Y);

                result.Add(vent);
            }

            return (result, point);
        }

        private void MapVents(List<HydrothermalVent> vents, bool includeDiagional)
        {
            foreach (HydrothermalVent vent in vents)
            {
                if (vent.IsHorizontal)
                {
                    this.MapHorizontally(vent);
                    continue;
                }
                else if (vent.IsVertical)
                {
                    this.MapVertically(vent);
                    continue;
                }
                else if (!includeDiagional)
                {
                    continue;
                }

                if (vent.IsDiagional)
                {
                    this.MapDiagionally(vent);
                }
            }
        }

        private void MoveNorthEast(int x, int y, HydrothermalVent vent)
        {
            for (int i = 1; i <= Math.Abs(vent.Origin.X - vent.Destination.X); i++)
            {
                x++;
                y--;
                this.Map[y, x]++;
            }
        }

        private void MoveSouthEast(int x, int y, HydrothermalVent vent)
        {
            for (int i = 1; i <= Math.Abs(vent.Origin.X - vent.Destination.X); i++)
            {
                x++;
                y++;
                this.Map[y, x]++;
            }
        }

        private void MoveSouthWest(int x, int y, HydrothermalVent vent)
        {
            for (int i = 1; i <= Math.Abs(vent.Origin.X - vent.Destination.X); i++)
            {
                x--;
                y++;
                this.Map[y, x]++;
            }
        }

        private void MoveNorthWest(int x, int y, HydrothermalVent vent)
        {
            for (int i = 1; i <= Math.Abs(vent.Origin.X - vent.Destination.X); i++)
            {
                x--;
                y--;
                this.Map[y, x]++;
            }
        }
    }
}
