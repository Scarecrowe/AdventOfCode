namespace AdventOfCode.Puzzles._2017.Day_11___Hex_Ed
{
    using AdventOfCode.Core;

    public class HexEd
    {
        public HexEd(string input)
        {
            string[] steps = input.Split(',');
            Vector<int> position = new(0, 0, 0);

            foreach (string step in steps)
            {
                this.Distance = Move(step, ref position);

                if (this.Distance > this.Furthest)
                {
                    this.Furthest = this.Distance;
                }
            }
        }

        public int Furthest { get; private set; }

        public int Distance { get; private set; }

        public static Dictionary<Cardinal, Vector<TSize>> Transform3D<TSize>() => new()
        {
            { Cardinal.North, new(0, 1, -1) },
            { Cardinal.South, new(0, -1, 1) },
            { Cardinal.NorthEast, new(1, 0, -1) },
            { Cardinal.SouthWest, new(-1, 0, 1) },
            { Cardinal.NorthWest, new(-1, 1, 0) },
            { Cardinal.SouthEast, new(1, -1, 0) },
        };

        public static int Move(string step, ref Vector<int> position)
        {
            position += Transform3D<int>()[CardinalHelper.CompassToCardinalMap[step.ToUpper()]];

            return position.Absolute() / 2;
        }
    }
}
