namespace AdventOfCode.Puzzles._2019.Day_25___Cryostasis
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Core.Extensions;
    using AdventOfCode.Puzzles._2019.IntCode;

    public class Cryostasis
    {
        private static readonly string[] Commands =
        [
            "north",
            "north",
            "north",
            "take mutex",
            "south",
            "south",
            "east",
            "north",
            "take loom",
            "south",
            "west",
            "south",
            "west",
            "west",
            "take sand",
            "south",
            "east",
            "north",
            "take wreath",
            "south",
            "west",
            "north",
            "north",
            "east",
            "east"
        ];

        public Cryostasis(string program) => this.Cpu = new(program);

        public Cryostasis(string program, IFrameRenderer renderer)
            : this(program)
        {
            this.Renderer = renderer;
        }

        public IntcodeCpu Cpu { get; }

        public IFrameRenderer? Renderer { get; }

        public string Run()
        {
            this.Cpu.RunAsciiCommand();

            foreach (string command in Commands[..^1])
            {
                this.Cpu.RunAsciiCommand(command);
            }

            this.Cpu.Output.Clear();
            this.Cpu.RunAsciiCommand(Commands[^1]);

            return this.Cpu.Output.Select(c => (char)c).ToArray().Join().Numbers();
        }

        public Cryostasis RenderSilver()
        {
            if (this.Renderer == null)
            {
                return this;
            }

            List<string[]> frames = [];
            List<string> transcript = [];

            this.Cpu.RunAsciiCommand();
            transcript.AddRange(this.ReadOutputLines());

            frames.Add(this.BuildFrame(transcript, "CRYOSTASIS // DROID BOOT"));

            for (int i = 0; i < Commands.Length; i++)
            {
                string command = Commands[i];

                this.Cpu.Output.Clear();
                this.Cpu.RunAsciiCommand(command);

                transcript.Add($"> {command}");
                transcript.Add("");
                transcript.AddRange(this.ReadOutputLines());

                string title = i == Commands.Length - 1
                    ? "CRYOSTASIS // PRESSURE SENSOR"
                    : $"CRYOSTASIS // COMMAND {i + 1:00}/{Commands.Length:00}";

                string[] frame = this.BuildFrame(transcript, title);

                for(int j = 0; j < 10; j++)
                {
                    frames.Add(frame);
                }
            }

            frames.Add(this.BuildFrame(transcript, "CRYOSTASIS // AIRLOCK CODE ACQUIRED"));

            this.RenderPaddedFrames(frames);

            return this;
        }

        private List<string> ReadOutputLines()
        {
            return this.Cpu.Output
                .Select(c => (char)c)
                .ToArray()
                .Join()
                .Replace("\r", string.Empty)
                .Split('\n')
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToList();
        }

        private string[] BuildFrame(List<string> transcript, string title)
        {
            const int visibleLines = 28;
            const int maxWidth = 86;

            List<string> result = [];

            result.Add(title);
            result.Add(new string('-', maxWidth));

            foreach (string line in transcript.TakeLast(visibleLines))
            {
                result.Add(this.TrimLine(line, maxWidth));
            }

            return [.. result];
        }

        private string TrimLine(string line, int width)
        {
            if (line.Length <= width)
            {
                return line;
            }

            return line[..(width - 1)] + "…";
        }

        private void RenderPaddedFrames(List<string[]> frames)
        {
            int width = frames.SelectMany(frame => frame).Max(row => row.Length);
            int height = frames.Max(frame => frame.Length);

            foreach (string[] frame in frames)
            {
                this.Renderer?.RenderFrame(new Frame(PadFrame(frame, width, height)));
            }
        }

        private static string[] PadFrame(string[] frame, int width, int height)
        {
            List<string> result = [];

            foreach (string row in frame)
            {
                result.Add(row.PadRight(width, ' '));
            }

            while (result.Count < height)
            {
                result.Add(new string(' ', width));
            }

            return [.. result];
        }
    }
}