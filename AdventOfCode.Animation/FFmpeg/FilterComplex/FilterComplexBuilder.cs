namespace AdventOfCode.Animation.FFmpeg.FilterComplex
{
    public class FilterComplexBuilder
    {
        public FilterComplexBuilder(FFmpegBuilder ffmpegBuilder)
        {
            this.FFmpegBuilder = ffmpegBuilder;
            this.Filters = new();
        }

        public FFmpegBuilder FFmpegBuilder { get; }

        public AMix? Amix { get; private set; }

        public List<FilterComplexInputBuilder> Filters { get; private set; }

        public FFmpegBuilder Build()
        {
            return this.FFmpegBuilder;
        }

        public FilterComplexInputBuilder WithInput(int index, string alias)
        {
            FilterComplexInputBuilder result = new(this, index, alias);

            this.Filters.Add(result);

            return result;
        }

        public FilterComplexBuilder WithAmix(string alias)
        {
            this.Amix = new AMix(alias, this.Filters.Count);

            return this;
        }
    }
}
