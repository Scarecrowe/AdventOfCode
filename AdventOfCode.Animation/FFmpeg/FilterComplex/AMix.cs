namespace AdventOfCode.Animation.FFmpeg.FilterComplex
{
    public class AMix
    {
        public AMix(string alias, int count)
        {
            this.Alias = alias;
            this.Count = count;
        }

        public string Alias { get; }

        public int Count { get; }
    }
}
