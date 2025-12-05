namespace AdventOfCode.Puzzles._2021.Day_19___Beacon_Scanner
{
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;

    public class Scanner
    {
        public Scanner(Vector<int> point, int rotation, List<Vector<int>> beacons)
        {
            this.Point = point;
            this.Rotation = rotation;
            this.Beacons = beacons;
        }

        public Vector<int> Point { get; }

        public int Rotation { get; private set; }

        public List<Vector<int>> Beacons { get; }

        public static List<Scanner> Build(string[] input)
        {
            List<Scanner> scanners = new(Parse(input.ToList()));
            List<Scanner> found = new() { scanners[0] };
            Queue<Scanner> queue = new();

            queue.Enqueue(scanners[0]);
            scanners.Remove(scanners[0]);

            while (queue.Any())
            {
                Scanner scannerA = queue.Dequeue();

                foreach (Scanner scannerB in scanners.ToArray())
                {
                    Scanner? scanner = scannerA.Find(scannerB);

                    if (scanner != null)
                    {
                        found.Add(scanner);
                        queue.Enqueue(scanner);
                        scanners.Remove(scannerB);
                    }
                }
            }

            return found;
        }

        public Scanner Clone() => new(this.Point, this.Rotation, this.Beacons);

        public Scanner Rotate()
        {
            this.Rotation++;

            return this;
        }

        public List<int> Points() => this.Map().SelectMany(x => x.AbsoluteValues()).ToList();

        public Scanner Translate(Vector<int> translation) => new(this.Point + translation, this.Rotation, this.Beacons);

        public Vector<int> Transform(Vector<int> point)
        {
            int x = point.X;
            int y = point.Y;
            int z = point.Z;

#pragma warning disable CS1717 // Assignment made to same variable
            switch (this.Rotation % 6)
            {
                case 1: (x, y, z) = (-x, y, -z); break;
                case 2: (x, y, z) = (y, -x, z); break;
                case 3: (x, y, z) = (-y, x, z); break;
                case 4: (x, y, z) = (z, y, -x); break;
                case 5: (x, y, z) = (-z, y, x); break;
            }

            switch ((this.Rotation / 6) % 4)
            {
                case 1: (x, y, z) = (x, -z, y); break;
                case 2: (x, y, z) = (x, -y, -z); break;
                case 3: (x, y, z) = (x, z, -y); break;
            }
#pragma warning restore CS1717 // Assignment made to same variable

            return this.Point + new Vector<int>(x, y, z);
        }

        public IEnumerable<Vector<int>> Map() => this.Beacons.Select(this.Transform);

        public IEnumerable<(Vector<int> beaconA, Vector<int> beaconB)> Matches(Scanner scanner)
        {
            foreach (Vector<int> beaconA in this.Map().TakeCount(11))
            {
                List<int> pointsA = this.Translate(beaconA.Negate()).Points();

                foreach (Vector<int> beaconB in scanner.Map().TakeCount(11))
                {
                    List<int> pointsB = scanner.Translate(beaconB.Negate()).Points();

                    if (pointsB.Count(d => pointsA.Contains(d)) >= 3 * 12)
                    {
                        yield return (beaconA, beaconB);
                    }
                }
            }
        }

        public Scanner? Find(Scanner scanner)
        {
            Vector<int>[] beaconsA = this.Map().ToArray();

            foreach (var (beaconA, beaconB) in this.Matches(scanner))
            {
                Scanner rotated = scanner;

                for (int i = 0; i < 24; i++)
                {
                    rotated = rotated.Clone().Rotate();

                    Scanner locatedB = rotated.Translate(beaconA - rotated.Transform(beaconB));

                    if (locatedB.Map().Intersect(beaconsA).Count() >= 12)
                    {
                        return locatedB;
                    }
                }
            }

            return null;
        }

        private static Scanner[] Parse(List<string> input)
        {
            input.Add(string.Empty);
            List<Scanner> result = new();
            Scanner? scanner = null;

            foreach (string line in input)
            {
                if (line.StartsWith("---"))
                {
                    scanner = new(new(0, 0, 0), 0, new());
                }
                else if (string.IsNullOrEmpty(line))
                {
                    result.Add(scanner ?? new(new(0, 0, 0), 0, new()));
                }
                else
                {
                    scanner?.Beacons.Add(new(line.Split(",").ToInt()));
                }
            }

            return result.ToArray();
        }
    }
}
