namespace AdventOfCode.Puzzles._2018.Day_03___No_Matter_How_You_Slice_It
{
    using AdventOfCode.Core;

    public class NoMatterHowYouSliceIt
    {
        public NoMatterHowYouSliceIt(string[] input)
        {
            this.Claims = Parse(input);
            this.Map = new();
        }

        public List<Claim> Claims { get; }

        private VectorDictionary<int, List<int>> Map { get; }

        public static List<Claim> Parse(string[] input) => input.Select(x => new Claim(x)).ToList();

        public NoMatterHowYouSliceIt PlotClaims()
        {
            foreach (Claim claim in this.Claims)
            {
                for (long y = claim.Point.Y; y < claim.Point.Y + claim.Height; y++)
                {
                    for (long x = claim.Point.X; x < claim.Point.X + claim.Width; x++)
                    {
                        if (!this.Map.ContainsKey(new(x, y)))
                        {
                            this.Map.Add(new(x, y), new());
                        }

                        this.Map[new(x, y)].Add(claim.Id);
                    }
                }
            }

            return this;
        }

        public int OverlappingClaims() => this.Map.Count(x => x.Value.Count > 1);

        public int NonOverlappingClaim()
        {
            foreach (Claim claim in this.Claims)
            {
                bool overlap = false;

                for (long y = claim.Point.Y; y < claim.Point.Y + claim.Height; y++)
                {
                    for (long x = claim.Point.X; x < claim.Point.X + claim.Width; x++)
                    {
                        if (!(this.Map[new(x, y)].Count == 1 && this.Map[new(x, y)][0] == claim.Id))
                        {
                            overlap = true;
                            break;
                        }
                    }

                    if (overlap)
                    {
                        break;
                    }
                }

                if (!overlap)
                {
                    return claim.Id;
                }
            }

            return -1;
        }
    }
}
