namespace AdventOfCode.Puzzles._2019.Day_21___Springdroid_Adventure
{
    using AdventOfCode.Animation.Renderers;
    using AdventOfCode.Puzzles._2019.IntCode;
    using System.Text;

    public class SpringdroidAdventure
    {
        public SpringdroidAdventure(string program) => this.Cpu = new IntcodeCpu(program);

        public SpringdroidAdventure(string program, IFrameRenderer renderer)
            : this(program)
        {
            this.Renderer = renderer;
        }

        private string Program { get; }

        private IFrameRenderer? Renderer { get; }

        private IntcodeCpu Cpu { get; }

        public long Run()
        {
            this.Cpu.RunAsciiCommand();
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("NOT C J"));
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("AND D J"));
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("NOT A T"));
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("OR T J"));
            this.Cpu.RunAsciiCommand("WALK");

            return this.Cpu.Output.FirstOrDefault(c => c > 255);
        }

        public long RunWihtInreasedSensor()
        {
            this.Cpu.RunAsciiCommand();
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("NOT C J"));
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("AND D J"));
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("AND H J"));
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("NOT B T"));
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("AND D T"));
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("OR T J"));
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("NOT A T"));
            this.Cpu.Input.EnqueueRange(IntcodeCpu.StringToAscii("OR T J"));
            this.Cpu.RunAsciiCommand("RUN");

            return this.Cpu.Output.FirstOrDefault(c => c > 255);
        }

        public SpringdroidAdventure RenderGold()
        {
            long damage = this.RunWihtInreasedSensor();

            List<string[]> frames = this.BuildHullAnimation(
                "SPRINGDROID ADVENTURE // RUN",
                "#####.###########.###.#######.###.###########",
                ShouldJumpGold,
                damage);

            this.RenderPaddedFrames(frames);

            return this;
        }

        public SpringdroidAdventure RenderSilver()
        {
            long damage = this.Run();

            List<string[]> frames = this.BuildHullAnimation(
                "SPRINGDROID ADVENTURE // WALK",
                "#####.###########.###.#######.############",
                ShouldJumpSilver,
                damage);

            this.RenderPaddedFrames(frames);

            return this;
        }

        private string[] BuildHullFrame(
            string title,
            string ground,
            int droidX,
            int droidY,
            long damage = 0)
        {
            const int viewWidth = 17;
            const int viewHeight = 3;

            List<string> result = [];

            result.Add(title);
            result.Add(string.Empty);

            for (int y = 0; y < viewHeight; y++)
            {
                StringBuilder row = new();

                for (int x = 0; x < viewWidth; x++)
                {
                    row.Append(x == droidX && y == droidY ? '@' : '.');
                }

                result.Add(row.ToString());
            }

            StringBuilder groundRow = new();

            for (int x = 0; x < viewWidth; x++)
            {
                groundRow.Append(ground[x]);
            }

            if (droidY == 3 && droidX >= 0 && droidX < viewWidth)
            {
                groundRow[droidX] = '@';
            }

            result.Add(groundRow.ToString());

            if (damage > 0)
            {
                result.Add(string.Empty);
                result.Add($"HULL DAMAGE: {damage}");
            }

            return [.. result];
        }

        private List<string[]> BuildHullAnimation(
            string title,
            string ground,
            Func<string, int, bool> shouldJump,
            long damage)
        {
            List<string[]> frames = [];

            int x = 0;
            int y = 2;
            int velocityY = 0;

            while (x < ground.Length - 1)
            {
                string view = ground[x..Math.Min(x + 17, ground.Length)].PadRight(17, '#');

                frames.Add(this.BuildHullFrame(title, view, 0, y));

                bool onGround = y == 2;

                if (onGround && shouldJump(ground, x))
                {
                    velocityY = -2;
                }

                x++;

                if (velocityY < 0)
                {
                    y--;
                    velocityY++;
                }
                else if (y < 2)
                {
                    y++;
                }

                if (ground[x] == '.' && y == 2)
                {
                    frames.Add(this.BuildHullFrame(title, view, Math.Min(16, x), 3));
                    break;
                }

                if (x >= ground.Length - 1)
                {
                    frames.Add(this.BuildHullFrame(title, view, 0, y, damage));
                    break;
                }
            }

            return frames;
        }

        private static bool ShouldJumpSilver(string ground, int x)
        {
            bool A = IsGround(ground, x + 1);
            bool C = IsGround(ground, x + 3);
            bool D = IsGround(ground, x + 4);

            return (!C && D) || !A;
        }

        private static bool ShouldJumpGold(string ground, int x)
        {
            bool A = IsGround(ground, x + 1);
            bool B = IsGround(ground, x + 2);
            bool C = IsGround(ground, x + 3);
            bool D = IsGround(ground, x + 4);
            bool H = IsGround(ground, x + 8);

            return ((!C && D && H) || (!B && D) || !A);
        }

        private static bool IsGround(string ground, int index)
            => index >= ground.Length || ground[index] == '#';

        private void RenderPaddedFrames(List<string[]> frames)
        {
            int width = frames.SelectMany(frame => frame).Max(row => row.Length);
            int height = frames.Max(frame => frame.Length);

            foreach (string[] frame in frames)
            {
                this.Renderer?.RenderFrame(
                    new Frame(PadFrame(frame, width, height)));
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
