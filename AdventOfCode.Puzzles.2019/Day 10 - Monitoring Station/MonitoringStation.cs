namespace AdventOfCode.Puzzles._2019.Day_10___Monitoring_Station
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class MonitoringStation
    {
        public MonitoringStation(string[] input)
        {
            this.Asteroids = Parse(input);
            this.BestLocation = new();
            this.VaporizedLocation = new();
        }

        public int MaxTargets { get; private set; }

        public Vector<int> BestLocation { get; private set; }

        public Vector<int> VaporizedLocation { get; private set; }

        private List<Vector<int>> Asteroids { get; }

        public MonitoringStation FindBestLocation()
        {
            foreach (Vector<int> asteroidA in this.Asteroids)
            {
                int count = 0;

                foreach (Vector<int> asteroidB in this.Asteroids)
                {
                    if (asteroidB.Equals(asteroidA))
                    {
                        continue;
                    }

                    bool blocked = false;

                    foreach (Vector<int> asteroidC in this.Asteroids)
                    {
                        if (asteroidA.IsLineOfSight(asteroidB, asteroidC))
                        {
                            blocked = true;
                            break;
                        }
                    }

                    if (!blocked)
                    {
                        count++;
                    }
                }

                if (count > this.MaxTargets)
                {
                    this.MaxTargets = count;
                    this.BestLocation = asteroidA;
                }
            }

            return this;
        }

        public MonitoringStation ClearAsteroidField()
        {
            this.Asteroids.Remove(this.BestLocation);

            this.MapPointsToSlopes(this.BestLocation, out Dictionary<Vector<int>, float> slopeMappingsRight, out Dictionary<Vector<int>, float> slopeMappingsLeft);
            this.BlastAsteroids(this.BestLocation, slopeMappingsRight, slopeMappingsLeft);

            return this;
        }

        public long VaporizedScore() => (this.VaporizedLocation.X * 100) + this.VaporizedLocation.Y;

        private static List<Vector<int>> Parse(string[] input)
            => Vector<int>.AxisEnumerator(input[0].Length, input.Length)
            .Where(point => input[point.Y][point.X] == '#')
            .ToList();

        private static float GetSlope(Vector<int> asteroid, Vector<int> origin)
        {
            if (asteroid.X == origin.X)
            {
                return (asteroid.Y > origin.Y) ? float.MaxValue : float.MinValue;
            }

            return (asteroid.Y - origin.Y) / ((float)(asteroid.X - origin.X));
        }

        private void MapPointsToSlopes(Vector<int> origin, out Dictionary<Vector<int>, float> slopeMappingsRight, out Dictionary<Vector<int>, float> slopeMappingsLeft)
        {
            slopeMappingsRight = new();
            slopeMappingsLeft = new();

            foreach (Vector<int> asteroid in this.Asteroids)
            {
                if (asteroid.X >= origin.X)
                {
                    slopeMappingsRight.Add(asteroid, GetSlope(asteroid, origin));
                }
                else
                {
                    slopeMappingsLeft.Add(asteroid, GetSlope(asteroid, origin));
                }
            }
        }

        private void BlastAsteroids(Vector<int> origin, Dictionary<Vector<int>, float> slopeMappingsRight, Dictionary<Vector<int>, float> slopeMappingsLeft)
        {
            int count = 0;

            while (slopeMappingsRight.Any() || slopeMappingsLeft.Any())
            {
                foreach (KeyValuePair<Vector<int>, float> asteroid in slopeMappingsRight
                    .OrderBy(x => x.Value)
                    .ThenBy(x => x.Key.Distance(origin))
                    .GroupBy(x => x.Value)
                    .Select(x => x.First()))
                {
                    count++;

                    if (count == 200)
                    {
                        this.VaporizedLocation = asteroid.Key;
                        break;
                    }

                    slopeMappingsRight.Remove(asteroid.Key);
                }

                foreach (KeyValuePair<Vector<int>, float> asteroid in slopeMappingsLeft
                    .OrderBy(x => x.Value)
                    .ThenBy(x => x.Key.Distance(origin))
                    .GroupBy(x => x.Value)
                    .Select(x => x.First()))
                {
                    count++;

                    if (count == 200)
                    {
                        this.VaporizedLocation = asteroid.Key;
                        break;
                    }

                    slopeMappingsLeft.Remove(asteroid.Key);
                }

                if (count >= 200)
                {
                    break;
                }
            }
        }
    }
}
