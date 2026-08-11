namespace AdventOfCode.Puzzles._2018.Day_03___No_Matter_How_You_Slice_It
{
    using System.Text;
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core;
    using AdventOfCode.Core.Extensions;
    
    public class NoMatterHowYouSliceIt
    {
        public NoMatterHowYouSliceIt(string[] input)
        {
            this.Claims = Parse(input);
            this.Map = new();
        }

        public NoMatterHowYouSliceIt(string[] input, IFrameRenderer renderer)
            : this(input)
        {
            this.Renderer = renderer;
        }

        public IFrameRenderer? Renderer { get; }

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

                this.RenderFrame();
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

        private void RenderFrame()
        {
            List<string> result = [];

            StringBuilder sb = new();

            int row = 0;

           foreach (VectorCell<int, List<int>> cell in this.Map.AxisEnumerator())
           {
                if (cell.Point.Y.ToInt() > row)
                {
                    row = cell.Point.Y.ToInt();
                    result.Add(sb.ToString());
                    sb.Clear();
                }

                if (cell.Value.Count() == 0)
                {
                    sb.Append($" ");
                }
                else
                {
                    sb.Append(cell.Value.Count());
                }
            }

            result.Add(sb.ToString());
            this.Renderer?.RenderFrame(new Frame([.. result]));
        }
    }
}
