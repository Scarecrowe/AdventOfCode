namespace AdventOfCode.Animation.FFmpeg
{
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Text;
    using AdventOfCode.Animation.FFmpeg.FilterComplex;

    public class FFmpegBuilder
    {
        public FFmpegBuilder()
        {
            this.Inputs = new();
            this.Outputs = new();
            this.GenericArguments = new();
            this.EncodingArguments = new();
            this.Maps = new();
        }

        public List<InputBuilder> Inputs { get; }

        public List<OutputBuilder> Outputs { get; }

        private List<string> GenericArguments { get; }

        private List<string> EncodingArguments { get; }

        private List<string> Maps { get; }

        private AudioCodecBuilder? AudioCodecBuilder { get; set; }

        private VideoCodecBuilder? VideoCodecBuilder { get; set; }

        private FilterComplexBuilder? FilterComplexBuilder { get; set; }

        private Process? Process { get; set; }

        private BinaryWriter? StandardInputWriter { get; set; }

        public InputBuilder WithInput(string source)
        {
            return new InputBuilder(this, this.Inputs.Count, source);
        }

        public OutputBuilder WithOutput(string output)
        {
            return new OutputBuilder(this, output);
        }

        public FFmpegBuilder WithAnalyzeDuration(int duration)
        {
            this.GenericArguments.Add("-analyzeduration");
            this.GenericArguments.Add($"{duration}");

            return this;
        }

        public FFmpegBuilder WithProbeSize(int size)
        {
            this.GenericArguments.Add("-probesize");
            this.GenericArguments.Add($"{size}");

            return this;
        }

        public FilterComplexBuilder WithFilterComplex()
        {
            this.FilterComplexBuilder = new FilterComplexBuilder(this);

            return this.FilterComplexBuilder;
        }

        public VideoCodecBuilder WithVideoCodec()
        {
            this.VideoCodecBuilder = new VideoCodecBuilder(this);

            return this.VideoCodecBuilder;
        }

        public AudioCodecBuilder WithAudioCodec()
        {
            this.AudioCodecBuilder = new AudioCodecBuilder(this);

            return this.AudioCodecBuilder;
        }

        public FFmpegBuilder WithOverwrite()
        {
            this.GenericArguments.Add("-y");

            return this;
        }

        public FFmpegBuilder WithPixelFormat(string format)
        {
            this.EncodingArguments.Add("-pix_fmt");
            this.EncodingArguments.Add(format);

            return this;
        }

        public FFmpegBuilder WithConstantQuality(int quality)
        {
            this.EncodingArguments.Add("-crf");
            this.EncodingArguments.Add($"{quality}");

            return this;
        }

        public FFmpegBuilder WithMap(string map)
        {
            this.Maps.Add(map);

            return this;
        }

        public FFmpegBuilder WithAsync(int value)
        {
            this.EncodingArguments.Add("-async");
            this.EncodingArguments.Add($"{value}");

            return this;
        }

        public FFmpegBuilder WithShortest()
        {
            this.EncodingArguments.Add("-shortest");
            this.EncodingArguments.Add($"-fflags");
            this.EncodingArguments.Add($"+shortest");

            return this;
        }

        public FFmpegBuilder Run()
        {
            Console.WriteLine("Starting FFMPEG");
            this.Process = new();
            this.Process.StartInfo.FileName = "ffmpeg.exe";
            this.Process.StartInfo.Arguments = string.Join(" ", this.BuildArguments());
            this.Process.StartInfo.UseShellExecute = false;
            this.Process.StartInfo.CreateNoWindow = false;
            this.Process.StartInfo.RedirectStandardInput = true;
            this.Process.Start();

            this.StandardInputWriter = new(this.Process.StandardInput.BaseStream);

            return this;
        }

        public void AddInput(InputBuilder inputBuilder) => this.Inputs.Add(inputBuilder);

        public void AddOutput(OutputBuilder outputBuilder) => this.Outputs.Add(outputBuilder);

        public void WriteBitmap(Bitmap bmp, ImageFormat imageFormat)
        {
            if (this.StandardInputWriter == null)
            {
                return;
            }

            bmp.Save(this.StandardInputWriter.BaseStream, imageFormat);
        }

        public void Quit()
        {
            if (this.Process == null
                || this.Process.HasExited)
            {
                return;
            }

            this.Process.StandardInput.Write("q");
        }

        private void BuildInputArguments(List<string> arguments)
        {
            foreach (InputBuilder input in this.Inputs)
            {
                arguments.AddRange(input.Arguments);
            }
        }

        private void BuildFilterComplexArguments(List<string> arguments)
        {
            if (this.FilterComplexBuilder != null)
            {
                arguments.Add("-filter_complex");

                StringBuilder sb = new();

                foreach (FilterComplexInputBuilder filter in this.FilterComplexBuilder.Filters)
                {
                    sb.Append(filter.Filter);
                }

                if (this.FilterComplexBuilder.Amix != null)
                {
                    foreach (FilterComplexInputBuilder filter in this.FilterComplexBuilder.Filters)
                    {
                        sb.Append($"[{filter.Alias}]");
                    }

                    sb.Append($"amix={this.FilterComplexBuilder.Amix.Count}[{this.FilterComplexBuilder.Amix.Alias}]");
                }

                arguments.Add(sb.ToString());
            }

            if (arguments[^1].EndsWith(";"))
            {
                arguments[^1] = arguments[^1][..^1];
            }
        }

        private void BuildMapArguments(List<string> arguments)
        {
            foreach (string map in this.Maps)
            {
                arguments.Add("-map");
                arguments.Add(map);
            }
        }

        private void BuildAudioCodecArguments(List<string> arguments)
        {
            if (this.AudioCodecBuilder != null)
            {
                arguments.AddRange(this.AudioCodecBuilder.Arguments);
            }
        }

        private void BuildVideoCodecArguments(List<string> arguments)
        {
            if (this.VideoCodecBuilder != null)
            {
                arguments.AddRange(this.VideoCodecBuilder.Arguments);
            }
        }

        private void BuildOutputArguments(List<string> arguments)
        {
            foreach (OutputBuilder output in this.Outputs)
            {
                arguments.AddRange(output.Arguments);
            }
        }

        private List<string> BuildArguments()
        {
            List<string> result = new();

            result.AddRange(this.GenericArguments);

            this.BuildInputArguments(result);
            this.BuildFilterComplexArguments(result);
            this.BuildMapArguments(result);

            result.AddRange(this.EncodingArguments);

            this.BuildAudioCodecArguments(result);
            this.BuildVideoCodecArguments(result);
            this.BuildOutputArguments(result);

            return result;
        }
    }
}
