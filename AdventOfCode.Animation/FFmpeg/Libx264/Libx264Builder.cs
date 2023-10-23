namespace AdventOfCode.Animation.FFmpeg.Libx264
{
    public class Libx264Builder
    {
        public Libx264Builder(VideoCodecBuilder videoCodecBuilder)
        {
            this.VideoCodecBuilder = videoCodecBuilder;
            this.Arguments = new();
        }

        private List<string> Arguments { get; }

        private VideoCodecBuilder VideoCodecBuilder { get; }

        public VideoCodecBuilder Build()
        {
            this.VideoCodecBuilder.Arguments.AddRange(this.Arguments);

            return this.VideoCodecBuilder;
        }

        public Libx264Builder WithPreset(Libx264Preset preset = Libx264Preset.Medium)
        {
            this.Arguments.Add("-preset");
            this.Arguments.Add($"{preset}".ToLower());

            return this;
        }

        public Libx264Builder WithTune(Libx264Tune tune)
        {
            this.Arguments.Add("-tune");
            this.Arguments.Add($"{tune}".ToLower());

            return this;
        }

        public Libx264Builder WithProfile(Libx264Profile profile)
        {
            this.Arguments.Add("-profile");
            this.Arguments.Add($"{profile}".ToLower());

            return this;
        }

        public Libx264Builder WithConstantQuality(int crf)
        {
            this.Arguments.Add("-crf");
            this.Arguments.Add($"{crf}");

            return this;
        }

        public Libx264Builder WithFastStart()
        {
            this.Arguments.Add("-movflags");
            this.Arguments.Add($"+faststart");

            return this;
        }
    }
}
