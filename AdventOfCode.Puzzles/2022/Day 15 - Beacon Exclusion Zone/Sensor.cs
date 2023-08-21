namespace AdventOfCode.Puzzles._2022.Day_15___Beacon_Exclusion_Zone
{
    using AdventOfCode.Core;

    public class Sensor
    {
        public Sensor(Vector<int> point, long distance)
        {
            this.Point = point;
            this.Distance = distance;
        }

        public Vector<int> Point { get; }

        public long Distance { get; }
    }
}
