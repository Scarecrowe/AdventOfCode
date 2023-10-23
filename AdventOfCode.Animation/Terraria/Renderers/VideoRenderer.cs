namespace AdventOfCode.Animation.Terraria.Renderers
{
    using AdventOfCode.Animation.FFmpeg;
    using AdventOfCode.Animation.FFmpeg.FilterComplex;
    using AdventOfCode.Animation.FFmpeg.Libx264;
    using AdventOfCode.Animation.Terraria.Audio;
    using AdventOfCode.Animation.Terraria.Biomes;

    public class VideoRenderer
    {
        public VideoRenderer(RenderParameters parameters)
        {
            this.Parameters = parameters;
        }

        private FFmpegBuilder? FFmpegBuilder { get; set; }

        private RenderParameters Parameters { get; }

        public FFmpegBuilder Build()
        {
            this.FFmpegBuilder = new FFmpegBuilder()
                .WithOverwrite()
                .WithAnalyzeDuration(40000)
                .WithInput("-")
                    .WithThreadQueueSize(4028)
                    .WithFramerate(WaterFallRenderer.Fps)
                    .WithFormat("image2pipe")
                .Build();

            foreach (KeyValuePair<BiomeType, BiomeAudio> pair in this.Parameters.Playlist)
            {
                this.FFmpegBuilder
                    .WithInput(pair.Value.Path)
                        .WithDuration(pair.Value.Duration + WaterFallRenderer.AudioFadeDuration)
                        .WithStreamLoop()
                    .Build();
            }

            this.FFmpegBuilder
                .WithAudioCodec()
                    .WithAAC()
                        .WithSampleRate(44100)
                    .Build()
                .Build()
                .WithVideoCodec()
                    .WithLibx264()
                        .WithPreset(Libx264Preset.Ultrafast)
                        .WithTune(Libx264Tune.Animation)
                        .WithConstantQuality(20)
                    .Build()
                .Build();

            int index = 0;

            FilterComplexBuilder filterComplexBuilder = this.FFmpegBuilder.WithFilterComplex();

            foreach (KeyValuePair<BiomeType, BiomeAudio> pair in this.Parameters.Playlist)
            {
                filterComplexBuilder
                    .WithInput(++index, $"audio{index}")
                        .WithDelay(pair.Value.Start)
                        .WithVolumeFadeIn(pair.Value.Start, WaterFallRenderer.AudioFadeDuration)
                        .WithVolumeFadeOut(pair.Value.End, WaterFallRenderer.AudioFadeDuration)
                    .Build();
            }

            filterComplexBuilder.WithAmix("audioout");

            return
                filterComplexBuilder.Build()
                .WithMap("0:v")
                .WithMap("[audioout]")
                .WithAsync(1)
                .WithShortest()
                .WithConstantQuality(34)
                .WithOutput($"{Animation.GetRenderPath()}\\{RandomGenerator.Seed}.mp4")
                    .WithFramerate(WaterFallRenderer.Fps)
                .Build();
        }
    }
}
