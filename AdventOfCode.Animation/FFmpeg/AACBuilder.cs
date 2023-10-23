namespace AdventOfCode.Animation.FFmpeg
{
    public class AACBuilder
    {
        public AACBuilder(AudioCodecBuilder audioCodecBuilder)
        {
            this.AudioCodecBuilder = audioCodecBuilder;
            this.Arguments = new();
        }

        public List<string> Arguments { get; }

        public int SampleRate { get; private set; }

        private AudioCodecBuilder AudioCodecBuilder { get; }

        public AACBuilder WithSampleRate(int sampleRate)
        {
            this.Arguments.Add("-ar");
            this.Arguments.Add($"{sampleRate}");

            return this;
        }

        public AudioCodecBuilder Build()
        {
            this.AudioCodecBuilder.Arguments.AddRange(this.Arguments);

            return this.AudioCodecBuilder;
        }
    }
}
