namespace AdventOfCode.Animation.Renderers.AsciiRenderer
{
    using AdventOfCode.Animation.FFmpeg;
    using AdventOfCode.Animation.FFmpeg.Libx264;
    using AdventOfCode.Core;

    public class AsciiVideoRenderer
    {
        public AsciiVideoRenderer(
            int year,
            int day,
            int fps,
            SolutionType solutionType)
        {
            this.Year = year;
            this.Day = day;
            this.Fps = fps;
            this.SolutionType = solutionType;
        }

        private int Year { get; set; }

        private int Day { get; set; }

        private int Fps { get; set; }

        private SolutionType SolutionType { get; set; }

        private FFmpegBuilder? FFmpegBuilder { get; set; }

        public FFmpegBuilder BuildMp4()
        {
            this.FFmpegBuilder = new FFmpegBuilder()
                .WithOverwrite()
                .WithAnalyzeDuration(40000)
                .WithInput("-")
                    .WithThreadQueueSize(4028)
                    .WithFramerate(this.Fps)
                    .WithFormat("image2pipe")
                    .WithPair("-c:v", "mjpeg")
                .Build()
                .WithNullAudio()
                .WithVideoCodec()
                    .WithLibx264()
                        .WithPreset(Libx264Preset.Ultrafast)
                        .WithTune(Libx264Tune.Animation)
                        .WithConstantQuality(20)
                    .Build()
                .Build()
                .WithMap("0:v")
                .WithAsync(1)
                .WithConstantQuality(34)
                .WithOutput($"{Animation.GetRenderPath(this.Year, this.Day, Puzzle.GetTitle(this.Year, this.Day))}\\ascii-{this.SolutionType}.mp4".ToLower())
                    .WithFramerate(this.Fps)
                .Build();

            return this.FFmpegBuilder;
        }

        public FFmpegBuilder BuildGif()
        {
            this.FFmpegBuilder = new FFmpegBuilder()
                .WithOverwrite()
                .WithAnalyzeDuration(40000)
                .WithInput("-")
                    .WithThreadQueueSize(4028)
                    .WithFramerate(this.Fps)
                    .WithFormat("image2pipe")
                .Build()
                .WithNullAudio()
                .WithVideoCodec()
                    .WithGif()
                .WithPair("-vf", $"\"fps={this.Fps},scale=in_range=pc:out_range=pc,format=rgb24\"")
                .WithOutput($"{Animation.GetRenderPath(this.Year, this.Day, Puzzle.GetTitle(this.Year, this.Day))}\\ascii-{this.SolutionType}.gif".ToLower())
                    .WithFramerate(this.Fps)
                .Build();

            return this.FFmpegBuilder;
        }
    }
}
