namespace AdventOfCode.Animation.FFmpeg
{
    public class AudioCodecBuilder
    {
        public AudioCodecBuilder(FFmpegBuilder ffmpegBuilder)
        {
            this.FFmpegBuilder = ffmpegBuilder;
            this.Arguments = new();
        }

        public List<string> Arguments { get; }

        private FFmpegBuilder FFmpegBuilder { get; }

        public AACBuilder WithAAC()
        {
            this.Arguments.Add("-c:a");
            this.Arguments.Add("aac");

            return new AACBuilder(this);
        }

        public FFmpegBuilder Build()
        {
            return this.FFmpegBuilder;
        }
    }
}
