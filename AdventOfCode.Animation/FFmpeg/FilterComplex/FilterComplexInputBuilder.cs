namespace AdventOfCode.Animation.FFmpeg.FilterComplex
{
    public class FilterComplexInputBuilder
    {
        public FilterComplexInputBuilder(
            FilterComplexBuilder filterComplexBuilder,
            int index,
            string alias)
        {
            this.FilterComplexBuilder = filterComplexBuilder;

            if (index < 0 || index > this.FilterComplexBuilder.FFmpegBuilder.Inputs.Count - 1)
            {
                throw new InvalidOperationException($"No FFMPEG input added with the index: {index}");
            }

            this.Filter = $"[{index}]";
            this.Index = index;
            this.Alias = alias;
        }

        public string Filter { get; private set; }

        public int Index { get; }

        public string Alias { get; }

        private bool HasFilter { get; set; }

        private FilterComplexBuilder FilterComplexBuilder { get; }

        public FilterComplexBuilder Build()
        {
            this.Filter += $"[{this.Alias}];";

            return this.FilterComplexBuilder;
        }

        public FilterComplexInputBuilder WithVolumeFadeIn(int start, int duration)
        {
            if (this.HasFilter)
            {
                this.Filter += ",";
            }

            this.Filter += $"afade=type=in:duration={duration}:start_time={start}";

            this.HasFilter = true;

            return this;
        }

        public FilterComplexInputBuilder WithVolumeFadeOut(int start, int duration)
        {
            if (this.HasFilter)
            {
                this.Filter += ",";
            }

            this.Filter += $"afade=type=out:duration={duration}:start_time={start}";

            this.HasFilter = true;

            return this;
        }

        public FilterComplexInputBuilder WithDelay(int seconds)
        {
            if (this.HasFilter)
            {
                this.Filter += ",";
            }

            this.Filter += $"adelay={seconds * 1000}|{seconds * 1000}";

            this.HasFilter = true;

            return this;
        }
    }
}
