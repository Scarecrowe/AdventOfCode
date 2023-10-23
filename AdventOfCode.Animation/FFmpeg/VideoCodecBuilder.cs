namespace AdventOfCode.Animation.FFmpeg
{
    using AdventOfCode.Animation.FFmpeg.Libx264;

    public class VideoCodecBuilder
    {
        public VideoCodecBuilder(FFmpegBuilder ffmpegBuilder)
        {
            this.FFmpegBuilder = ffmpegBuilder;
            this.Arguments = new();
        }

        public List<string> Arguments { get; }

        private FFmpegBuilder FFmpegBuilder { get; }

        public FFmpegBuilder Build()
        {
            return this.FFmpegBuilder;
        }

        public Libx264Builder WithLibx264()
        {
            this.Arguments.Add("-c:v");
            this.Arguments.Add("libx264");

            return new Libx264Builder(this);
        }
    }
}
