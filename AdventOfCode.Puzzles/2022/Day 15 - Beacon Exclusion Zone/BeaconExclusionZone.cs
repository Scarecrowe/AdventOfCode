namespace AdventOfCode.Puzzles._2022.Day_15___Beacon_Exclusion_Zone
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class BeaconExclusionZone
    {
        public BeaconExclusionZone(string[] input)
        {
            this.Map = new();
            this.Sensors = new();
            this.ParseSensors(input);
        }

        public VectorDictionary<int, int> Map { get; private set; }

        public List<Sensor> Sensors { get; private set; }

        public long NonBeacon()
        {
            long min = this.GetDistances().Min();
            long max = this.GetDistances().Max();

            long total = 0;
            for (long i = min; i <= max; ++i)
            {
                long maxX = this.MaxX(i, min);

                if (maxX > min - 1)
                {
                    total += maxX - i + 1 - this.Map.Count(m => m.Key.Y == 2000000 && m.Key.X >= i && m.Key.X <= maxX);
                    i = maxX;
                }
            }

            return total;
        }

        public object TuningFrequency()
        {
            foreach (Sensor sensor in this.Sensors)
            {
                for (long i = Math.Max(0, sensor.Point.X - sensor.Distance - 1); i <= Math.Min(4000000, sensor.Point.X + sensor.Distance + 1); ++i)
                {
                    long positiveY = Math.Min(sensor.Point.Y + sensor.Distance + 1 - (sensor.Point.X - i).Abs(), 4000000);
                    long negativeY = Math.Max(sensor.Point.Y - (sensor.Distance + 1 - (sensor.Point.X - i).Abs()), 0);

                    if (this.Sensors.All(sensor => (sensor.Point.X - i).Abs() + (sensor.Point.Y - positiveY).Abs() > sensor.Distance))
                    {
                        return (i * 4000000) + positiveY;
                    }

                    if (this.Sensors.All(sensor => (sensor.Point.X - i).Abs() + (sensor.Point.Y - negativeY).Abs() > sensor.Distance))
                    {
                        return (i * 4000000) + negativeY;
                    }
                }
            }

            throw new InvalidOperationException();
        }

        private void ParseSensors(string[] input) => input.ForEach(x => this.ParseSensor(x));

        private void ParseSensor(string input)
        {
            string[] tokens = input.Replace(",").Replace(":").SplitSpace();
            Vector<int> beacon = new(tokens[8].Split('=')[1].ToLong(), tokens[9].Split('=')[1].ToLong());
            Vector<int> sensor = new(tokens[2].Split('=')[1].ToLong(), tokens[3].Split('=')[1].ToLong());
            this.Map.Add(beacon, 1);
            this.Map.Add(sensor, 1);
            this.Sensors.Add(new(sensor, sensor.Distance(beacon)));
        }

        private IEnumerable<long> GetDistances() => this.Sensors.Select(sensor => sensor.Point.X - (sensor.Distance - (sensor.Point.Y - 2000000).Abs()));

        private long MaxX(long i, long min) => this.Sensors
                    .Where(sensor => (sensor.Point.X - i).Abs() + (sensor.Point.Y - 2000000).Abs() <= sensor.Distance)
                    .Select(sensor => sensor.Distance + sensor.Point.X - (sensor.Point.Y - 2000000).Abs())
                    .OrderByDescending(sensor => sensor)
                    .FirstOrDefault(min - 1);
    }
}
