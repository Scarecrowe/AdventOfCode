namespace AdventOfCode.Animation.FFmpeg
{
    public class OutputBuilder
    {
        public OutputBuilder(FFmpegBuilder ffmpegBuilder, string output)
        {
            this.FFmpegBuilder = ffmpegBuilder;
            this.Arguments = new();

            this.Output = output.IndexOf(" ") > -1
                ? $"\"{output}\""
                : output;
        }

        public List<string> Arguments { get; }

        private FFmpegBuilder FFmpegBuilder { get; }

        private string Output { get; }

        public FFmpegBuilder Build()
        {
            this.Arguments.Add(this.Output);

            this.FFmpegBuilder.AddOutput(this);

            return this.FFmpegBuilder;
        }

        public OutputBuilder WithFormat(string format)
        {
            this.Arguments.Add("-f");
            this.Arguments.Add(format);

            return this;
        }

        public OutputBuilder WithDuration(TimeSpan duration)
        {
            this.Arguments.Add("-t");
            this.Arguments.Add(duration.ToString());

            return this;
        }

        public OutputBuilder WithFramerate(int framerate)
        {
            this.Arguments.Add("-r");
            this.Arguments.Add($"{framerate}");

            return this;
        }
    }
}
