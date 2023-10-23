namespace AdventOfCode.Animation.FFmpeg
{
    using AdventOfCode.Animation.FFmpeg.FilterComplex;

    public class InputBuilder
    {
        public InputBuilder(FFmpegBuilder ffmpegBuilder, int index, string source)
        {
            this.FfmpegBuilder = ffmpegBuilder;
            this.Arguments = new();
            this.Index = index;

            this.Source = source.IndexOf(" ") > -1
                ? $"\"{source}\""
                : source;
        }

        public List<string> Arguments { get; }

        public int Index { get; }

        private FFmpegBuilder FfmpegBuilder { get; }

        private string Source { get; }

        public FFmpegBuilder Build()
        {
            this.Arguments.Add("-i");
            this.Arguments.Add(this.Source);

            this.FfmpegBuilder.AddInput(this);

            return this.FfmpegBuilder;
        }

        public InputBuilder WithFormat(string format)
        {
            this.Arguments.Add("-f");
            this.Arguments.Add(format);

            return this;
        }

        public InputBuilder WithFramerate(int framerate)
        {
            this.Arguments.Add("-framerate");
            this.Arguments.Add($"{framerate}");

            return this;
        }

        public InputBuilder WithR(int framerate)
        {
            this.Arguments.Add("-r");
            this.Arguments.Add($"{framerate}");

            return this;
        }

        public InputBuilder WithThreadQueueSize(int size)
        {
            this.Arguments.Add("-thread_queue_size");
            this.Arguments.Add($"{size}");

            return this;
        }

        public InputBuilder WithStreamLoop(int count = -1)
        {
            this.Arguments.Add("-stream_loop");
            this.Arguments.Add($"{count}");

            return this;
        }

        public InputBuilder WithLoop(int count = -1)
        {
            this.Arguments.Add("-loop");
            this.Arguments.Add($"{count}");

            return this;
        }

        public InputBuilder WithDuration(int seconds)
        {
            this.Arguments.Add("-t");
            this.Arguments.Add($"{seconds}");

            return this;
        }
    }
}
