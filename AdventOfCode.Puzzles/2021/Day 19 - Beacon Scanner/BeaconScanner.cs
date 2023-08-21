namespace AdventOfCode.Puzzles._2021.Day_19___Beacon_Scanner
{
    public class BeaconScanner
    {
        public BeaconScanner(string[] input) => this.Scanners = Scanner.Build(input);

        public List<Scanner> Scanners { get; }

        public int BeaconCount() => this.Scanners.SelectMany(x => x.Map()).Distinct().Count();

        public int LargestDistance()
        {
            var result = 0;

            foreach (var scannerA in this.Scanners)
            {
                foreach (var scannerB in this.Scanners)
                {
                    if (scannerA != scannerB)
                    {
                        int distance = scannerA.Point.Distance(scannerB.Point);

                        if (distance > result)
                        {
                            result = distance;
                        }
                    }
                }
            }

            return result;
        }
    }
}
