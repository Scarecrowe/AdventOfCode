namespace AdventOfCode.Puzzles._2019.Day_10___Monitoring_Station
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    using System.Text;

    public class MonitoringStation
    {
        public MonitoringStation(string[] input)
        {
            this.Asteroids = Parse(input);
            this.BestLocation = new();
            this.VaporizedLocation = new();
        }

        public MonitoringStation(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

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

        public Vector<int> TwoHundredthVaporized()
        {
            return this.VaporizationOrder()[199];
        }

        public void Render()
        {
            List<Vector<int>> order = this.VaporizationOrder();
            HashSet<Vector<int>> remaining = this.Asteroids.ToHashSet();

            int count = 0;

            foreach (Vector<int> vaporized in order)
            {
                count++;

                remaining.Remove(vaporized);

                this.Renderer?.RenderFrame(
                    new Frame(this.BuildFrame(remaining, vaporized, count)));
            }
        }

        public void RenderGold()
        {
            if (this.Renderer == null)
            {
                return;
            }

            this.FindBestLocation();

            List<Vector<int>> order = this.VaporizationOrderGold();
            HashSet<Vector<int>> remaining = this.Asteroids.ToHashSet();

            remaining.Remove(this.BestLocation);

            for (int i = 0; i < order.Count; i++)
            {
                int count = i + 1;
                Vector<int> target = order[i];

                this.Renderer.RenderFrame(
                    new Frame(this.BuildGoldFrame(remaining, target, count, LaserState.Aiming)));

                this.Renderer.RenderFrame(
                    new Frame(this.BuildGoldFrame(remaining, target, count, LaserState.Explosion)));

                remaining.Remove(target);

                this.Renderer.RenderFrame(
                    new Frame(this.BuildGoldFrame(remaining, target, count, LaserState.Cleared)));

                if (count == 200)
                {
                    this.VaporizedLocation = target;

                    for (int hold = 0; hold < 20; hold++)
                    {
                        this.Renderer.RenderFrame(
                            new Frame(this.BuildGoldFrame(remaining, target, count, LaserState.Explosion)));
                    }
                }
            }
        }

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

        private List<Vector<int>> VaporizationOrder()
        {
            if (this.BestLocation == new Vector<int>())
            {
                this.FindBestLocation();
            }

            List<Vector<int>> remaining = this.Asteroids
                .Where(a => a != this.BestLocation)
                .ToList();

            List<Vector<int>> result = [];

            while (remaining.Count > 0)
            {
                List<Vector<int>> round = remaining
                    .GroupBy(a => AngleFromUpClockwise(this.BestLocation, a))
                    .OrderBy(g => g.Key)
                    .Select(g => g.OrderBy(a => a.Distance(this.BestLocation)).First())
                    .ToList();

                foreach (Vector<int> asteroid in round)
                {
                    result.Add(asteroid);
                    remaining.Remove(asteroid);
                }
            }

            return result;
        }

        private List<Vector<int>> VaporizationOrderGold()
        {
            List<Vector<int>> remaining = this.Asteroids
                .Where(a => a != this.BestLocation)
                .ToList();

            List<Vector<int>> order = [];

            while (remaining.Count > 0)
            {
                List<Vector<int>> round = remaining
                    .GroupBy(a => AngleFromUpClockwise(this.BestLocation, a))
                    .OrderBy(g => g.Key)
                    .Select(g => g.OrderBy(a => a.Distance(this.BestLocation)).First())
                    .ToList();

                foreach (Vector<int> asteroid in round)
                {
                    order.Add(asteroid);
                    remaining.Remove(asteroid);
                }
            }

            return order;
        }

        private static double AngleFromUpClockwise(Vector<int> origin, Vector<int> asteroid)
        {
            int dx = asteroid.X - origin.X;
            int dy = asteroid.Y - origin.Y;

            double angle = Math.Atan2(dx, -dy);

            if (angle < 0)
            {
                angle += Math.PI * 2;
            }

            return angle;
        }

        private string[] BuildFrame(
            HashSet<Vector<int>> remaining,
            Vector<int> vaporized,
            int count)
        {
            int width = remaining
                .Append(this.BestLocation)
                .Append(vaporized)
                .Max(p => p.X) + 1;

            int height = remaining
                .Append(this.BestLocation)
                .Append(vaporized)
                .Max(p => p.Y) + 1;

            List<string> result = [];

            result.Add($"MONITORING STATION // VAPORIZED {count:000}");
            result.Add($"BEST LOCATION: {this.BestLocation.X},{this.BestLocation.Y}");
            result.Add(string.Empty);

            for (int y = 0; y < height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < width; x++)
                {
                    Vector<int> point = new(x, y);

                    if (point == this.BestLocation)
                    {
                        sb.Append('S');
                    }
                    else if (point == vaporized)
                    {
                        sb.Append('*');
                    }
                    else if (remaining.Contains(point))
                    {
                        sb.Append('#');
                    }
                    else
                    {
                        sb.Append('.');
                    }
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private string[] BuildGoldFrame(
            HashSet<Vector<int>> remaining,
            Vector<int> target,
            int vaporizedCount,
            LaserState state)
        {
            int width = this.Asteroids.Max(p => p.X) + 1;
            int height = this.Asteroids.Max(p => p.Y) + 1;

            HashSet<Vector<int>> beam = state == LaserState.Aiming
                ? GetLine(this.BestLocation, target).ToHashSet()
                : [];

            List<string> result = [];

            double angle = AngleFromUpClockwise(this.BestLocation, target) * 180 / Math.PI;

            result.Add($"MONITORING STATION LASER // VAPORIZED {vaporizedCount:000} // ANGLE {angle:000.0}");
            result.Add($"STATION {this.BestLocation.X},{this.BestLocation.Y} // TARGET {target.X},{target.Y}");
            result.Add(string.Empty);

            for (int y = 0; y < height; y++)
            {
                StringBuilder sb = new();

                for (int x = 0; x < width; x++)
                {
                    Vector<int> point = new(x, y);

                    if (point == this.BestLocation)
                    {
                        sb.Append('S');
                    }
                    else if (point == target && vaporizedCount == 200)
                    {
                        sb.Append('2');
                    }
                    else if (point == target && state == LaserState.Explosion)
                    {
                        sb.Append('*');
                    }
                    else if (beam.Contains(point) && point != target)
                    {
                        sb.Append(GetBeamCharacter(this.BestLocation, target));
                    }
                    else if (remaining.Contains(point))
                    {
                        sb.Append('#');
                    }
                    else
                    {
                        sb.Append('.');
                    }
                }

                result.Add(sb.ToString());
            }

            return [.. result];
        }

        private static IEnumerable<Vector<int>> GetLine(Vector<int> from, Vector<int> to)
        {
            int x0 = from.X;
            int y0 = from.Y;
            int x1 = to.X;
            int y1 = to.Y;

            int dx = Math.Abs(x1 - x0);
            int dy = -Math.Abs(y1 - y0);

            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;

            int error = dx + dy;

            while (true)
            {
                yield return new(x0, y0);

                if (x0 == x1 && y0 == y1)
                {
                    break;
                }

                int e2 = 2 * error;

                if (e2 >= dy)
                {
                    error += dy;
                    x0 += sx;
                }

                if (e2 <= dx)
                {
                    error += dx;
                    y0 += sy;
                }
            }
        }

        private static char GetBeamCharacter(Vector<int> from, Vector<int> to)
        {
            int dx = to.X - from.X;
            int dy = to.Y - from.Y;

            if (dx == 0)
            {
                return '|';
            }

            if (dy == 0)
            {
                return '-';
            }

            return Math.Sign(dx) == Math.Sign(dy)
                ? '\\'
                : '/';
        }
    }
}
